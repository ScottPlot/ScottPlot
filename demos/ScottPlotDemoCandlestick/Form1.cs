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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GenerateNewData()
        {
            Random rand = new Random();

            int pointCount = 100;
            ScottPlot.OHLC[] ohlcs = new ScottPlot.OHLC[pointCount];
            double[] timestamps = new double[pointCount];
            double[] volumes = new double[pointCount];
            for (int i=0; i< ohlcs.Length; i++)
            {
                double open = rand.NextDouble() * 10 + 50;
                double close = rand.NextDouble() * 10 + 50;
                double high = Math.Max(open, close) + rand.NextDouble() * 10;
                double low = Math.Min(open, close) - rand.NextDouble() * 10;
                ohlcs[i] = new ScottPlot.OHLC(open, high, low, close, i);
                timestamps[i] = i;
                volumes[i] = 1000 + rand.NextDouble() * 500;
            }

            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.YLabel("Share Price", fontSize: 10);
            scottPlotUC1.plt.Title("ScottPlot Candlestick Demo");
            scottPlotUC1.plt.PlotOHLC(ohlcs);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();

            scottPlotUC2.plt.Clear();
            scottPlotUC2.plt.YLabel("Volume", fontSize: 10);
            scottPlotUC2.plt.PlotBar(timestamps, volumes, barWidth: .5);
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
