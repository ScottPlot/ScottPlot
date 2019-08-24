using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScottPlot
{

    public partial class FormsPlot : UserControl
    {
        public Plot plt = new Plot();

        private bool currentlyRendering = false;
        private System.Timers.Timer lastInteractionTimer;
        ContextMenuStrip rightClickMenu;

        public FormsPlot()
        {
            InitializeComponent();
            SetupMenu();
            SetupTimers();
            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;
            if (Process.GetCurrentProcess().ProcessName == "devenv")
                Tools.DesignerModeDemoPlot(plt);
            PbPlot_SizeChanged(null, null);
        }

        public void SetupTimers()
        {
            lastInteractionTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                SynchronizingObject = this,
                Enabled = false,
            };
            lastInteractionTimer.Elapsed += (o, arg) => Render(skipIfCurrentlyRendering: false);
        }

        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {
            if (lastInteractionTimer.Enabled)
                lastInteractionTimer.Stop();

            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt.GetBitmap(true, lowQuality);
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                currentlyRendering = false;
            }
        }

        private bool enableRightClickMenu = true;
        public void Configure(
            bool? enablePanning = null,
            bool? enableZooming = null,
            bool? enableRightClickMenu = null
            )
        {
            if (enablePanning != null)
                plt.mouseTracker.enablePanning = (bool)enablePanning;
            if (enableZooming != null)
                plt.mouseTracker.enableZooming = (bool)enableZooming;
            if (enableRightClickMenu != null)
                this.enableRightClickMenu = (bool)enableRightClickMenu;
        }

        private void SetupMenu()
        {
            rightClickMenu = new ContextMenuStrip();
            rightClickMenu.Items.Add("Save Image");
            rightClickMenu.Items.Add("Settings");
            rightClickMenu.Items.Add("Help");
            rightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);
        }

        public virtual void LaunchMenu()
        {
            // override this to use your own custom menu
            if (enableRightClickMenu)
            {
                rightClickMenu.Show(pbPlot, PointToClient(Cursor.Position));
                Render(skipIfCurrentlyRendering: false, lowQuality: false);
            }
        }

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            rightClickMenu.Hide();
            switch (e.ClickedItem.Text)
            {
                case "Save Image":
                    Tools.SaveImageDialog(plt);
                    break;

                case "Settings":
                    var formSettings = new UserControls.FormSettings(plt);
                    formSettings.ShowDialog();
                    Render();
                    break;

                case "Help":
                    var formHelp = new UserControls.FormHelp();
                    formHelp.ShowDialog();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            plt.Resize(Width, Height);
            Render(skipIfCurrentlyRendering: false);
        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseDown(e.Location);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDownOnPlottable(EventArgs.Empty);
        }

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseMove(e.Location);
            OnMouseMoved(EventArgs.Empty);

            Plottable plottableUnderCursor = plt.mouseTracker.PlottableUnderCursor(e.Location);
            if (plottableUnderCursor != null)
            {
                if (e.Button != MouseButtons.None)
                    OnMouseDragPlottable(EventArgs.Empty);
                if (plottableUnderCursor is PlottableAxLine axLine)
                {
                    if (axLine.vertical == true)
                        pbPlot.Cursor = Cursors.SizeWE;
                    else
                        pbPlot.Cursor = Cursors.SizeNS;
                }
                else
                {
                    pbPlot.Cursor = Cursors.Arrow;
                }
            }

            if (e.Button != MouseButtons.None)
            {
                Render(skipIfCurrentlyRendering: true, lowQuality: plt.mouseTracker.lowQualityWhileInteracting);
                OnMouseDragged(EventArgs.Empty);
            }
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            if ((!plt.mouseTracker.MouseHasMoved()) && (e.Button == MouseButtons.Right))
            {
                LaunchMenu();
                return;
            }

            plt.mouseTracker.MouseUp(e.Location);
            if (plt.mouseTracker.lowQualityWhileInteracting && plt.mouseTracker.mouseUpHQRenderDelay > 0)
            {
                Render(false, true);
                lastInteractionTimer.Interval = plt.mouseTracker.mouseUpHQRenderDelay;
                lastInteractionTimer.Start();
            }
            else
            {
                Render(skipIfCurrentlyRendering: false);
            }

            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDropPlottable(EventArgs.Empty);

            plt.mouseTracker.MouseIs(false, false);
        }

        private void PbPlot_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClicked(e);
            if (e.Button == MouseButtons.Middle)
                Render(skipIfCurrentlyRendering: false);
            else if (e.Button == MouseButtons.Right && plt.mouseTracker.mouseDownStopwatch.ElapsedMilliseconds < 100)
                LaunchMenu();
            if (e.Button == MouseButtons.Middle)
                plt.mouseTracker.MiddleButtonClicked();
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            plt.Benchmark(toggle: true);
            Render(skipIfCurrentlyRendering: false);
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            PointF zoomCenter = plt.CoordinateFromPixel(e.Location);

            double zoomAmountY = 0.15;
            if (plt.mouseTracker.ctrlIsDown())
                zoomAmountY = 0;

            double zoomAmountX = 0.15;
            if (plt.mouseTracker.altIsDown())
                zoomAmountX = 0;

            if (e.Delta > 0)
                plt.AxisZoom(1 + zoomAmountX, 1 + zoomAmountY, zoomCenter);
            else
                plt.AxisZoom(1 - zoomAmountX, 1 - zoomAmountY, zoomCenter);

            if (plt.mouseTracker.lowQualityWhileInteracting && plt.mouseTracker.mouseWheelHQRenderDelay > 0)
            {
                Render(false, true);
                lastInteractionTimer.Interval = plt.mouseTracker.mouseWheelHQRenderDelay; // delay in ms
                lastInteractionTimer.Start();
            }
            else
                Render(skipIfCurrentlyRendering: false);
        }

        public event EventHandler MouseDownOnPlottable;
        public event EventHandler MouseDragPlottable;
        public event EventHandler MouseMoved;
        public event EventHandler MouseDragged;
        public event EventHandler MouseDropPlottable;
        public event MouseEventHandler MouseClicked;
        protected virtual void OnMouseDownOnPlottable(EventArgs e) { MouseDownOnPlottable?.Invoke(this, e); }
        protected virtual void OnMouseDragPlottable(EventArgs e) { MouseDragPlottable?.Invoke(this, e); }
        protected virtual void OnMouseMoved(EventArgs e) { MouseMoved?.Invoke(this, e); }
        protected virtual void OnMouseDragged(EventArgs e) { MouseDragged?.Invoke(this, e); }
        protected virtual void OnMouseDropPlottable(EventArgs e) { MouseDropPlottable?.Invoke(this, e); }
        protected virtual void OnMouseClicked(MouseEventArgs e) { MouseClicked?.Invoke(this, e); }
    }
}
