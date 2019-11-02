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

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void PbPlot_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {

        }

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
