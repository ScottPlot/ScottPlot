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
    public partial class FormBarGraph : Form
    {
        public FormBarGraph()
        {
            InitializeComponent();
            formsPlot1.plt.Title("Demo Bar Graph");
            formsPlot1.plt.Grid(false);
            formsPlot1.plt.PlotHLine(0, color: Color.Black);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnAdd_Click(null, null);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int pointCount = 25;
            Random rand = new Random();
            double[] dataY = new double[pointCount];
            double[] errorY = new double[pointCount];
            double[] dataX = new double[pointCount];
            for (int i=0; i<pointCount; i++)
            {
                double frac = (double)i / pointCount;
                dataX[i] = i * 10;
                dataY[i] = Math.Sin(frac * Math.PI) * 10 + rand.NextDouble() * 5 - 5;
                errorY[i] = rand.NextDouble() * 2;
            }
            string label = $"Bar {formsPlot1.plt.GetPlottables().Count}";
            formsPlot1.plt.PlotBar(dataX, dataY, errorY: errorY, label: label);
            formsPlot1.plt.Legend();
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Clear(axisLines: false);
            formsPlot1.Render();
        }
    }
}
