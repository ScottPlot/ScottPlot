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

        public ScottPlotUC()
        {
            InitializeComponent();
            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;
            RightClickMenuSetup();
            UpdateSize();
            Render();
        }

        private void ScottPlotUC_Load(object sender, EventArgs e)
        {
            bool isInFormsDesignerMode = (Process.GetCurrentProcess().ProcessName == "devenv");
            if (isInFormsDesignerMode)
                PlotDemoData();
        }

        private void PlotDemoData(int pointCount = 101)
        {
            double pointSpacing = .01;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount, pointSpacing);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto(0);
            plt.Title("ScottPlot User Control");
            plt.YLabel("Sample Data");
            Render();
        }

        private void UpdateSize()
        {
            plt.Resize(Width, Height);
        }

        private bool busyRendering = false;
        public void Render(bool skipIfBusy = false)
        {
            if (skipIfBusy && busyRendering)
            {
                return;
            }
            else
            {
                busyRendering = true;
                pbPlot.Image = plt.GetBitmap();
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                busyRendering = false;
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
            Render();
        }

        public event EventHandler MouseDownOnPlottable;
        protected virtual void OnMouseDownOnPlottable(EventArgs e)
        {
            var handler = MouseDownOnPlottable;
            if (handler != null)
                handler(this, e);
        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseDown(e.Location);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDownOnPlottable(EventArgs.Empty);
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

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseMove(e.Location);
            OnMouseMoved(EventArgs.Empty);

            // do things based on if the mouse is over a plottable object
            var hoverPlottable = plt.mouseTracker.PlottableUnderCursor(e.Location);
            if (hoverPlottable != null)
            {
                if (e.Button != MouseButtons.None)
                    OnMouseDragPlottable(EventArgs.Empty);
                if (hoverPlottable is PlottableAxLine axLine)
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
                Render(skipIfBusy: true);
                OnMouseDragged(EventArgs.Empty);
            }
        }

        public event EventHandler MouseDropPlottable;
        protected virtual void OnMouseDropPlottable(EventArgs e)
        {
            var handler = MouseDropPlottable;
            if (handler != null)
                handler(this, e);
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseUp(e.Location);
            Render(true);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDropPlottable(EventArgs.Empty);
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

        private ContextMenuStrip cmRightClickMenu;
        private void RightClickMenuSetup()
        {
            cmRightClickMenu = new ContextMenuStrip();
            cmRightClickMenu.Items.Add("Save Image");
            cmRightClickMenu.Items.Add("Auto-Axis");
            cmRightClickMenu.Items.Add("Clear");
            cmRightClickMenu.Items.Add(new ToolStripSeparator());
            cmRightClickMenu.Items.Add("Help");
            int i = cmRightClickMenu.Items.Count - 1;
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("left-click-drag to pan");
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("right-click-drag to zoom");
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("middle-click for auto-axis");
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems.Add("double-click to toggle benchmark");
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[0].Enabled = false;
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[1].Enabled = false;
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[2].Enabled = false;
            (cmRightClickMenu.Items[i] as ToolStripMenuItem).DropDownItems[3].Enabled = false;
            cmRightClickMenu.Items.Add(new ToolStripSeparator());
            cmRightClickMenu.Items.Add("About ScottPlot");
        }
        private void RightClickMenu()
        {
            cmRightClickMenu.Show(pbPlot, PointToClient(Cursor.Position));
            cmRightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);
        }

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            switch (item.ToString())
            {
                case "Save Image":
                    cmRightClickMenu.Hide();
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = "ScottPlot.png";
                    savefile.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                        plt.SaveFig(savefile.FileName);
                    break;
                case "Auto-Axis":
                    cmRightClickMenu.Hide();
                    plt.AxisAuto();
                    OnMouseDragged(EventArgs.Empty);
                    Render();
                    break;
                case "Clear":
                    cmRightClickMenu.Hide();
                    plt.Clear();
                    Render();
                    break;
                case "About ScottPlot":
                    cmRightClickMenu.Hide();
                    System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
                    break;
            }
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
                RightClickMenu();
            }
        }
    }
}
