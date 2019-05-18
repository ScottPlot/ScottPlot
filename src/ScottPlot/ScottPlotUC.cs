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
        private bool mouseMoveRedrawInProgress = false;

        public ScottPlotUC()
        {
            InitializeComponent();
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
            plt.Axis(0, 1, -1, 1);
            plt.title = "ScottPlot User Control";
            plt.yLabel = "Sample Data";
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
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            plt.settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
            plt.settings.MouseUp();
            Render();
        }
    }
}
