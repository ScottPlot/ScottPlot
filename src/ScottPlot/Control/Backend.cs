/* This file describes the ScottPlot back-end control module.
 * 
 *  Goals for this module:
 *    - handle interact with the Plot object so controls don't have to
 *    - single location for mouse interaction logic so controls don't have to implement it
 *    - use events to tell controls when to update the image or change the mouse cursor
 *    - TODO: render calls should be non-blocking so GUI/controls aren't slowed by render requests
 *    - TODO: a timer should ask for a high quality render after mouse interaction stops
 *   
 *   Default Controls:
 *   
 *    - Left-click-drag: pan
 *    - Right-click-drag: zoom
 *    - Middle-click-drag: zoom region
 *    - ALT+Left-click-drag: zoom region
 *    - Scroll wheel: zoom to cursor
 *   
 *    - Right-click: show menu
 *    - Middle-click: auto-axis
 *    - Double-click: show benchmark
 *   
 *    - CTRL+Left-click-drag to pan horizontally
 *    - SHIFT+Left-click-drag to pan vertically
 *    - CTRL+Right-click-drag to zoom horizontally
 *    - SHIFT+Right-click-drag to zoom vertically
 *    - CTRL+SHIFT+Right-click-drag to zoom evenly
 *    - SHIFT+click-drag draggables for fixed-size dragging
 *    
 *  Configurable options:
 *  
 *    - left-click-drag pan
 *    - right-click-drag zoom
 *    - lock vertical or horizontal axis
 *    - middle-click auto-axis margin
 *    - double-click benchmark toggle
 *    - scrollwheel zoom
 *    - low quality (never / while dragging / always)
 *    - delayed high quality render after scrollwheel
 *   
 */

