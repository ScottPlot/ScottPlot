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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

        public ControlBackEnd(float width, float height) =>
            Reset(width, height);

        public void Reset(float width, float height) =>
            Reset(width, height, new Plot());

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

        private void RenderAfterDragging() =>
            Render(
                lowQuality: Configuration.Quality == QualityMode.LowWhileDragging,
                skipIfCurrentlyRendering: Configuration.AllowDroppedFramesWhileDragging);

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
            IsMiddleDown = input.MiddleWasJustPressed;
            IsRightDown = input.RightWasJustPressed;
            IsLeftDown = input.LeftWasJustPressed;
            PlottableBeingDragged = Plot.GetDraggableUnderMouse(input.X, input.Y);
            Settings.MouseDown(input.X, input.Y);
        }

        private float MouseLocationX;
        private float MouseLocationY;

        public (double x, double y) GetMouseCoordinates() => Plot.GetCoordinate(MouseLocationX, MouseLocationY);

        public (float x, float y) GetMousePixel() => (MouseLocationX, MouseLocationY);

        private bool IsZoomingRectangle;
        public void MouseMove(InputState input)
        {
            IsZoomingRectangle = IsMiddleDown || (IsLeftDown && input.AltDown);
            MouseLocationX = input.X;
            MouseLocationY = input.Y;
            if (PlottableBeingDragged != null)
                MouseMovedToDragPlottable(input);
            else if (IsLeftDown && !input.AltDown)
                MouseMovedToPan(input);
            else if (IsRightDown)
                MouseMovedToZoom(input);
            else if (IsZoomingRectangle)
                MouseMovedToZoomRectangle(input);
            else
                MouseMovedWithoutInteraction(input);
        }

        private void MouseMovedToDragPlottable(InputState input)
        {
            double x = Plot.GetCoordinateX(input.X);
            double y = Plot.GetCoordinateY(input.Y);
            PlottableBeingDragged.DragTo(x, y, fixedSize: input.ShiftDown);
            RenderAfterDragging();
        }

        private void MouseMovedToPan(InputState input)
        {
            if (Configuration.LeftClickDragPan == false)
                return;

            float x = (input.ShiftDown || Configuration.LockHorizontalAxis) ? Settings.MouseDownX : input.X;
            float y = (input.CtrlDown || Configuration.LockVerticalAxis) ? Settings.MouseDownY : input.Y;
            Settings.MousePan(x, y);

            RenderAfterDragging();
        }

        private void MouseMovedToZoom(InputState input)
        {
            if (Configuration.RightClickDragZoom == false)
                return;

            var originalLimits = Plot.GetAxisLimits();

            if (input.ShiftDown && input.CtrlDown)
            {
                float dx = input.X - Settings.MouseDownX;
                float dy = Settings.MouseDownY - input.Y;
                float delta = Math.Max(dx, dy);
                Settings.MouseZoom(Settings.MouseDownX + delta, Settings.MouseDownY - delta);
            }
            else
            {
                float x = input.ShiftDown ? Settings.MouseDownX : input.X;
                float y = input.CtrlDown ? Settings.MouseDownY : input.Y;
                Settings.MouseZoom(x, y);
            }

            if (Configuration.LockHorizontalAxis)
                Plot.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (Configuration.LockVerticalAxis)
                Plot.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);

            RenderAfterDragging();
        }

        private void MouseMovedToZoomRectangle(InputState input)
        {
            Settings.MouseZoomRect(input.X, input.Y);
            RenderAfterDragging();
        }

        private void MouseMovedWithoutInteraction(InputState input)
        {
            UpdateCursor(input);
        }

        private void UpdateCursor(InputState input)
        {
            var draggableUnderCursor = Plot.GetDraggableUnderMouse(input.X, input.Y);
            Cursor = (draggableUnderCursor is null) ? ScottPlot.Cursor.Arrow : draggableUnderCursor.DragCursor;
            CursorChanged(null, EventArgs.Empty);
        }

        public void MouseUp(InputState input)
        {
            PlottableBeingDragged = null;
            bool mouseWasDragged = Settings.MouseHasMoved(input.X, input.Y);

            if (IsZoomingRectangle && mouseWasDragged)
                ApplyZoomRectangle(input);
            else if (IsMiddleDown)
                MiddleClickAutoAxis();

            if (IsRightDown && mouseWasDragged == false)
                RightClicked(null, EventArgs.Empty);

            IsMiddleDown = false;
            IsRightDown = false;
            IsLeftDown = false;

            Render();
            UpdateCursor(input);
        }

        private void ApplyZoomRectangle(InputState input)
        {
            if (Configuration.MiddleClickDragZoom == false)
                return;

            Settings.RecallAxisLimits();

            var originalLimits = Plot.GetAxisLimits();

            Settings.MouseZoomRect(input.X, input.Y, finalize: true);

            if (Configuration.LockHorizontalAxis)
                Plot.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (Configuration.LockVerticalAxis)
                Plot.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);
        }

        private void MiddleClickAutoAxis()
        {
            if (Configuration.MiddleClickAutoAxis == false)
                return;

            Settings.ZoomRectangle.Clear();

            if (Configuration.LockVerticalAxis == false)
                Plot.AxisAutoY(Configuration.MiddleClickAutoAxisMarginY);

            if (Configuration.LockHorizontalAxis == false)
                Plot.AxisAutoX(Configuration.MiddleClickAutoAxisMarginX);

            Render();
        }

        public void DoubleClick()
        {
            if (Configuration.DoubleClickBenchmark == false)
                return;

            Plot.Benchmark(!Plot.Benchmark(null));
            Render();
        }

        /// <summary>
        /// Apply a scroll wheel action, perform a low quality render, and later re-render in high quality.
        /// </summary>
        public void MouseWheel(InputState input)
        {
            // this method is suitable for WinForms
            MouseWheelQueue.Enqueue(input);
            _ = MouseWheelQueueProcessorAsync(sendUpdateEvents: true);
        }

        /// <summary>
        /// Apply a scroll wheel action, perform a low quality render, and later re-render in high quality.
        /// </summary>
        public async Task MouseWheelAsync(InputState input)
        {
            // this method is suitable for WPF
            MouseWheelQueue.Enqueue(input);
            await MouseWheelQueueProcessorAsync(sendUpdateEvents: false);
        }

        /// <summary>
        /// Perform a low quality render after a mouse wheel.
        /// The MouseWheelQueueProcessor will later repeat a render using high quality.
        /// </summary>
        private void MouseWheelRender(InputState input)
        {
            if (Configuration.ScrollWheelZoom == false)
                return;

            double xFrac = input.WheelScrolledUp ? 1.15 : 0.85;
            double yFrac = input.WheelScrolledUp ? 1.15 : 0.85;

            if (Configuration.LockHorizontalAxis)
                xFrac = 1;
            if (Configuration.LockVerticalAxis)
                yFrac = 1;

            Settings.AxesZoomTo(xFrac, yFrac, input.X, input.Y);
            Render(lowQuality: true);
        }

        /// <summary>
        /// Watches the queue for mouse events and processes them as they come in using low quality rendering.
        /// After a certain amount of time without new events, perform a high quality render and exit.
        /// </summary>
        private async Task MouseWheelQueueProcessorAsync(bool sendUpdateEvents)
        {
            if (MouseWheelStopwatch.IsRunning)
                return;

            MouseWheelStopwatch.Start();
            while (true)
            {
                while (MouseWheelQueue.Count > 0)
                {
                    Debug.WriteLine($"{MouseWheelStopwatch.ElapsedMilliseconds} rendering LW");
                    MouseWheelRender(MouseWheelQueue.Dequeue());
                    MouseWheelStopwatch.Restart();
                }

                if (sendUpdateEvents)
                    BitmapUpdated.Invoke(null, EventArgs.Empty);

                await Task.Delay(10);

                if (MouseWheelStopwatch.ElapsedMilliseconds >= Configuration.ScrollWheelZoomHighQualityDelay)
                {
                    Debug.WriteLine($"{MouseWheelStopwatch.ElapsedMilliseconds} rendering HQ");
                    Render(lowQuality: false);
                    MouseWheelStopwatch.Reset();
                    return;
                }
            }
        }
    }
}
