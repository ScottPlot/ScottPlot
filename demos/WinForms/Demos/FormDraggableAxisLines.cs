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

            // plot markers specific X positions
            formsPlot1.plt.PlotVLine(x: 15, draggable: true, dragLimitLower: 0, dragLimitUpper: 49);
            formsPlot1.plt.PlotHSpan(x1: 20, x2: 25, draggable: true, dragLimitLower: 0, dragLimitUpper: 49);

            // plot markers at specific Y positions
            formsPlot1.plt.PlotHLine(y: -.75, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);
            formsPlot1.plt.PlotVSpan(y1: .25, y2: .5, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);

            formsPlot1.Render();
            UpdateMessage();
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

                if (plottables[i] is ScottPlot.PlottableVLine vLine)
                {
                    msg += $"{i}: VLine ({vLine.position:F4})\r\n";
                }
                else if (plottables[i] is ScottPlot.PlottableHLine hLine)
                {
                    msg += $"{i}: HLine ({hLine.position:F4})\r\n";
                }
                else if (plottables[i] is ScottPlot.PlottableVSpan vSpan)
                {
                    msg += $"{i}: VSpan ({vSpan.position1:F4} to {vSpan.position2:F4})\r\n";
                }
                else if (plottables[i] is ScottPlot.PlottableHSpan hSpan)
                {
                    msg += $"{i}: HSpan ({hSpan.position1:F4} to {hSpan.position2:F4})\r\n";
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
