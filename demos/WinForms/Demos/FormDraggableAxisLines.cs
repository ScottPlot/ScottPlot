using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormDraggableAxisLines : Form
    {
        Random rand = new Random();

        public FormDraggableAxisLines()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] Ys = ScottPlot.DataGen.Sin(pointCount, 2);
            double[] Xs = ScottPlot.DataGen.Consecutive(pointCount);
            formsPlot1.plt.Grid(false);
            formsPlot1.plt.PlotScatter(Xs, Ys);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
            UpdateMessage();

            BtnAddVline_Click(null, null);
            BtnAddHspan_Click(null, null);
        }

        private void BtnAddHline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 2 - 1;
            formsPlot1.plt.PlotHLine(position, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);
            formsPlot1.Render();
            UpdateMessage();
        }

        private void BtnAddVline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 50;
            formsPlot1.plt.PlotVLine(position, draggable: true, dragLimitLower: 0, dragLimitUpper: 49);
            formsPlot1.Render();
            UpdateMessage();
        }

        private void BtnClearLines_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Clear(scatterPlots: false, axisSpans: false);
            formsPlot1.Render();
            UpdateMessage();
        }

        private void UpdateMessage()
        {

            string msg = "";
            var plottables = formsPlot1.plt.GetPlottables();
            for (int i = 0; i < plottables.Count; i++)
            {
                if (plottables[i] is ScottPlot.PlottableAxLine axLine)
                {
                    string lineType = (axLine.vertical) ? "VLine" : "HLine";
                    msg += $"{i}: {lineType} ({Math.Round(axLine.position, 4)})\r\n";
                }
                else if (plottables[i] is ScottPlot.PlottableAxSpan axSpan)
                {
                    string lineType = (axSpan.vertical) ? "VSpan" : "HSpan";
                    msg += $"{i}: {lineType} ({Math.Round(axSpan.position1, 4)} to {Math.Round(axSpan.position2, 4)})\r\n";
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

        private void BtnAddHspan_Click(object sender, EventArgs e)
        {
            (double y1, double y2) = ScottPlot.DataGen.RandomSpan(rand: null, low: -1, high: 1, minimumSpacing: .25);

            formsPlot1.plt.PlotHSpan(y1, y2, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);
            formsPlot1.Render();
            UpdateMessage();
        }

        private void BtnAddVSpan_Click(object sender, EventArgs e)
        {
            (double x1, double x2) = ScottPlot.DataGen.RandomSpan(rand: null, low: 0, high: 50, minimumSpacing: 10);

            formsPlot1.plt.PlotVSpan(x1, x2, draggable: true, dragLimitLower: 0, dragLimitUpper: 50);
            formsPlot1.Render();
            UpdateMessage();
        }

        private void BtnClearSpans_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Clear(axisLines: false, scatterPlots: false);
            formsPlot1.Render();
            UpdateMessage();
        }
    }
}
