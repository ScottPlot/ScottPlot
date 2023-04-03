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
        /// This event is invoked when the user left-clicks the control with the mouse.
        /// It is typically used to interact with custom plot types.
        /// </summary>
        public event EventHandler LeftClicked = delegate { };

        /// <summary>
        /// This event is invoked when the user left-clicks a plottable control with the mouse.
        /// </summary>
        public event EventHandler LeftClickedPlottable = delegate { };

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// </summary>
        public event EventHandler PlottableDragged = delegate { };

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// </summary>
        public event EventHandler PlottableDropped = delegate { };

        /// <summary>
        /// The control configuration object stores advanced customization and behavior settings
        /// for mouse-interactive plots.
        /// </summary>
        public readonly Configuration Configuration;

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
        /// Unique identifier of the plottables list that was last rendered.
        /// This value is used to determine if the plottables list was modified (requiring a re-render).
        /// </summary>
        private int PlottablesIdentifierAtLastRender = 0;

        /// <summary>
        /// This is set to True while the render loop is running.
        /// This prevents multiple renders from occurring at the same time.
        /// </summary>
        private bool currentlyRendering = false;

        /// <summary>
        /// The style of cursor the control should display
        /// </summary>
        public Cursor Cursor { get; private set; }

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
        /// Total number of renders performed.
        /// Note that at least one render occurs before the first request to measure the layout and calculate data area.
        /// This means the first render increments this number twice.
        /// </summary>
        public int RenderCount { get; private set; } = 0;

        /// <summary>
        /// Tracks the total distance the mouse was click-dragged (rectangular pixel units)
        /// </summary>
        private float MouseDownTravelDistance;

        /// <summary>
        /// True if the mouse was dragged (with a button down) long enough to quality as a drag instead of a click
        /// </summary>
        public bool MouseDownDragged => MouseDownTravelDistance > Configuration.IgnoreMouseDragDistance;


        /// <summary>
        /// Indicates whether Render() has been explicitly called by the user.
        /// Renders requested by resize events do not count.
        /// </summary>
        public bool WasManuallyRendered;

        /// <summary>
        /// Variable name for the user control tied to this backend.
        /// </summary>
        public readonly string ControlName;

        /// <summary>
        /// Plots whose axes and layout will be updated when this plot changes
        /// </summary>
        readonly List<LinkedPlotControl> LinkedPlotControls = new();

        /// <summary>
        /// Create a back-end for a user control
        /// </summary>
        /// <param name="width">initial bitmap size (pixels)</param>
        /// <param name="height">initial bitmap size (pixels)</param>
        /// <param name="name">variable name of the user control using this backend</param>
        public ControlBackEnd(float width, float height, string name = "UnamedControl")
        {
            Configuration = new(this);
            Cursor = Configuration.DefaultCursor;
            EventFactory = new UIEventFactory(Configuration, Settings, Plot);
            EventsProcessor = new EventsProcessor(
                    renderAction: (lowQuality) => Render(lowQuality),
                    renderDelay: (int)Configuration.ScrollWheelZoomHighQualityDelay);
            ControlName = name;
            Reset(width, height);
        }

        /// <summary>
        /// The host control may instantiate the back-end and start sending it events
        /// before it has fully connected its event handlers. To prevent processing events before
        /// the host is control is ready, the processor will be stopped until is called by the host control.
        /// </summary>
        public void StartProcessingEvents()
        {
            EventsProcessor.Enable = true;
        }

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
            WasManuallyRendered = false;
            Resize(width, height, useDelayedRendering: false);
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
            RenderCount += 1;
            PlottablesIdentifierAtLastRender = Settings.PlottablesIdentifier;

            if (WasManuallyRendered == false &&
                Settings.Plottables.Count > 0 &&
                Configuration.WarnIfRenderNotCalledManually &&
                Debugger.IsAttached)
            {
                string message = $"ScottPlot {Plot.Version} WARNING:\n" +
                    $"{ControlName}.Refresh() must be called\n" +
                    $"after modifying the plot or its data.";
                Debug.WriteLine(message.Replace("\n", " "));
                AddErrorMessage(Bmp, message);
            }

            AxisLimits newLimits = Plot.GetAxisLimits();
            if (!newLimits.Equals(LimitsOnLastRender) && Configuration.AxesChangedEventEnabled)
            {
                AxesChanged(null, EventArgs.Empty);
                UpdateLinkedPlotControls();
            }
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

        /// <summary>
        /// Add error text on top of the rendered plot
        /// </summary>
        private static void AddErrorMessage(System.Drawing.Bitmap bmp, string message)
        {
            System.Drawing.Color foreColor = System.Drawing.Color.Red;
            System.Drawing.Color backColor = System.Drawing.Color.Yellow;
            System.Drawing.Color shadowColor = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Black);
            int padding = 10;
            int shadowOffset = 7;

            using System.Drawing.Graphics gfx = Drawing.GDI.Graphics(bmp);
            using System.Drawing.StringFormat sf = Drawing.GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Middle);
            using System.Drawing.Font font = new(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold);
            System.Drawing.SizeF messageSize = gfx.MeasureString(message, font);
            System.Drawing.RectangleF messageRect = new(
                x: bmp.Width / 2 - messageSize.Width / 2 - padding,
                y: bmp.Height / 2 - messageSize.Height / 2 - padding,
                width: messageSize.Width + padding * 2,
                height: messageSize.Height + padding * 2);
            System.Drawing.RectangleF shadowRect = new(
                x: messageRect.X + shadowOffset,
                y: messageRect.Y + shadowOffset,
                width: messageRect.Width,
                height: messageRect.Height);

            using System.Drawing.SolidBrush foreBrush = new(foreColor);
            using System.Drawing.Pen forePen = new(foreColor, width: 5);
            using System.Drawing.SolidBrush backBrush = new(backColor);
            using System.Drawing.SolidBrush shadowBrush = new(shadowColor);

            if (messageSize.Width > bmp.Width || messageSize.Height > bmp.Height)
            {
                System.Drawing.RectangleF plotRect = new(0, 0, bmp.Width, bmp.Height);
                sf.Alignment = System.Drawing.StringAlignment.Near;
                sf.LineAlignment = System.Drawing.StringAlignment.Near;
                message = message.Replace("\n", " ");

                using System.Drawing.Font fontSmall = new(System.Drawing.FontFamily.GenericSansSerif, 12, System.Drawing.FontStyle.Bold);
                gfx.Clear(backColor);
                gfx.DrawString(message, fontSmall, foreBrush, plotRect, sf);
            }
            else
            {
                gfx.FillRectangle(shadowBrush, shadowRect);
                gfx.FillRectangle(backBrush, messageRect);
                gfx.DrawRectangle(forePen, System.Drawing.Rectangle.Round(messageRect));
                gfx.DrawString(message, font, foreBrush, messageRect, sf);
            }
        }

        /// <summary>
        /// Request a render using the render queue. 
        /// This method does not block the calling thread.
        /// </summary>
        public void RenderRequest(RenderType renderType)
        {
            switch (renderType)
            {
                case RenderType.LowQuality:
                    ProcessEvent(EventFactory.CreateManualLowQualityRender());
                    return;

                case RenderType.HighQuality:
                    ProcessEvent(EventFactory.CreateManualHighQualityRender());
                    return;

                case RenderType.HighQualityDelayed:
                    ProcessEvent(EventFactory.CreateManualDelayedHighQualityRender());
                    return;

                case RenderType.LowQualityThenHighQuality:
                    ProcessEvent(EventFactory.CreateManualLowQualityRender());
                    ProcessEvent(EventFactory.CreateManualHighQualityRender());
                    return;

                case RenderType.LowQualityThenHighQualityDelayed:
                    ProcessEvent(EventFactory.CreateManualDelayedHighQualityRender());
                    return;

                case RenderType.ProcessMouseEventsOnly:
                    return;

                default:
                    throw new InvalidOperationException($"unsupported render type {renderType}");
            }
        }

        /// <summary>
        /// Check if the number of plottibles has changed and if so request a render.
        /// This is typically called by a continuously running timer in the user control.
        /// </summary>
        [Obsolete("Automatic render timer has been removed. Call Render() manually.", true)]
        public void RenderIfPlottableListChanged()
        {
            if (Configuration.RenderIfPlottableListChanges == false)
                return;

            if (Bmp is null)
                return;

            if (Settings.PlottablesIdentifier != PlottablesIdentifierAtLastRender)
                Render();
        }

        /// <summary>
        /// Resize the control (creates a new Bitmap and requests a render)
        /// </summary>
        /// <param name="width">new width (pixels)</param>
        /// <param name="height">new height (pixels)</param>
        /// <param name="useDelayedRendering">Render using the queue (best for mouse events), otherwise render immediately.</param>
        public void Resize(float width, float height, bool useDelayedRendering)
        {
            // don't render if the requested size cannot produce a valid bitmap
            if (width < 1 || height < 1)
                return;

            // don't render if the request is so early that the processor hasn't started
            if (EventsProcessor is null)
                return;

            // avoid update loops
            if (Bmp?.Width == (int)width && Bmp?.Height == (int)height)
                return;

            // Disposing a Bitmap the GUI is displaying will cause an exception.
            // Keep track of old bitmaps so they can be disposed of later.
            OldBitmaps.Enqueue(Bmp);
            Bmp = new System.Drawing.Bitmap((int)width, (int)height);
            BitmapRenderCount = 0;

            if (useDelayedRendering)
                RenderRequest(RenderType.HighQualityDelayed);
            else
                Render();
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
            PlottableBeingDragged = IsLeftDown ? Plot.GetDraggable(input.X, input.Y) : null;
            Settings.MouseDown(input.X, input.Y);
            MouseDownTravelDistance = 0;
        }

        /// <summary>
        /// Return the mouse position on the plot (in coordinate space) for the latest X and Y coordinates
        /// </summary>
        public (double x, double y) GetMouseCoordinates(int xAxisIndex = 0, int yAxisIndex = 0)
        {
            (double x, double y) = Plot.GetCoordinate(MouseLocationX, MouseLocationY, xAxisIndex, yAxisIndex);
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
            bool isZoomingRectangleWithAltLeft = IsLeftDown && input.AltDown && Configuration.AltLeftClickDragZoom;
            bool isZoomingRectangleWithMiddle = IsMiddleDown && Configuration.MiddleClickDragZoom;

            bool wasZoomingRectangle = IsZoomingRectangle;
            IsZoomingRectangle = isZoomingRectangleWithAltLeft || isZoomingRectangleWithMiddle;

            if (wasZoomingRectangle && !IsZoomingRectangle)
                Settings.ZoomRectangle.Clear();

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

            if ((IsRightDown || IsMiddleDown) && (Cursor != Configuration.DefaultCursor))
            {
                // cursor was originally over a draggable but is now zooming
                Cursor = Configuration.DefaultCursor;
                CursorChanged?.Invoke(this, EventArgs.Empty);
            }

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
                // TODO: document that draggable events aren't supported by the render queue
                _ = EventsProcessor.ProcessAsync(uiEvent);
            }
            else
            {
                //Console.WriteLine($"[{DateTime.Now:ss.ffff}] PROCESSING: {uiEvent}");
                uiEvent.ProcessEvent();

                if (uiEvent.RenderType == RenderType.ProcessMouseEventsOnly)
                    return;

                bool lowQuality =
                    uiEvent is EventProcess.Events.MouseMovedToZoomRectangle ||
                    uiEvent is EventProcess.Events.MousePanEvent ||
                    uiEvent is EventProcess.Events.MouseZoomEvent ||
                    uiEvent is EventProcess.Events.PlottableDragEvent ||
                    uiEvent is EventProcess.Events.RenderLowQuality;

                bool allowSkip = lowQuality && Configuration.AllowDroppedFramesWhileDragging;

                Render(lowQuality: lowQuality, skipIfCurrentlyRendering: allowSkip);

                if (uiEvent is EventProcess.Events.PlottableDragEvent)
                    PlottableDragged(PlottableBeingDragged, EventArgs.Empty);
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
            var newCursor = Configuration.DefaultCursor;

            var draggableUnderCursor = Plot.GetDraggable(input.X, input.Y);
            if (draggableUnderCursor is not null)
            {
                newCursor = draggableUnderCursor.DragCursor;
            }

            var hittable = Plot.GetHittable(input.X, input.Y);
            if (hittable is not null)
            {
                newCursor = ((IHittable)hittable).HitCursor;
            }

            if (Cursor != newCursor)
            {
                Cursor = newCursor;
                CursorChanged(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicate a mouse button has been released.
        /// This may initiate a render (and/or a delayed render).
        /// </summary>
        /// <param name="input"></param>
        public void MouseUp(InputState input)
        {
            var droppedPlottable = PlottableBeingDragged;

            IUIEvent mouseEvent;
            if (IsZoomingRectangle && MouseDownDragged)
                mouseEvent = EventFactory.CreateApplyZoomRectangleEvent(input.X, input.Y);
            else if (IsMiddleDown && Configuration.MiddleClickAutoAxis && MouseDownDragged == false)
                mouseEvent = EventFactory.CreateMouseAutoAxis();
            else
                mouseEvent = EventFactory.CreateMouseUpClearRender();
            ProcessEvent(mouseEvent);

            if (IsRightDown && MouseDownDragged == false)
                RightClicked(null, EventArgs.Empty);

            if (IsLeftDown && MouseDownDragged == false)
            {
                LeftClicked(null, EventArgs.Empty);

                IPlottable plottableHit = Plot.GetHittable(input.X, input.Y);
                if (plottableHit is not null)
                {
                    LeftClickedPlottable(plottableHit, EventArgs.Empty);
                }
            }

            IsMiddleDown = false;
            IsRightDown = false;
            IsLeftDown = false;

            UpdateCursor(input);

            if (droppedPlottable != null)
                PlottableDropped(droppedPlottable, EventArgs.Empty);

            PlottableBeingDragged = null;
            if (droppedPlottable != null)
                ProcessEvent(EventFactory.CreateMouseUpClearRender());
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
            if (input.WheelScrolledDown == false && input.WheelScrolledUp == false)
                throw new ArgumentException("mouse wheel was scrolled but neither direction is set");

            if (!Settings.AllAxesHaveBeenSet)
                Plot.SetAxisLimits(Plot.GetAxisLimits());

            if (Configuration.ScrollWheelZoom)
            {
                IUIEvent mouseEvent = EventFactory.CreateMouseScroll(input.X, input.Y, input.WheelScrolledUp);
                ProcessEvent(mouseEvent);
            }
        }

        /// <summary>
        /// Add a plot which will have its axes and layout updated when this plot changes
        /// </summary>
        public void AddLinkedControl(IPlotControl otherPlot, bool horizontal = true, bool vertical = true, bool layout = true)
        {
            LinkedPlotControl linkedControl = new(otherPlot, horizontal, vertical, layout);
            LinkedPlotControls.Add(linkedControl);
            UpdateLinkedPlotControls();
        }

        public void ClearLinkedControls()
        {
            LinkedPlotControls.Clear();
        }

        private void UpdateLinkedPlotControls()
        {
            if (!Configuration.EmitLinkedControlUpdateSignals)
                return;

            foreach (LinkedPlotControl linkedPlotControl in LinkedPlotControls)
            {
                linkedPlotControl.PlotControl.Configuration.EmitLinkedControlUpdateSignals = false;

                linkedPlotControl.PlotControl.Plot.MatchAxis(
                    sourcePlot: this.Plot,
                    horizontal: linkedPlotControl.LinkHorizontal,
                    vertical: linkedPlotControl.LinkVertical);

                if (linkedPlotControl.LinkLayout)
                {
                    linkedPlotControl.PlotControl.Plot.MatchLayout(
                        sourcePlot: this.Plot,
                        horizontal: linkedPlotControl.LinkHorizontal,
                        vertical: linkedPlotControl.LinkVertical);
                }

                linkedPlotControl.PlotControl.Refresh();

                linkedPlotControl.PlotControl.Configuration.EmitLinkedControlUpdateSignals = true;
            }
        }
    }
}
