using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // generate some data
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

            // plot the data
            formsPlot1.plt.PlotScatter(dataXs, dataSin);
            formsPlot1.plt.PlotScatter(dataXs, dataCos);
            formsPlot1.plt.XLabel("experiment time (ms)");
            formsPlot1.plt.YLabel("signal (mV)");
            formsPlot1.plt.Title("ScottPlot Quickstart");
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }
    }
}
