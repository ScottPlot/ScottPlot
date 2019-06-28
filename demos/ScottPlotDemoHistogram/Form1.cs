using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoHistogram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button1_Click(null, null);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int pointCount = (int)nudPoints.Value;
            double[] randomValues = ScottPlot.DataGen.RandomNormal(new Random(), pointCount, 50, 25);
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);

            var hist = new ScottPlot.Histogram(randomValues);

            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.Title("Original Data");
            scottPlotUC1.plt.XLabel("Sample Number");
            scottPlotUC1.plt.YLabel("Value");
            scottPlotUC1.plt.PlotScatter(xs, randomValues, lineWidth: 0);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();

            scottPlotUC2.plt.Clear();
            if (cbCount.Checked)
            {
                scottPlotUC2.plt.Title("Histogram");
                scottPlotUC2.plt.XLabel("Value");
                scottPlotUC2.plt.YLabel("Count (#)");
                scottPlotUC2.plt.PlotBar(hist.bins, hist.counts);
            }
            else if (cbNorm.Checked)
            {
                scottPlotUC2.plt.Title("Normalized Histogram");
                scottPlotUC2.plt.XLabel("Value");
                scottPlotUC2.plt.YLabel("Probability");
                scottPlotUC2.plt.PlotBar(hist.bins, hist.countsFrac);
            }
            else if (cbCumulative.Checked)
            {
                scottPlotUC2.plt.Title("Cumulative Probability Histogram");
                scottPlotUC2.plt.XLabel("Value");
                scottPlotUC2.plt.YLabel("Probability");
                scottPlotUC2.plt.PlotBar(hist.bins, hist.cumulativeFrac);
            }
            else
            {
                throw new NotImplementedException();
            }

            scottPlotUC2.plt.PlotHLine(0, color: Color.Black);
            scottPlotUC2.plt.AxisAuto();
            scottPlotUC2.Render();
        }

        private void CbNorm_CheckedChanged(object sender, EventArgs e)
        {
            Button1_Click(null, null);
        }

        private void CbCumulative_CheckedChanged(object sender, EventArgs e)
        {
            Button1_Click(null, null);
        }

        private void CbCount_CheckedChanged(object sender, EventArgs e)
        {
            Button1_Click(null, null);
        }
    }
}
