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
            double[] dataXs = ScottPlot.DataGen.Range(0, 1, 1.0 / pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            // plot the data
            formsPlot1.plt.PlotScatter(dataXs, dataSin);
            formsPlot1.plt.PlotScatter(dataXs, dataCos);
            formsPlot1.plt.XLabel("Experiment Time (ms)");
            formsPlot1.plt.YLabel("Measurement (mV)");
            formsPlot1.plt.Title("ScottPlot .NET Core 3 Winforms Quickstart");
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }
    }
}
