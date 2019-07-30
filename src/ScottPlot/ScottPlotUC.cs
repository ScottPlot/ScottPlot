using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScottPlot
{
    public partial class ScottPlotUC : UserControl
    {
        public Plot plt = new Plot();
        private bool currentlyRendering = false;
        private ContextMenuStrip rightClickMenu;

        public ScottPlotUC()
        {
            InitializeComponent();

            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;
            RightClickMenuSetup();
            if (Process.GetCurrentProcess().ProcessName == "devenv")
                ScottPlot.UserControlTools.DesignerModeDemoPlot(plt);

            PbPlot_SizeChanged(null, null);
        }

        public void Render(bool skipIfCurrentlyRendering = false)
        {
            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt.GetBitmap();
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                currentlyRendering = false;
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            plt.Resize(Width, Height);
            Render();
        }

        #region mouse events

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

            var plottableUnderCursor = plt.mouseTracker.PlottableUnderCursor(e.Location);
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
            }
            else
            {
                pbPlot.Cursor = Cursors.Arrow;
            }

            if (e.Button != MouseButtons.None)
            {
                Render(skipIfCurrentlyRendering: true);
                OnMouseDragged(EventArgs.Empty);
            }
        }


        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseUp(e.Location);
            Render(true);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDropPlottable(EventArgs.Empty);
        }

        private void PbPlot_MouseClick(object sender, MouseEventArgs e)
        {
            double mouseDownMsec = plt.mouseTracker.mouseDownStopwatch.ElapsedMilliseconds;
            if (e.Button == MouseButtons.Middle)
            {
                plt.AxisAuto();
                OnMouseDragged(EventArgs.Empty);
                Render();
            }
            else if (e.Button == MouseButtons.Right && mouseDownMsec < 100)
            {
                rightClickMenu.Show(pbPlot, PointToClient(Cursor.Position));
                rightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);
            }
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            plt.Benchmark(toggle: true);
            Render();
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            double zoomAmount = 0.15;
            PointF zoomCenter = plt.CoordinateFromPixel(e.Location);
            if (e.Delta > 0)
                plt.AxisZoom(1 + zoomAmount, 1 + zoomAmount, zoomCenter);
            else
                plt.AxisZoom(1 - zoomAmount, 1 - zoomAmount, zoomCenter);
            Render();
        }

        #endregion

        #region right click menu

        private void RightClickMenuSetup()
        {
            rightClickMenu = new ContextMenuStrip();
            rightClickMenu.Items.Add("Save Image");
            rightClickMenu.Items.Add("Auto-Axis");
            rightClickMenu.Items.Add("Clear");
            rightClickMenu.Items.Add(new ToolStripSeparator());
            rightClickMenu.Items.Add("Help");
            int i = rightClickMenu.Items.Count - 1;
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("left-click-drag to pan");
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("right-click-drag to zoom");
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("middle-click for auto-axis");
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("double-click to toggle benchmark");
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[0].Enabled = false;
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[1].Enabled = false;
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[2].Enabled = false;
            (rightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[3].Enabled = false;
            rightClickMenu.Items.Add(new ToolStripSeparator());
            rightClickMenu.Items.Add("About ScottPlot");
        }

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            switch (item.ToString())
            {
                case "Save Image":
                    rightClickMenu.Hide();
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = "ScottPlot.png";
                    savefile.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                        plt.SaveFig(savefile.FileName);
                    break;
                case "Auto-Axis":
                    rightClickMenu.Hide();
                    plt.AxisAuto();
                    OnMouseDragged(EventArgs.Empty);
                    Render();
                    break;
                case "Clear":
                    rightClickMenu.Hide();
                    plt.Clear();
                    Render();
                    break;
                case "About ScottPlot":
                    rightClickMenu.Hide();
                    System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
                    break;
            }
        }

        #endregion

        #region custom events

        public event EventHandler MouseDownOnPlottable;
        protected virtual void OnMouseDownOnPlottable(EventArgs e)
        {
            var handler = MouseDownOnPlottable;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler MouseDragPlottable;
        protected virtual void OnMouseDragPlottable(EventArgs e)
        {
            var handler = MouseDragPlottable;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler MouseMoved;
        protected virtual void OnMouseMoved(EventArgs e)
        {
            var handler = MouseMoved;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler MouseDragged;
        protected virtual void OnMouseDragged(EventArgs e)
        {
            var handler = MouseDragged;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler MouseDropPlottable;
        protected virtual void OnMouseDropPlottable(EventArgs e)
        {
            var handler = MouseDropPlottable;
            if (handler != null)
                handler(this, e);
        }
        #endregion
    }
}
