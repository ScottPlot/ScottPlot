using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace ScottPlot
{

    public partial class FormsPlot : UserControl
    {
        public readonly Plot plt;
        private readonly Settings settings;

        private readonly bool isDesignerMode;

        public FormsPlot()
        {
            InitializeComponent();
            pbPlot.MouseWheel += PbPlot_MouseWheel;

            isDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";

            plt = new Plot();
            plt.Style(Style.Control);
            settings = plt.GetSettings(showWarning: false);

            PbPlot_MouseUp(null, null);
            PbPlot_SizeChanged(null, null);
        }

        private bool currentlyRendering;
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {
            if (isDesignerMode)
            {
                pbPlot.Image = Tools.DesignerModeBitmap(pbPlot.Size);
            }
            else
            {
                if (!(skipIfCurrentlyRendering && currentlyRendering))
                {
                    currentlyRendering = true;
                    pbPlot.Image = plt.GetBitmap(true, lowQuality);
                    if (isMouseDragging)
                        Application.DoEvents();
                    currentlyRendering = false;
                }
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            plt.Resize(Width, Height);
            Render(skipIfCurrentlyRendering: false);
        }

        #region user control configuration

        private bool enablePadding;
        private bool enableZooming;
        private bool enableRightClickMenu;
        public void Configure(
            bool? enablePanning = null,
            bool? enableZooming = null,
            bool? enableRightClickMenu = null
            )
        {
            if (enablePanning != null) this.enablePadding = (bool)enablePanning;
            if (enableZooming != null) this.enableZooming = (bool)enableZooming;
            if (enableRightClickMenu != null) this.enableRightClickMenu = (bool)enableRightClickMenu;
        }

        #endregion

        #region mouse tracking

        private Point? mouseLeftDownLocation, mouseRightDownLocation, mouseMiddleDownLocation;
        private Point mouseLocation;
        double[] axisLimitsOnMouseDown;
        private bool isMouseDragging
        {
            get
            {
                if (axisLimitsOnMouseDown is null)
                    return false;

                if (mouseLeftDownLocation != null) return true;
                else if (mouseRightDownLocation != null) return true;
                else if (mouseMiddleDownLocation != null) return true;

                return false;
            }
        }

        PlottableAxLine draggingAxLine = null;
        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {

            draggingAxLine = settings.GetDraggableAxisLineUnderCursor(e.Location);

            if (draggingAxLine is null)
            {
                // mouse being used for click and zoom
                if (e.Button == MouseButtons.Left) mouseLeftDownLocation = e.Location;
                else if (e.Button == MouseButtons.Right) mouseRightDownLocation = e.Location;
                else if (e.Button == MouseButtons.Middle) mouseMiddleDownLocation = e.Location;
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {

            }
        }

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;

            if (isMouseDragging)
            {
                plt.Axis(axisLimitsOnMouseDown);

                if (mouseLeftDownLocation != null)
                {
                    int deltaX = ((Point)mouseLeftDownLocation).X - mouseLocation.X;
                    int deltaY = mouseLocation.Y - ((Point)mouseLeftDownLocation).Y;
                    settings.AxesPanPx(deltaX, deltaY);
                }
                else if (mouseRightDownLocation != null)
                {
                    int deltaX = ((Point)mouseRightDownLocation).X - mouseLocation.X;
                    int deltaY = mouseLocation.Y - ((Point)mouseRightDownLocation).Y;
                    settings.AxesZoomPx(-deltaX, -deltaY);
                }
                else if (mouseMiddleDownLocation != null)
                {
                    int x1 = Math.Min(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                    int x2 = Math.Max(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                    int y1 = Math.Min(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);
                    int y2 = Math.Max(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);

                    Point origin = new Point(x1 - settings.dataOrigin.X, y1 - settings.dataOrigin.Y);
                    Size size = new Size(x2 - x1, y2 - y1);

                    settings.mouseMiddleRect = new Rectangle(origin, size);
                }

                Render(true);
                return;
            }

            if (draggingAxLine != null)
            {
                var pos = plt.CoordinateFromPixel(e.Location);
                draggingAxLine.position = (draggingAxLine.vertical) ? pos.X : pos.Y;
                pbPlot.Cursor = (draggingAxLine.vertical == true) ? Cursors.SizeWE : Cursors.SizeNS;
                Render(true);
                return;
            }

            var axLineUnderCursor = settings.GetDraggableAxisLineUnderCursor(e.Location);
            if (axLineUnderCursor is null)
                pbPlot.Cursor = Cursors.Arrow;
            else
                pbPlot.Cursor = (axLineUnderCursor.vertical == true) ? Cursors.SizeWE : Cursors.SizeNS;
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseMiddleDownLocation != null)
            {
                int x1 = Math.Min(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                int x2 = Math.Max(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                int y1 = Math.Min(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);
                int y2 = Math.Max(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);

                Point topLeft = new Point(x1, y1);
                Size size = new Size(x2 - x1, y2 - y1);
                Point botRight = new Point(topLeft.X + size.Width, topLeft.Y + size.Height);

                if ((size.Width > 2) && (size.Height > 2))
                {
                    // only change axes if suffeciently large square was drawn
                    plt.Axis(
                            x1: plt.CoordinateFromPixel(topLeft).X,
                            x2: plt.CoordinateFromPixel(botRight).X,
                            y1: plt.CoordinateFromPixel(botRight).Y,
                            y2: plt.CoordinateFromPixel(topLeft).Y
                        );
                }
            }

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;
            draggingAxLine = null;
            Render();
        }

        #endregion

        #region mouse clicking

        private void PbPlot_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                plt.AxisAuto();
                Render();
            }
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            plt.Benchmark(toggle: true);
            Render();
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            double xFrac = (e.Delta > 0) ? 1.15 : 0.85;
            double yFrac = (e.Delta > 0) ? 1.15 : 0.85;
            plt.AxisZoom(xFrac, yFrac, plt.CoordinateFromPixel(e.Location));
            Render();
        }

        #endregion

        #region custom events

        public event EventHandler MouseDownOnPlottable;
        public event EventHandler MouseDragPlottable;
        public event EventHandler MouseMoved;
        public event EventHandler MouseDragged;
        public event EventHandler MouseDropPlottable;
        public event EventHandler AxesChanged;
        public event MouseEventHandler MouseClicked;
        protected virtual void OnMouseDownOnPlottable(EventArgs e) { MouseDownOnPlottable?.Invoke(this, e); }
        protected virtual void OnMouseDragPlottable(EventArgs e) { MouseDragPlottable?.Invoke(this, e); }
        protected virtual void OnMouseMoved(EventArgs e) { MouseMoved?.Invoke(this, e); }
        protected virtual void OnMouseDragged(EventArgs e) { MouseDragged?.Invoke(this, e); }
        protected virtual void OnMouseDropPlottable(EventArgs e) { MouseDropPlottable?.Invoke(this, e); }
        protected virtual void OnMouseClicked(MouseEventArgs e) { MouseClicked?.Invoke(this, e); }

        #endregion
    }
}
