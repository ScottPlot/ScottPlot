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
        Stopwatch mouseDownStopwatch = new Stopwatch();
        private bool mouseMoveRedrawInProgress = false;

        public ScottPlotUC()
        {
            InitializeComponent();
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

        public void Render()
        {
            pbPlot.Image = plt.GetBitmap();
            Application.DoEvents();
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("SizeChanged");
            UpdateSize();
            Render();
        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownStopwatch.Restart();
            if (Control.MouseButtons == MouseButtons.Left)
                plt.settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, panning: true);
            else if (Control.MouseButtons == MouseButtons.Right)
                plt.settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, zooming: true);
        }

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseMoveRedrawInProgress && Control.MouseButtons != MouseButtons.None)
            {
                mouseMoveRedrawInProgress = true;
                plt.settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
                Render();
                mouseMoveRedrawInProgress = false;
            }

            bool showMouseLocation = false;
            if (showMouseLocation)
            {
                PointF position = plt.settings.GetLocation(e.Location);
                Console.WriteLine($"cursor is at {e.Location} which is {position}");
            }
            
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            plt.settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
            plt.settings.MouseUp();
            Render();
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            plt.settings.displayBenchmark = !plt.settings.displayBenchmark;
            Render();
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                plt.AxisZoom(1.5, 1.5);
            else
                plt.AxisZoom(0.5, 0.5);
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
            double mouseDownMsec = mouseDownStopwatch.ElapsedMilliseconds;
            if (e.Button == MouseButtons.Middle)
            {
                plt.AxisAuto();
                Render();
            }
            else if (e.Button == MouseButtons.Right && mouseDownMsec < 100)
            {
                RightClickMenu();
            }
        }
    }
}
