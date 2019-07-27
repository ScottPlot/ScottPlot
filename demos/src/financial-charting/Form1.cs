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
            ScottPlot.OHLC[] ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, pointCount);
            double[] timestamps = ScottPlot.DataGen.Consecutive(pointCount);
            double[] volumes = ScottPlot.DataGen.Random(rand, pointCount, 500, 1000);

            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.YLabel("Share Price", fontSize: 10);
            scottPlotUC1.plt.Title("ScottPlot Candlestick Demo");
            if (rbCandle.Checked)
                scottPlotUC1.plt.PlotCandlestick(ohlcs);
            else
                scottPlotUC1.plt.PlotOHLC(ohlcs);
            scottPlotUC1.plt.AxisAuto();

            scottPlotUC2.plt.Clear();
            scottPlotUC2.plt.YLabel("Volume", fontSize: 10);
            scottPlotUC2.plt.PlotBar(timestamps, volumes, barWidth: .5);
            scottPlotUC2.plt.AxisAuto(.01, .1);
            scottPlotUC2.plt.Axis(null, null, 0, null);

            scottPlotUC1.plt.MatchPadding(scottPlotUC2.plt, horizontal: true, vertical: false);
            scottPlotUC1.Render();
            scottPlotUC2.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void RbCandle_CheckedChanged(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void RbOHLC_CheckedChanged(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void ScottPlotUC1_MouseDragged(object sender, EventArgs e)
        {
            scottPlotUC2.plt.MatchAxis(scottPlotUC1.plt, horizontal: true, vertical: false);
            scottPlotUC2.Render();
        }

        private void ScottPlotUC2_MouseDragged(object sender, EventArgs e)
        {
            scottPlotUC1.plt.MatchAxis(scottPlotUC2.plt, horizontal: true, vertical: false);
            scottPlotUC1.Render();
        }
    }
}
