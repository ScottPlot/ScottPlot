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
    public partial class FormQuickstart : Form
    {
        public FormQuickstart()
        {
            InitializeComponent();
        }

        private void FormQuickstart_Load(object sender, EventArgs e)
        {
            QuickstartNonInteractive();
            QuickstartInteractive();
        }

        // demonstrate how to use the ScottPlot directly for non-interactive plotting
        public void QuickstartNonInteractive()
        {
            // create some data to plot
            int pointCount = 100;
            double[] dataXs = new double[pointCount];
            double[] dataSin = new double[pointCount];
            double[] dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
            }

            // plot the data
            ScottPlot.Plot plt = new ScottPlot.Plot(600, 400);
            plt.data.AddScatter(dataXs, dataSin);
            plt.data.AddScatter(dataXs, dataCos);
            plt.settings.AxisFit();
            plt.settings.title = "ScottPlot Quickstart";
            plt.figure.Save("quickstart2.png");
        }

        // demonstrate how to use the ScottPlotUC for interactive plotting
        public void QuickstartInteractive()
        {
            // create some data to plot
            int pointCount = 100;
            double[] dataXs = new double[pointCount];
            double[] dataSin = new double[pointCount];
            double[] dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
            }

            // plot the data
            scottPlotUC1.plt.data.AddScatter(dataXs, dataSin);
            scottPlotUC1.plt.data.AddScatter(dataXs, dataCos);
            scottPlotUC1.plt.settings.AxisFit();
            scottPlotUC1.plt.settings.title = "ScottPlot Quickstart";
            scottPlotUC1.Render();
        }
    }
}
