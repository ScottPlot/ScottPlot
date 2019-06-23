using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoBarGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            scottPlotUC1.plt.Title("Demo Bar Graph");
            scottPlotUC1.plt.Grid(false);
            scottPlotUC1.plt.PlotHLine(0, color: Color.Black);
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
            string label = $"Bar {scottPlotUC1.plt.GetPlottables().Count}";
            scottPlotUC1.plt.PlotBar(dataX, dataY, errorY: errorY, label: label);
            scottPlotUC1.plt.Legend();
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Clear(axisLines: false);
            scottPlotUC1.Render();
        }
    }
}
