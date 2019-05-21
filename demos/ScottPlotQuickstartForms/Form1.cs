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
            int pointCount = 101;
            double pointSpacing = .01;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount, pointSpacing);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            scottPlotUC1.plt.PlotScatter(dataXs, dataSin);
            scottPlotUC1.plt.PlotScatter(dataXs, dataCos);
            scottPlotUC1.plt.XLabel("experiment time (ms)");
            scottPlotUC1.plt.YLabel("signal (mV)");
            scottPlotUC1.plt.Title("Groundbreaking Data");
            scottPlotUC1.plt.AxisAuto(0);
            scottPlotUC1.Render();
        }
    }
}
