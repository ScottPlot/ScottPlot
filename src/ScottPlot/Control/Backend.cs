/* This file describes the ScottPlot back-end control module.
 * 
 *  Goals for this module:
 *    - Interact with the Plot object so controls don't have to.
 *    - Wrap/abstract mouse interaction logic so controls don't have to implement it.
 *    - Use events to tell controls when to update the image or change the mouse cursor.
 *    - Render calls are non-blocking so GUI/controls aren't slowed by render requests.
 *    - Delayed high quality renders are possible after mouse interaction stops.
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
 *    - quality (anti-aliasing on/off) for various actions
 *    - delayed high quality render after low-quality interactive renders
 *   
 */

using ScottPlot.Control.EventProcess;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    /// <summary>
    /// The control back end module contains all the logic required to manage a mouse-interactive
    /// plot to display in a user control. However, this module contains no control-specific dependencies.
    /// User controls can instantiate this object, pass mouse and resize event information in, and have
    /// renders triggered using events.
    /// </summary>
    public class ControlBackEnd
    {
        /// <summary>
        /// This event is invoked when an existing Bitmap is redrawn.
        /// e.g., after rendering following a click-drag-pan mouse event.
        /// </summary>
        public event EventHandler BitmapUpdated = delegate { };

        /// <summary>
        /// This event is invoked after a new Bitmap was created.
        /// e.g., after resizing the control, requiring a new Bitmap of a different size
        /// </summary>
        public event EventHandler BitmapChanged = delegate { };

        /// <summary>
        /// This event is invoked when the cursor is supposed to change.
        /// Cursor changes may be required when hovering over draggable plottable objects.
        /// </summary>
        public event EventHandler CursorChanged = delegate { };

        /// <summary>
        /// This event is invoked when the axis limts change.
        /// This is typically the result of a pan or zoom operation.
        /// </summary>
        public event EventHandler AxesChanged = delegate { };

        /// <summary>
        /// This event is invoked when the user right-clicks the control with the mouse.
        /// It is typically used to deploy a context menu.
        /// </summary>
        public event EventHandler RightClicked = delegate { };

        /// <summary>
        /// The control configuration object stores advanced customization and behavior settings
        /// for mouse-interactive plots.
        /// </summary>
        public readonly Configuration Configuration = new();

        /// <summary>
        /// True if the middle mouse button is pressed
        /// </summary>
        private bool IsMiddleDown;

        /// <summary>
        /// True if the right mouse button is pressed
        /// </summary>
        private bool IsRightDown;

        /// <summary>
        /// True if the left mouse button is pressed
        /// </summary>
        private bool IsLeftDown;

        /// <summary>
        /// Current position of the mouse in pixels
        /// </summary>
        private float MouseLocationX;

        /// <summary>
        /// Current position of the mouse in pixels
        /// </summary>
        private float MouseLocationY;

        /// <summary>
        /// Holds the plottable actively being dragged with the mouse.
        /// Contains null if no plottable is being dragged.
        /// </summary>
        private IDraggable PlottableBeingDragged = null;

        /// <summary>
        /// True when a zoom rectangle is being drawn and the mouse button is still down
        /// </summary>
        private bool IsZoomingRectangle;

        /// <summary>
        /// True if a zoom rectangle is being actively drawn using ALT + left click
        /// </summary>
        private bool IsZoomingWithAlt;

        /// <summary>
        /// The plot underlying this control.
        /// </summary>
        public Plot Plot { get; private set; }

        /// <summary>
        /// The settings object underlying the plot.
        /// </summary>
        private Settings Settings;

        /// <summary>
        /// The latest render is stored in this bitmap.
        /// New renders may be performed on this existing bitmap.
        /// When a new bitmap is created, this bitmap will be stored in OldBitmaps and eventually disposed.
        /// </summary>
        private System.Drawing.Bitmap Bmp;

        /// <summary>
        /// Bitmaps that are created are stored here so they can be kept track of and
        /// disposed properly when new bitmaps are created.
        /// </summary>
        private readonly Queue<System.Drawing.Bitmap> OldBitmaps = new();

        /// <summary>
        /// Store last render limits so new renders can know whether the axis limits
        /// have changed and decide whether to invoke the AxesChanged event or not.
        /// </summary>
        private AxisLimits LimitsOnLastRender = new();

        /// <summary>
        /// Store the total number of plottables so user controls can implement a timer
        /// to automatically update the plot if this number changes.
        /// </summary>
        private int PlottableCountOnLastRender = -1;

        /// <summary>
        /// This is set to True while the render loop is running.
        /// This prevents multiple renders from occurring at the same time.
        /// </summary>
        private bool currentlyRendering = false;

        /// <summary>
        /// The style of cursor the control should display
        /// </summary>
        public Cursor Cursor { get; private set; } = Cursor.Arrow;

        /// <summary>
        /// The events processor invokes renders in response to custom events
        /// </summary>
        private readonly EventsProcessor EventsProcessor;

        /// <summary>
        /// The event factor creates event objects to be handled by the event processor
        /// </summary>
        private UIEventFactory EventFactory;

        /// <summary>
        /// Number of times the current bitmap has been rendered on.
        /// </summary>
        private int BitmapRenderCount = 0;

        /// <summary>
        /// Tracks the total distance the mouse was click-dragged (rectangular pixel units)
        /// </summary>
        private float MouseDownTravelDistance;

        /// <summary>
        /// True if the mouse was dragged (with a button down) long enough to quality as a drag instead of a click
        /// </summary>
        private bool MouseDownDragged => MouseDownTravelDistance > Configuration.IgnoreMouseDragDistance;

        /// <summary>
        /// Create a back-end for a user control
        /// </summary>
        /// <param name="width">initial bitmap size (pixels)</param>
        /// <param name="height">initial bitmap size (pixels)</param>
        public ControlBackEnd(float width, float height)
        {
            EventFactory = new UIEventFactory(Configuration, Settings, Plot);
            Reset(width, height);

            // create an event processor and later request new renders by interacting with it.
            EventsProcessor = new EventsProcessor(
                    renderAction: (lowQuality) => Render(lowQuality),
                    renderDelay: (int)Configuration.ScrollWheelZoomHighQualityDelay);
        }

        /// <summary>
        /// The host control may instantiate the back-end and start sending it events
        /// before it has fully connected its event handlers. To prevent processing events before
        /// the host is control is ready, the processor will be stopped until is called by the host control.
        /// </summary>
        public void StartProcessingEvents() => EventsProcessor.Enable = true;

        /// <summary>
        /// Reset the back-end by creating an entirely new plot of the given dimensions
        /// </summary>
        public void Reset(float width, float height) => Reset(width, height, new Plot());

        /// <summary>
        /// Reset the back-end by replacing the existing plot with one that has already been created
        /// </summary>
        public void Reset(float width, float height, Plot newPlot)
        {
            Plot = newPlot;
            Settings = Plot.GetSettings(false);
            EventFactory = new UIEventFactory(Configuration, Settings, Plot);
            Resize(width, height);
        }

        /// <summary>
        /// Return a copy of the list of draggable plottables
        /// </summary>
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

        /// <summary>
        /// Return a multi-line string describing the default mouse interactions.
        /// This can be useful for displaying a help message in a user control.
        /// </summary>
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

        /// <summary>
        /// Return the most recently rendered Bitmap.
        /// This method also disposes old Bitmaps if they exist.
        /// </summary>
        public System.Drawing.Bitmap GetLatestBitmap()
        {
            while (OldBitmaps.Count > 3)
                OldBitmaps.Dequeue()?.Dispose();
            return Bmp;
        }

        /// <summary>
        /// Render onto the existing Bitmap.
        /// Quality describes whether anti-aliasing will be used.
        /// </summary>
        public void Render(bool lowQuality = false, bool skipIfCurrentlyRendering = false)
        {
            if (Bmp is null)
                return;

            if (currentlyRendering && skipIfCurrentlyRendering)
                return;
            currentlyRendering = true;

            if (Configuration.Quality == QualityMode.High)
                lowQuality = false;
            else if (Configuration.Quality == QualityMode.Low)
                lowQuality = true;

            Plot.Render(Bmp, lowQuality);
            BitmapRenderCount += 1;
            PlottableCountOnLastRender = Settings.Plottables.Count;

            AxisLimits newLimits = Plot.GetAxisLimits();
            if (!newLimits.Equals(LimitsOnLastRender) && Configuration.AxesChangedEventEnabled)
                AxesChanged(null, EventArgs.Empty);
            LimitsOnLastRender = newLimits;

            if (BitmapRenderCount == 1)
            {
                // a new bitmap was rendered on for the first time
                BitmapChanged(this, EventArgs.Empty);
            }
            else
            {
                // an existing bitmap was re-rendered onto
                BitmapUpdated(null, EventArgs.Empty);
            }

            currentlyRendering = false;
        }

        public void RenderLowQuality()
        {
            ProcessEvent(EventFactory.CreateManualLowQualityRender());
        }

        public void RenderHighQuality()
        {
            ProcessEvent(EventFactory.CreateManualHighQualityRender());
        }

        /// <summary>
        /// Render the plot using low quality (fast) then immediate re-render using high quality (slower).
        /// </summary>
        public void RenderLowThenImmediateHighQuality()
        {
            ProcessEvent(EventFactory.CreateManualLowQualityRender());
            ProcessEvent(EventFactory.CreateManualHighQualityRender());
        }

        /// <summary>
        /// Render the plot using low quality (fast) then delayed re-render using high quality (slower).
        /// </summary>
        public void RenderDelayedHighQuality()
        {
            ProcessEvent(EventFactory.CreateManualDelayedHighQualityRender());
        }

        /// <summary>
        /// Check if the number of plottibles has changed and if so request a render.
        /// This is typically called by a continuously running timer in the user control.
        /// </summary>
        public void RenderIfPlottableCountChanged()
        {
            if (Bmp is null)
                return;
            if (Settings.Plottables.Count != PlottableCountOnLastRender)
                Render();
        }

        /// <summary>
        /// Resize the control (creates a new Bitmap and requests a render)
        /// </summary>
        public void Resize(float width, float height)
        {
            // don't render if the requested size cannot produce a valid bitmap
            if (width < 1 || height < 1)
                return;

            // don't render if the request is so early that the processor hasn't started
            if (EventsProcessor is null)
                return;

            // Disposing a Bitmap the GUI is displaying will cause an exception.
            // Keep track of old bitmaps so they can be disposed of later.
            OldBitmaps.Enqueue(Bmp);
            Bmp = new System.Drawing.Bitmap((int)width, (int)height);
            BitmapRenderCount = 0;

            RenderDelayedHighQuality();
        }

        /// <summary>
        /// Indicate a mouse button has just been pressed
        /// </summary>
        public void MouseDown(InputState input)
        {
            if (!Settings.AllAxesHaveBeenSet)
                Plot.SetAxisLimits(Plot.GetAxisLimits());
            IsMiddleDown = input.MiddleWasJustPressed;
            IsRightDown = input.RightWasJustPressed;
            IsLeftDown = input.LeftWasJustPressed;
            PlottableBeingDragged = GetDraggableUnderMouse(input.X, input.Y);
            Settings.MouseDown(input.X, input.Y);
            MouseDownTravelDistance = 0;
        }

        /// <summary>
        /// Return the mouse position on the plot (in coordinate space) for the latest X and Y coordinates
        /// </summary>
        public (double x, double y) GetMouseCoordinates()
        {
            (double x, double y) = Plot.GetCoordinate(MouseLocationX, MouseLocationY);
            return (double.IsNaN(x) ? 0 : x, double.IsNaN(y) ? 0 : y);
        }

        /// <summary>
        /// Return the mouse position (in pixel space) for the last observed mouse position
        /// </summary>
        public (float x, float y) GetMousePixel() => (MouseLocationX, MouseLocationY);

        /// <summary>
        /// Indicate the mouse has moved to a new position
        /// </summary>
        public void MouseMove(InputState input)
        {
            bool altWasLifted = IsZoomingWithAlt && !input.AltDown;
            if (IsZoomingRectangle && altWasLifted)
                Settings.ZoomRectangle.Clear();

            IsZoomingWithAlt = IsLeftDown && input.AltDown;
            bool isMiddleClickDragZooming = IsMiddleDown;
            bool isZooming = IsZoomingWithAlt || isMiddleClickDragZooming;
            IsZoomingRectangle = isZooming && Configuration.MiddleClickDragZoom && MouseDownDragged;

            MouseDownTravelDistance += Math.Abs(input.X - MouseLocationX);
            MouseDownTravelDistance += Math.Abs(input.Y - MouseLocationY);

            MouseLocationX = input.X;
            MouseLocationY = input.Y;

            IUIEvent mouseMoveEvent = null;
            if (PlottableBeingDragged != null)
                mouseMoveEvent = EventFactory.CreatePlottableDrag(input.X, input.Y, input.ShiftDown, PlottableBeingDragged);
            else if (IsLeftDown && !input.AltDown && Configuration.LeftClickDragPan)
                mouseMoveEvent = EventFactory.CreateMousePan(input);
            else if (IsRightDown && Configuration.RightClickDragZoom)
                mouseMoveEvent = EventFactory.CreateMouseZoom(input);
            else if (IsZoomingRectangle)
                mouseMoveEvent = EventFactory.CreateMouseMovedToZoomRectangle(input.X, input.Y);

            if (mouseMoveEvent != null)
                ProcessEvent(mouseMoveEvent);
            else
                MouseMovedWithoutInteraction(input);
        }

        /// <summary>
        /// Process an event using the render queue (non-blocking) or traditional rendering (blocking)
        /// based on the UseRenderQueue flag in the Configuration module.
        /// </summary>
        private void ProcessEvent(IUIEvent uiEvent)
        {
            if (Configuration.UseRenderQueue)
            {
                // TODO: refactor to better support async
                _ = EventsProcessor.ProcessAsync(uiEvent);
            }
            else
            {
                uiEvent.ProcessEvent();

                if (uiEvent.RenderType == RenderType.None)
                    return;

                bool lowQuality =
                    uiEvent is EventProcess.Events.MouseMovedToZoomRectangle ||
                    uiEvent is EventProcess.Events.MousePanEvent ||
                    uiEvent is EventProcess.Events.MouseZoomEvent ||
                    uiEvent is EventProcess.Events.PlottableDragEvent ||
                    uiEvent is EventProcess.Events.RenderLowQuality;

                bool allowSkip = lowQuality && Configuration.AllowDroppedFramesWhileDragging;

                Render(lowQuality: lowQuality, skipIfCurrentlyRendering: allowSkip);
            }
        }

        /// <summary>
        /// Call this when the mouse moves without any buttons being down.
        /// It will only update the cursor based on what's beneath the cursor.
        /// </summary>
        private void MouseMovedWithoutInteraction(InputState input)
        {
            UpdateCursor(input);
        }

        /// <summary>
        /// Set the cursor based on whether a draggable is engaged or not,
        /// then invoke the CursorChanged event.
        /// </summary>
        private void UpdateCursor(InputState input)
        {
            var draggableUnderCursor = GetDraggableUnderMouse(input.X, input.Y);
            Cursor = (draggableUnderCursor is null) ? Cursor.Arrow : draggableUnderCursor.DragCursor;
            CursorChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Indicate a mouse button has been released.
        /// This may initiate a render (and/or a delayed render).
        /// </summary>
        /// <param name="input"></param>
        public void MouseUp(InputState input)
        {
            PlottableBeingDragged = null;

            IUIEvent mouseEvent;
            if (IsZoomingRectangle && MouseDownDragged && Configuration.MiddleClickDragZoom)
                mouseEvent = EventFactory.CreateApplyZoomRectangleEvent(input.X, input.Y);
            else if (IsMiddleDown && Configuration.MiddleClickAutoAxis && MouseDownDragged == false)
                mouseEvent = EventFactory.CreateMouseAutoAxis();
            else
                mouseEvent = EventFactory.CreateMouseUpClearRender();
            ProcessEvent(mouseEvent);

            if (IsRightDown && MouseDownDragged == false)
                RightClicked(null, EventArgs.Empty);

            IsMiddleDown = false;
            IsRightDown = false;
            IsLeftDown = false;

            UpdateCursor(input);
        }

        /// <summary>
        /// Indicate the left mouse button has been double-clicked.
        /// The default action of a double-click is to toggle the benchmark.
        /// </summary>
        public void DoubleClick()
        {
            if (Configuration.DoubleClickBenchmark)
            {
                IUIEvent mouseEvent = EventFactory.CreateBenchmarkToggle();
                ProcessEvent(mouseEvent);
            }
        }

        /// <summary>
        /// Apply a scroll wheel action, perform a low quality render, and later re-render in high quality.
        /// </summary>
        public void MouseWheel(InputState input)
        {
            if (!Settings.AllAxesHaveBeenSet)
                Plot.SetAxisLimits(Plot.GetAxisLimits());

            if (Configuration.ScrollWheelZoom)
            {
                IUIEvent mouseEvent = EventFactory.CreateMouseScroll(input.X, input.Y, input.WheelScrolledUp);
                ProcessEvent(mouseEvent);
            }
        }
    }
}
