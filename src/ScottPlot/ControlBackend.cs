using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Interactive
{
    /// <summary>
    ///     A struct for representing an item in a context menu (right click menu)
    /// </summary>
    public struct ContextMenuItem
    {
        /// <summary>
        ///     The name of the item.
        /// </summary>
        public string itemName;
        /// <summary>
        ///     The <c>Action</c> to be ran when clicked. Note that the <c>Action</c> tyoe takes no parameters and has no return type.
        ///     
        ///     <example>
        ///         The <c>Action</c> type need not be instantiated directly, it is compatible with the delegate syntax:
        ///         
        ///         <code>
        ///             onClick = () => Debug.WriteLine("Clicked!");
        ///         </code>
        ///     </example>
        /// </summary>
        public Action onClick;
    }

    /// <summary>
    ///     Represents the buttons on a mouse, for the purpose of mouse/pointer events.
    /// </summary>
    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    /// <summary>
    ///     A class which provides a backend for creating user controls without rewriting large amounts of code.
    /// </summary>
    public abstract class ControlBackend
    {
        /// <summary>
        ///     The backing <c><see cref="ScottPlot.Plot"/></c> object
        /// </summary>
        public Plot plt { get; protected set; }

        /// <summary>
        ///     The settings object of <c>this.pltt</c>
        /// </summary>
        protected Settings settings;

        /// <summary>
        ///     For disabling previews in designer mode. In the event that this is difficult to detect this variable can be ignored.
        /// </summary>
        protected bool isDesignerMode;

        /// <summary>
        ///     For DPI scaling. On DPI unaware platforms this should be left at 1. For DPI aware platforms this can be set like so:
        ///     
        ///     <example>
        ///         <code>
        ///             dpiScale = settings.gfxFigure.DpiX / 96;
        ///         </code>
        ///     </example>
        /// </summary>
        public double dpiScale { get; protected set; } = 1;

        /// <summary>
        ///     Returns true if the plot is holding a <c><see cref="ScottPlot.PlottableHeatmap"/></c> object (whether visible or not).
        /// </summary>
        private bool plotContainsHeatmap => settings?.plottables.Where(p => p is PlottableHeatmap).Count() > 0;

        /// <summary>
        ///     A list of <c><see cref="ScottPlot.Interactive.ContextMenuItem"/></c> objects which hold the items in the context menu of this user control.
        /// </summary>
        public List<ContextMenuItem> contextMenuItems;

        /// <summary>
        ///     A method which should be overridden to provide the default context menu.
        /// </summary>
        /// <returns>
        ///     <c>List&lt;<see cref="ScottPlot.Interactive.ContextMenuItem"/>&gt;</c> A user defined default context menu
        /// </returns>
        public virtual List<ContextMenuItem> DefaultRightClickMenu()
        {
            return new List<ContextMenuItem>();
        }

        /// <summary>
        ///     Calls <see cref="ControlBackend.Reset(Plot)"/> with <c>null</c> as the only argument
        /// </summary>
        public void Reset()
        {
            Reset(null);
        }

        /// <summary>
        ///     Resets the plot
        /// </summary>
        /// <param name="plt">The plot to reset</param>
        public void Reset(Plot plt)
        {
            this.plt = (plt is null) ? new Plot() : plt;
            InitializeScottPlot();
            Render();
        }

        /// <summary>
        ///     Provides first-time initialization. This may not be necessary for all platforms
        /// </summary>
        public abstract void InitializeScottPlot();

        /// <summary>
        ///     Indicates whether the application is currently rendering the plotting window
        /// </summary>
        protected bool currentlyRendering = false;

        /// <summary>
        ///     Renders the plotting window.
        /// </summary>
        /// <param name="skipIfCurrentlyRendering">
        ///     Indicates whether the render should be skipped if rendering is already in process.
        /// </param>
        /// <param name="lowQuality">
        ///     Indicates whether the plotting window should be rendered in low quality (for example, antialiasing may be disabled).
        /// </param>
        /// <param name="recalculateLayout">
        ///     Indicates whether the layout should be recalculated prior to rendering.
        /// </param>
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false, bool recalculateLayout = false)
        {
            if (!isDesignerMode)
            {
                if (recalculateLayout)
                    plt.TightenLayout();

                if (equalAxes)
                    plt.AxisEqual();

                if (!(skipIfCurrentlyRendering && currentlyRendering))
                {
                    currentlyRendering = true;
                    SetImagePlot(lowQuality);
                    currentlyRendering = false;
                    Rendered?.Invoke(null, null);
                }
            }
        }

        /// <summary>
        ///     Displays the bitmap of the rendered plotting window.
        /// </summary>
        /// <param name="lowQuality">
        ///     Indicates whether the plotting window should be rendered in low quality (for example, antialiasing may be disabled).
        /// </param>
        public abstract void SetImagePlot(bool lowQuality);

        /// <summary>
        ///     A method to be called when the plotting window or it's container's size has changed.
        /// </summary>
        /// <param name="dpiCorrectedWidth">
        ///     The DPI corrected width of the window
        /// </param>
        /// <param name="dpiCorrectedHeight">
        ///     The DPI corrected height of the window
        /// </param>
        public void CanvasSizeChanged(int dpiCorrectedWidth, int dpiCorrectedHeight)
        {
            plt?.Resize(dpiCorrectedWidth, dpiCorrectedHeight);
            Render();
        }

        #region user control configuration
        /// <summary>
        ///     Indicates whether panning is enabled.
        /// </summary>
        private bool enablePanning = true;

        /// <summary>
        ///     Indicates whether zooming is enabled.
        /// </summary>
        private bool enableZooming = true;

        /// <summary>
        ///     Indicates whether zooming with the scroll wheel is enabled.
        /// </summary>
        private bool enableScrollWheelZoom = true;

        /// <summary>
        ///     Indicates whether render quality should be decreased when panning or zoooming
        /// </summary>
        private bool lowQualityWhileDragging = true;

        /// <summary>
        ///     Indicates whether double clicking should toggle benchmarking
        /// </summary>
        private bool doubleClickingTogglesBenchmark = true;

        /// <summary>
        ///     Indicates whether the vertical (y) axis should be locked, i.e. immovable.
        /// </summary>
        private bool lockVerticalAxis = false;

        /// <summary>
        ///     Indicates whether the horizontal (x) axis should be locked, i.e. immovable.
        /// </summary>
        private bool lockHorizontalAxis = false;

        /// <summary>
        ///     Indicates whether axes should have equal scale.
        /// </summary>
        private bool equalAxes = false;

        // I don't actually know what these do...
        private double middleClickMarginX = .1;
        private double middleClickMarginY = .1;

        /// <summary>
        ///     Indicates whether the layout should be recalculated upon release of the mouse button
        /// </summary>
        private bool? recalculateLayoutOnMouseUp = null;

        /// <summary>
        ///     Configures the user control with the given parameters.
        /// </summary>
        /// <param name="enablePanning">Sets <see cref="enablePanning"/></param>
        /// <param name="enableRightClickZoom">Sets <see cref="enableZooming"/></param>
        /// <param name="enableRightClickMenu">Indicates whether <c><see cref="contextMenuItems"/></c> should be initialized with the return value of <c><see cref="DefaultRightClickMenu"/></c></param>
        /// <param name="enableScrollWheelZoom">Sets <see cref="enableScrollWheelZoom"/></param>
        /// <param name="lowQualityWhileDragging">Sets <see cref="lowQualityWhileDragging"/></param>
        /// <param name="enableDoubleClickBenchmark">Sets <see cref="doubleClickingTogglesBenchmark"/></param>
        /// <param name="lockVerticalAxis">Sets <see cref="lockVerticalAxis"/></param>
        /// <param name="lockHorizontalAxis">Sets <see cref="lockHorizontalAxis"/></param>
        /// <param name="equalAxes">Sets <see cref="equalAxes"/></param>
        /// <param name="middleClickMarginX">Sets <see cref="middleClickMarginX"/></param>
        /// <param name="middleClickMarginY">Sets <see cref="middleClickMarginY"/></param>
        /// <param name="recalculateLayoutOnMouseUp">Sets <see cref="recalculateLayoutOnMouseUp"/></param>
        public void Configure(
            bool? enablePanning = null,
            bool? enableRightClickZoom = null,
            bool? enableRightClickMenu = null,
            bool? enableScrollWheelZoom = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null,
            double? middleClickMarginX = null,
            double? middleClickMarginY = null,
            bool? recalculateLayoutOnMouseUp = null
            )
        {
            if (enablePanning != null) this.enablePanning = (bool)enablePanning;
            if (enableRightClickZoom != null) this.enableZooming = (bool)enableRightClickZoom;
            if (enableRightClickMenu != null) contextMenuItems = (enableRightClickMenu.Value) ? DefaultRightClickMenu() : null;
            if (enableScrollWheelZoom != null) this.enableScrollWheelZoom = (bool)enableScrollWheelZoom;
            if (lowQualityWhileDragging != null) this.lowQualityWhileDragging = (bool)lowQualityWhileDragging;
            if (enableDoubleClickBenchmark != null) this.doubleClickingTogglesBenchmark = (bool)enableDoubleClickBenchmark;
            if (lockVerticalAxis != null) this.lockVerticalAxis = (bool)lockVerticalAxis;
            if (lockHorizontalAxis != null) this.lockHorizontalAxis = (bool)lockHorizontalAxis;
            if (equalAxes != null) this.equalAxes = (bool)equalAxes;
            this.middleClickMarginX = middleClickMarginX ?? this.middleClickMarginX;
            this.middleClickMarginY = middleClickMarginY ?? this.middleClickMarginY;
            this.recalculateLayoutOnMouseUp = recalculateLayoutOnMouseUp;
        }

        /// <summary>
        ///     Indicates whether the "Alt" key is pressed (Left or Right Alt)
        /// </summary>
        protected bool isAltPressed = false;

        /// <summary>
        ///     Indicates whether the "Shift" key is pressed (Left or Right Shift)
        /// </summary>
        protected bool isShiftPressed = false;

        /// <summary>
        ///     Indicates whether the "Control" key is pressed (Left or Right Ctrl)
        /// </summary>
        protected bool isCtrlPressed = false;

        #endregion

        #region mouse tracking

        /// <summary>
        ///     The point in plotting space where the left mouse button was pressed. This property should be set to null upon release of the button.
        /// </summary>
        public System.Drawing.PointF? mouseLeftDownLocation { get; private set; }

        /// <summary>
        ///     The point in plotting space where the right mouse button was pressed. This property should be set to null upon release of the button.
        /// </summary>
        public System.Drawing.PointF? mouseRightDownLocation { get; private set; }

        /// <summary>
        ///     The point in plotting space where the middle mouse button (scroll wheel button) was pressed. This property should be set to null upon release of the button.
        /// </summary>
        public System.Drawing.PointF? mouseMiddleDownLocation { get; private set; }

        /// <summary>
        ///     The axis limits at the time the mouse last transitioned from unpressed to pressed (i.e. the last time the mousedown event fired)
        /// </summary>
        double[] axisLimitsOnMouseDown;

        /// <summary>
        ///     Indicates whether the plotting window is being panned or zoomed.
        /// </summary>
        private bool isPanningOrZooming
        {
            get
            {
                if (axisLimitsOnMouseDown is null) return false;
                if (mouseLeftDownLocation != null) return true;
                else if (mouseRightDownLocation != null) return true;
                else if (mouseMiddleDownLocation != null) return true;
                return false;
            }
        }

        /// <summary>
        ///     The plottable being dragged by the mouse cursor, if applicable. Otherwise, <c>null</c>
        /// </summary>
        IDraggable plottableBeingDragged = null;

        /// <summary>
        ///     Indicates whether the user is moving an <c><see cref="IDraggable"/></c>
        /// </summary>
        private bool isMovingDraggable { get { return (plottableBeingDragged != null); } }

        /// <summary>
        ///     To be called on mouse dragging
        /// </summary>
        /// <param name="mousePosition">
        ///     The position of the mouse cursor in plotting space
        /// </param>
        /// <param name="button">
        ///     The mouse button being pressed
        /// </param>
        public void MouseDrag(PointF mousePosition, MouseButtons button)
        {
            plottableBeingDragged = plt.GetDraggableUnderMouse(mousePosition.X, mousePosition.Y);

            if (plottableBeingDragged is null)
            {
                // MouseDown event is to start a pan or zoom
                if (button == MouseButtons.Left && isAltPressed) mouseMiddleDownLocation = mousePosition;
                else if (button == MouseButtons.Left && enablePanning) mouseLeftDownLocation = mousePosition;
                else if (button == MouseButtons.Right && enableZooming) mouseRightDownLocation = mousePosition;
                else if (button == MouseButtons.Middle && enableScrollWheelZoom) mouseMiddleDownLocation = mousePosition;
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {
                // mouse is being used to drag a plottable
            }
        }

        /// <summary>
        ///     The location of the mouse cursor in plotting space
        /// </summary>
        protected PointF mouseLocation;

        /// <summary>
        ///     To be called on mouse movement
        /// </summary>
        /// <param name="pointerPosition">
        ///     The position of the mouse cursor in plotting space
        /// </param>
        public void MouseMove(PointF pointerPosition)
        {
            mouseLocation = pointerPosition;

            if (isPanningOrZooming)
                MouseMovedToPanOrZoom();
            else if (isMovingDraggable)
                MouseMovedToMoveDraggable();
            else
                MouseMovedWithoutInteraction(mouseLocation);
        }

        /// <summary>
        ///     To be called in the event that the mouse is moved in order to pan or zoom the plotting window
        /// </summary>
        private void MouseMovedToPanOrZoom()
        {
            plt.Axis(axisLimitsOnMouseDown);

            if (mouseLeftDownLocation != null)
            {
                // left-click-drag panning
                double deltaX = mouseLeftDownLocation.Value.X - mouseLocation.X;
                double deltaY = mouseLocation.Y - mouseLeftDownLocation.Value.Y;

                if (isCtrlPressed || lockVerticalAxis) deltaY = 0;
                if (isShiftPressed || lockHorizontalAxis) deltaX = 0;

                settings.AxesPanPx((int)deltaX, (int)deltaY);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseRightDownLocation != null)
            {
                // right-click-drag zooming
                double deltaX = mouseRightDownLocation.Value.X - mouseLocation.X;
                double deltaY = mouseLocation.Y - mouseRightDownLocation.Value.Y;

                if (isCtrlPressed == true && isShiftPressed == false) deltaY = 0;
                if (isShiftPressed == true && isCtrlPressed == false) deltaX = 0;

                settings.AxesZoomPx(-(int)deltaX, -(int)deltaY, lockRatio: isCtrlPressed && isShiftPressed);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseMiddleDownLocation != null)
            {
                // middle-click-drag zooming to rectangle
                double x1 = Math.Min(mouseLocation.X, mouseMiddleDownLocation.Value.X);
                double x2 = Math.Max(mouseLocation.X, mouseMiddleDownLocation.Value.X);
                double y1 = Math.Min(mouseLocation.Y, mouseMiddleDownLocation.Value.Y);
                double y2 = Math.Max(mouseLocation.Y, mouseMiddleDownLocation.Value.Y);

                var origin = new System.Drawing.Point((int)x1 - settings.dataOrigin.X, (int)y1 - settings.dataOrigin.Y);
                var size = new System.Drawing.Size((int)(x2 - x1), (int)(y2 - y1));

                if (lockVerticalAxis)
                {
                    origin.Y = 0;
                    size.Height = settings.dataSize.Height - 1;
                }
                if (lockHorizontalAxis)
                {
                    origin.X = 0;
                    size.Width = settings.dataSize.Width - 1;
                }

                settings.mouseMiddleRect = new System.Drawing.Rectangle(origin, size);
            }

            Render(true, lowQuality: lowQualityWhileDragging);
            return;
        }

        /// <summary>
        ///     Obtains the coordinates of the mouse in plotting space
        /// </summary>
        /// <returns>
        ///     A tuple containing the coordinates of the mouse in plotting space
        /// </returns>
        public (double x, double y) GetMouseCoordinates()
        {
            double x = plt.CoordinateFromPixelX(mouseLocation.X / dpiScale);
            double y = plt.CoordinateFromPixelY(mouseLocation.Y / dpiScale);
            return (x, y);
        }

        /// <summary>
        ///     To be called if the mouse is moved to move an <c><see cref="IDraggable"/></c>
        /// </summary>
        private void MouseMovedToMoveDraggable()
        {
            plottableBeingDragged.DragTo(
                plt.CoordinateFromPixelX(mouseLocation.X), plt.CoordinateFromPixelY(mouseLocation.Y),
                isShiftPressed, isAltPressed, isCtrlPressed);
            Render(true);
        }

        /// <summary>
        ///     To be called if the mouse is moved without further interaction. Often used to change the mouse cursor. This may be a noop on platforms which do not support changing the mouse cursor.
        /// </summary>
        /// <param name="mouseLocation">
        ///     The coordinates of the mouse in plotting space
        /// </param>
        public abstract void MouseMovedWithoutInteraction(PointF mouseLocation);

        /// <summary>
        ///     To be called on release of any mouse button
        /// </summary>
        public void MouseUp()
        {
            plottableBeingDragged = null;

            if (mouseMiddleDownLocation != null)
            {
                double x1 = Math.Min(mouseLocation.X, mouseMiddleDownLocation.Value.X);
                double x2 = Math.Max(mouseLocation.X, mouseMiddleDownLocation.Value.X);
                double y1 = Math.Min(mouseLocation.Y, mouseMiddleDownLocation.Value.Y);
                double y2 = Math.Max(mouseLocation.Y, mouseMiddleDownLocation.Value.Y);

                PointF topLeft = new PointF((float)x1, (float)y1);
                SizeF size = new SizeF((float)(x2 - x1), (float)(y2 - y1));
                PointF botRight = new PointF(topLeft.X + size.Width, topLeft.Y + size.Height);

                if ((size.Width > 2) && (size.Height > 2))
                {
                    // only change axes if suffeciently large square was drawn
                    if (!lockHorizontalAxis)
                        plt.Axis(
                            x1: plt.CoordinateFromPixelX(topLeft.X),
                            x2: plt.CoordinateFromPixelX(botRight.X));
                    if (!lockVerticalAxis)
                        plt.Axis(
                            y1: plt.CoordinateFromPixelY(botRight.Y),
                            y2: plt.CoordinateFromPixelY(topLeft.Y));
                    AxisChanged?.Invoke(null, null);
                }
                else
                {
                    bool shouldTighten = recalculateLayoutOnMouseUp ?? !plotContainsHeatmap;
                    plt.AxisAuto(middleClickMarginX, middleClickMarginY, tightenLayout: shouldTighten);
                    AxisChanged?.Invoke(null, null);
                }
            }

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;

            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plotContainsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
        }

        #endregion

        #region mouse clicking

        /// <summary>
        ///     To be called on mouse wheel scroll
        /// </summary>
        /// <param name="yScroll">
        ///     The scroll of the mouse in the y direction (horizontal scrolling is ignored).
        /// </param>
        public void MouseWheel(double yScroll)
        {
            if (enableScrollWheelZoom == false)
                return;

            double xFrac = (yScroll > 0) ? 1.15 : 0.85;
            double yFrac = (yScroll > 0) ? 1.15 : 0.85;

            if (isCtrlPressed) yFrac = 1;
            if (isShiftPressed) xFrac = 1;

            plt.AxisZoom(xFrac, yFrac, plt.CoordinateFromPixelX(mouseLocation.X), plt.CoordinateFromPixelY(mouseLocation.Y));
            AxisChanged?.Invoke(null, null);
            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plotContainsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
        }


        /// <summary>
        ///     To be called if the mouse is double-clicked
        /// </summary>
        public void MouseDoubleClick()
        {
            if (doubleClickingTogglesBenchmark)
            {
                plt.Benchmark(toggle: true);
                Render();
            }
        }


        /// <summary>
        ///     To be called when a user wants to save an image to disk
        /// </summary>
        public abstract void SaveImage();

        /// <summary>
        ///     To be called when a user wants to copy an image to their system clipboard. This may be a noop on platforms which do not support this behaviour.
        /// </summary>
        public abstract void CopyImage();

        /// <summary>
        ///     To be called when a user wants to open the plotting window in a new window.
        /// </summary>
        public abstract void OpenInNewWindow();

        /// <summary>
        ///     To be called when a user wants to access the help page.
        /// </summary>
        public abstract void OpenHelp();

        #endregion

        #region event handling

        /// <summary>
        ///     An event invoked on every render of the plotting window.
        /// </summary>
        public event EventHandler Rendered;

        /// <summary>
        ///     An event invoked on every change to the axes of the plotting window.
        /// </summary>
        public event EventHandler AxisChanged;

        #endregion


    }
}
