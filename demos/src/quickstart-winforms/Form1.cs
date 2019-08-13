using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotQuickstartForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] dataXs = new double[pointCount];
            double[] dataSin = new double[pointCount];
            double[] dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
            }

            interactivePlot1.plt.PlotScatter(dataXs, dataSin);
            interactivePlot1.plt.PlotScatter(dataXs, dataCos);
            interactivePlot1.plt.XLabel("experiment time (ms)");
            interactivePlot1.plt.YLabel("signal (mV)");
            interactivePlot1.plt.Title("ScottPlot Quickstart");
            interactivePlot1.plt.AxisAuto();
            interactivePlot1.Render();
        }
    }
}
