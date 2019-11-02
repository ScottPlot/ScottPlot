using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace ScottPlot
{

    public partial class FormsPlot : UserControl
    {
        public Plot plt = new Plot();

        private readonly bool isDesignerMode;

        public FormsPlot()
        {
            isDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";
            InitializeComponent();
            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;

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

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) mouseLeftDownLocation = e.Location;
            else if (e.Button == MouseButtons.Right) mouseRightDownLocation = e.Location;
            else if (e.Button == MouseButtons.Middle) mouseMiddleDownLocation = e.Location;

            axisLimitsOnMouseDown = plt.Axis();
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
                    plt.GetSettings().AxesPanPx(deltaX, deltaY);
                    Render(true);
                }

                if (mouseRightDownLocation != null)
                {
                    int deltaX = ((Point)mouseRightDownLocation).X - mouseLocation.X;
                    int deltaY = mouseLocation.Y - ((Point)mouseRightDownLocation).Y;
                    plt.GetSettings().AxesZoomPx(-deltaX, -deltaY);
                    Render(true);
                }
            }
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
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
