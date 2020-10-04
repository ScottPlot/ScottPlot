using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Interactive
{
    public struct ContextMenuItem
    {
        public string itemName;
        public Action onClick;
    }

    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    public abstract class ControlBackend
    {
        public Plot plt { get; protected set; }
        protected Settings settings;
        protected bool isDesignerMode;
        public double dpiScaleInput { get; protected set; } = 1;
        public double dpiScaleOutput { get; protected set; } = 1;
        private bool plotContainsHeatmap => settings?.plottables.Where(p => p is PlottableHeatmap).Count() > 0;


        public List<ContextMenuItem> contextMenuItems;

        public virtual List<ContextMenuItem> DefaultRightClickMenu()
        {
            return new List<ContextMenuItem>();
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Plot plt)
        {
            this.plt = (plt is null) ? new Plot() : plt;
            InitializeScottPlot();
            Render();
        }
        public abstract void InitializeScottPlot();

        protected bool currentlyRendering = false;
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

        public abstract void SetImagePlot(bool lowQuality);

        public void CanvasSizeChanged(int dpiCorrectedWidth, int dpiCorrectedHeight)
        {
            plt?.Resize(dpiCorrectedWidth, dpiCorrectedHeight);
            Render();
        }

        #region user control configuration

        private bool enablePanning = true;
        private bool enableZooming = true;
        private bool enableScrollWheelZoom = true;
        private bool lowQualityWhileDragging = true;
        private bool doubleClickingTogglesBenchmark = true;
        private bool lockVerticalAxis = false;
        private bool lockHorizontalAxis = false;
        private bool equalAxes = false;
        private double middleClickMarginX = .1;
        private double middleClickMarginY = .1;
        private bool? recalculateLayoutOnMouseUp = null;
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

        protected bool isAltPressed = false;
        protected bool isShiftPressed = false;
        protected bool isCtrlPressed = false;

        #endregion

        #region mouse tracking

        public System.Drawing.PointF? mouseLeftDownLocation { get; private set; }
        public System.Drawing.PointF? mouseRightDownLocation { get; private set; }
        public System.Drawing.PointF? mouseMiddleDownLocation { get; private set; }

        double[] axisLimitsOnMouseDown;
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

        IDraggable plottableBeingDragged = null;
        private bool isMovingDraggable { get { return (plottableBeingDragged != null); } }

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

        protected PointF mouseLocation;
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

        public (double x, double y) GetMouseCoordinates()
        {
            double x = plt.CoordinateFromPixelX(mouseLocation.X / dpiScaleInput);
            double y = plt.CoordinateFromPixelY(mouseLocation.Y / dpiScaleInput);
            return (x, y);
        }

        private void MouseMovedToMoveDraggable()
        {
            plottableBeingDragged.DragTo(
                plt.CoordinateFromPixelX(mouseLocation.X), plt.CoordinateFromPixelY(mouseLocation.Y),
                isShiftPressed, isAltPressed, isCtrlPressed);
            Render(true);
        }

        public abstract void MouseMovedWithoutInteraction(PointF mouseLocation);

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

        public void MouseDoubleClick()
        {
            if (doubleClickingTogglesBenchmark)
            {
                plt.Benchmark(toggle: true);
                Render();
            }
        }

        public abstract void SaveImage();

        public abstract void CopyImage();

        public abstract void OpenInNewWindow();

        public abstract void OpenHelp();

        #endregion

        #region event handling

        public event EventHandler Rendered;
        public event EventHandler AxisChanged;

        #endregion


    }
}