using ScottPlot.Control.EventProcess;
using ScottPlot.Control.EventProcess.Factories;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    public class ControlBackEnd
    {
        public event EventHandler BitmapUpdated = delegate { };
        public event EventHandler BitmapChanged = delegate { };
        public event EventHandler CursorChanged = delegate { };
        public event EventHandler AxesChanged = delegate { };
        public event EventHandler RightClicked = delegate { };

        public readonly Configuration Configuration = new Configuration();
        public ScottPlot.Plot Plot { get; private set; }

        private ScottPlot.Settings Settings;
        private System.Drawing.Bitmap Bmp;
        private readonly List<System.Drawing.Bitmap> OldBitmaps = new List<System.Drawing.Bitmap>();
        public ScottPlot.Cursor Cursor { get; private set; } = ScottPlot.Cursor.Arrow;

        private readonly Queue<InputState> MouseWheelQueue = new Queue<InputState>();
        private readonly Stopwatch MouseWheelStopwatch = new Stopwatch();
        private EventsProcessor eventProcessor;
        private IUIEventFactory eventFactory;


        public ControlBackEnd(float width, float height)
        {
            eventFactory = new ModernDecorator(new UIEventFactory(Configuration, Settings, Plot));
            Reset(width, height);

            // create an event processor and later request new renders by interacting with it.
            eventProcessor = new EventsProcessor(
                    renderAction: (lowQuality) => Render(lowQuality),
                    renderDelay: (int)Configuration.ScrollWheelZoomHighQualityDelay);
        }

        public void Reset(float width, float height) =>
            Reset(width, height, new Plot());

        /// <summary>
        /// Return a copy of the list of draggable plottables
        /// </summary>
        /// <returns></returns>
        private IDraggable[] GetDraggables() =>
            Settings.Plottables.Where(x => x is IDraggable).Select(x => (IDraggable)x).ToArray();

        /// <summary>
        /// Return the draggable plottable under the mouse cursor (or null if there isn't one)
        /// </summary>
        private IDraggable GetDraggableUnderMouse(double pixelX, double pixelY, int snapDistancePixels = 5)
        {
            double snapWidth = Settings.XAxis.Dims.UnitsPerPx * snapDistancePixels;
            double snapHeight = Settings.YAxis.Dims.UnitsPerPx * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(Plot.GetCoordinateX((float)pixelX), Plot.GetCoordinateY((float)pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }

        public static string GetHelpMessage()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Left-click-drag: pan");
            sb.AppendLine("Right-click-drag: zoom");
            sb.AppendLine("Middle-click-drag: zoom region");
            sb.AppendLine("ALT+Left-click-drag: zoom region");
            sb.AppendLine("Scroll wheel: zoom to cursor");
            sb.AppendLine("");
            sb.AppendLine("Right-click: show menu");
            sb.AppendLine("Middle-click: auto-axis");
            sb.AppendLine("Double-click: show benchmark");
            sb.AppendLine("");
            sb.AppendLine("CTRL+Left-click-drag to pan horizontally");
            sb.AppendLine("SHIFT+Left-click-drag to pan vertically");
            sb.AppendLine("CTRL+Right-click-drag to zoom horizontally");
            sb.AppendLine("SHIFT+Right-click-drag to zoom vertically");
            sb.AppendLine("CTRL+SHIFT+Right-click-drag to zoom evenly");
            sb.AppendLine("SHIFT+click-drag draggables for fixed-size dragging");
            return sb.ToString();
        }

        public void Reset(float width, float height, Plot newPlot)
        {
            Plot = newPlot;
            Settings = Plot.GetSettings(false);
            eventFactory.plt = Plot;
            eventFactory.settings = Settings;
            Resize(width, height);
        }

        public System.Drawing.Bitmap GetLatestBitmap()
        {
            foreach (System.Drawing.Bitmap bmp in OldBitmaps)
                bmp?.Dispose();
            OldBitmaps.Clear();
            return Bmp;
        }

        private void NewBitmap(float width, float height)
        {
            if (width < 1 || height < 1)
                return;

            // Disposing a Bitmap the GUI is displaying will cause an exception.
            // Keep track of old bitmaps so they can be disposed of later.
            OldBitmaps.Add(Bmp);
            Bmp = new System.Drawing.Bitmap((int)width, (int)height);
            BitmapChanged(this, EventArgs.Empty);
        }

        private AxisLimits LimitsOnLastRender = new AxisLimits();
        private int PlottableCountOnLastRender = -1;
        private bool currentlyRendering = false;
        public void Render(bool lowQuality = false, bool skipIfCurrentlyRendering = false)
        {
            if (Bmp is null)
                return;

            if (currentlyRendering && skipIfCurrentlyRendering)
                return;
            currentlyRendering = true;

            //Debug.WriteLine("Render called by:" + new StackTrace().GetFrame(1).GetMethod().Name);

            if (Configuration.Quality == QualityMode.High)
                lowQuality = false;
            else if (Configuration.Quality == QualityMode.Low)
                lowQuality = true;

            PlottableCountOnLastRender = Settings.Plottables.Count;
            Plot.Render(Bmp, lowQuality);

            ScottPlot.AxisLimits newLimits = Plot.GetAxisLimits();
            if (!newLimits.Equals(LimitsOnLastRender) && Configuration.AxesChangedEventEnabled)
                AxesChanged(null, EventArgs.Empty);
            LimitsOnLastRender = newLimits;

            BitmapUpdated(null, EventArgs.Empty);
            currentlyRendering = false;
        }

        public void RenderIfPlottableCountChanged()
        {
            if (Bmp is null)
                return;
            if (Settings.Plottables.Count != PlottableCountOnLastRender)
                Render();
        }

        public void Resize(float width, float height)
        {
            NewBitmap(width, height);
            Render(false);
        }

        private bool IsMiddleDown;
        private bool IsRightDown;
        private bool IsLeftDown;
        private ScottPlot.Plottable.IDraggable PlottableBeingDragged = null;
        public void MouseDown(InputState input)
        {
            if (!Settings.AllAxesHaveBeenSet)
                Plot.SetAxisLimits(Plot.GetAxisLimits());
            IsMiddleDown = input.MiddleWasJustPressed;
            IsRightDown = input.RightWasJustPressed;
            IsLeftDown = input.LeftWasJustPressed;
            PlottableBeingDragged = GetDraggableUnderMouse(input.X, input.Y);
            Settings.MouseDown(input.X, input.Y);
        }

        private float MouseLocationX;
        private float MouseLocationY;

        public (double x, double y) GetMouseCoordinates()
        {
            (double x, double y) = Plot.GetCoordinate(MouseLocationX, MouseLocationY);
            return (double.IsNaN(x) ? 0 : x, double.IsNaN(y) ? 0 : y);
        }

        public (float x, float y) GetMousePixel() => (MouseLocationX, MouseLocationY);

        private bool IsZoomingRectangle;
        private bool IsZoomingWithAlt;
        public void MouseMove(InputState input)
        {
            bool altWasLifted = IsZoomingWithAlt && !input.AltDown;
            if (IsZoomingRectangle && altWasLifted)
                Settings.ZoomRectangle.Clear();

            IsZoomingWithAlt = IsLeftDown && input.AltDown;
            bool isMiddleClickDragZooming = IsMiddleDown;
            bool isZooming = IsZoomingWithAlt || isMiddleClickDragZooming;
            IsZoomingRectangle = isZooming && Configuration.MiddleClickDragZoom;

            MouseLocationX = input.X;
            MouseLocationY = input.Y;
            if (PlottableBeingDragged != null)
                eventProcessor.Process(eventFactory.CreatePlottableDrag(input.X, input.Y, input.ShiftDown, PlottableBeingDragged));
            else if (IsLeftDown && !input.AltDown && Configuration.LeftClickDragPan)
                eventProcessor.Process(eventFactory.CreateMousePan(input));
            else if (IsRightDown && Configuration.RightClickDragZoom)
                eventProcessor.Process(eventFactory.CreateMouseZoom(input));
            else if (IsZoomingRectangle)
                eventProcessor.Process(eventFactory.CreateMouseMovedToZoomRectangle(input.X, input.Y));
            else
                MouseMovedWithoutInteraction(input);
        }

        private void MouseMovedWithoutInteraction(InputState input)
        {
            UpdateCursor(input);
        }

        private void UpdateCursor(InputState input)
        {
            var draggableUnderCursor = GetDraggableUnderMouse(input.X, input.Y);
            Cursor = (draggableUnderCursor is null) ? ScottPlot.Cursor.Arrow : draggableUnderCursor.DragCursor;
            CursorChanged(null, EventArgs.Empty);
        }

        public void MouseUp(InputState input)
        {
            PlottableBeingDragged = null;
            bool mouseWasDragged = Settings.MouseHasMoved(input.X, input.Y);

            if (IsZoomingRectangle && mouseWasDragged && Configuration.MiddleClickDragZoom)
                eventProcessor.Process(eventFactory.CreateApplyZoomRectangleEvent(input.X, input.Y));
            else if (IsMiddleDown && Configuration.MiddleClickAutoAxis)
                eventProcessor.Process(eventFactory.CreateMouseAutoAxis());
            else
                eventProcessor.Process(eventFactory.CreateMouseUpClearRender());

            if (IsRightDown && mouseWasDragged == false)
                RightClicked(null, EventArgs.Empty);

            IsMiddleDown = false;
            IsRightDown = false;
            IsLeftDown = false;

            UpdateCursor(input);
        }


        public void DoubleClick()
        {
            if (Configuration.DoubleClickBenchmark == false)
                return;

            eventProcessor.Process(eventFactory.CreateBenchmarkToggle());
        }

        /// <summary>
        /// Apply a scroll wheel action, perform a low quality render, and later re-render in high quality.
        /// </summary>
        public void MouseWheel(InputState input)
        {
            if (!Settings.AllAxesHaveBeenSet)
                Plot.SetAxisLimits(Plot.GetAxisLimits());

            if (Configuration.ScrollWheelZoom)
                eventProcessor.Process(eventFactory.CreateMouseScroll(input.X, input.Y, input.WheelScrolledUp));

        }

        /// <summary>
        /// Apply a scroll wheel action, perform a low quality render, and later re-render in high quality.
        /// </summary>
        public async Task MouseWheelAsync(InputState input)
        {
        }
    }
}
