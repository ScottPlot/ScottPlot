using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoCandlestick
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Button1_Click(null, null);
        }

        private void GenerateNewData()
        {
            Random rand = new Random();
            int pointCount = 100;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] volume = ScottPlot.DataGen.RandomNormal(rand, pointCount, 100_000, 20_000);
            double[] priceMean = ScottPlot.DataGen.RandomWalk(rand, pointCount, 20, 75);
            double[] priceVar = ScottPlot.DataGen.RandomNormal(rand, pointCount, 5, 2);

            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.YLabel("Share Price", fontSize: 10);
            scottPlotUC1.plt.Title("ScottPlot Candlestick Demo");
            scottPlotUC1.plt.PlotScatter(xs, priceMean, lineWidth: 0, errorY: priceVar, errorCapSize: 0);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();

            scottPlotUC2.plt.Clear();
            scottPlotUC2.plt.YLabel("Volume", fontSize: 10);
            scottPlotUC2.plt.PlotBar(xs, volume, barWidth: .5);
            scottPlotUC2.plt.AxisAuto(.01, .1);
            scottPlotUC2.plt.Axis(null, null, 0, null);
            scottPlotUC2.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GenerateNewData();
        }
    }
}
