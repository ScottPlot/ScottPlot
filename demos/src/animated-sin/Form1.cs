using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotAnimatedSin
{
    public partial class Form1 : Form
    {
        private double[] dataXs;
        private double[] dataSin;
        private double[] dataCos;

        public Form1()
        {
            InitializeComponent();

            int pointCount = 100;
            dataXs = new double[pointCount];
            dataSin = new double[pointCount];
            dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                dataXs[i] = i;

            scottPlotUC1.plt.PlotScatter(dataXs, dataSin);
            scottPlotUC1.plt.PlotScatter(dataXs, dataCos);
            scottPlotUC1.plt.Title("ScottPlot Animated Sin Demo");
            scottPlotUC1.plt.YLabel("signal");
            scottPlotUC1.plt.XLabel("phase");
            scottPlotUC1.plt.AxisAuto(0);

        }

        private void UpdateDataAndPlot()
        {
            double offset = (double)DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            for (int i = 0; i < dataSin.Length; i++)
            {
                dataSin[i] = Math.Sin(i * 5 * Math.PI / dataSin.Length + offset);
                dataCos[i] = Math.Cos(i * 3 * Math.PI / dataSin.Length + offset) * .5;
            }
                
            scottPlotUC1.Render();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateDataAndPlot();
        }
    }
}
