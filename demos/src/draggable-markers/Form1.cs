using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDraggableMarkers
{
    public partial class Form1 : Form
    {
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] Ys = ScottPlot.DataGen.Sin(pointCount, 2);
            double[] Xs = ScottPlot.DataGen.Consecutive(pointCount);
            scottPlotUC1.plt.Grid(false);
            scottPlotUC1.plt.PlotScatter(Xs, Ys);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
            UpdateMessage();
            BtnAddVline_Click(null, null);
        }

        private void BtnAddHline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 2 - 1;
            scottPlotUC1.plt.PlotHLine(position, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);
            scottPlotUC1.Render();
            UpdateMessage();
        }

        private void BtnAddVline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 50;
            scottPlotUC1.plt.PlotVLine(position, draggable: true, dragLimitLower: 0, dragLimitUpper: 49);
            scottPlotUC1.Render();
            UpdateMessage();
        }

        private void BtnClearLines_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Clear(scatterPlots: false);
            scottPlotUC1.Render();
            UpdateMessage();
        }

        private void UpdateMessage()
        {

            string msg = "";
            var plottables = scottPlotUC1.plt.GetPlottables();
            for (int i = 0; i < plottables.Count; i++)
            {
                if (plottables[i] is ScottPlot.PlottableAxLine axLine)
                {
                    string lineType = (axLine.vertical) ? "VLine" : "HLine";
                    msg += $"{i}: {lineType} at {Math.Round(axLine.position, 4)}\r\n";
                }
            }
            richTextBox1.Text = msg;
        }

        private void ScottPlotUC1_MouseDownOnPlottable(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked on a plottable object");
            richTextBox1.Enabled = false;
            UpdateMessage();
        }

        private void ScottPlotUC1_MouseDragPlottable(object sender, EventArgs e)
        {
            Console.WriteLine("Moved a plottable object");
            UpdateMessage();
        }

        private void ScottPlotUC1_MouseDropPlottable(object sender, EventArgs e)
        {
            Console.WriteLine("Dropped a plottable object");
            UpdateMessage();
            richTextBox1.Enabled = true;
        }
    }
}
