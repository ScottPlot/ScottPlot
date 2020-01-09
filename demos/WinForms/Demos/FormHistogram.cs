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
    public partial class FormHistogram : Form
    {
        public FormHistogram()
        {
            InitializeComponent();
            GenerateData();
            PlotHistogram();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        double[] values;
        private void GenerateData()
        {
            values = ScottPlot.DataGen.RandomNormal(null, (int)nudPointCount.Value, 50, 20);
            PlotValues();
        }

        private void PlotValues()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(values.Length, 1, 1);
            formsPlot1.plt.Clear();
            formsPlot1.plt.Title("Normally Distributed Random Data");
            formsPlot1.plt.YLabel("Value (units)");
            formsPlot1.plt.XLabel("Point Number");
            formsPlot1.plt.PlotScatter(xs, values, lineWidth: 0);
            formsPlot1.Render();
        }

        private void PlotHistogram()
        {
            double? min = (double)nudMin.Value;
            double? max = (double)nudMax.Value;

            if (cbMinAuto.Checked) min = null;
            if (cbMaxAuto.Checked) max = null;

            double? binSize = (double)nudBinSize.Value;
            double? binCount = (double)nudBinCount.Value;

            if (cbBinSizeAuto.Checked) binSize = null;
            if (cbBinCountAuto.Checked) binCount = null;

            // ignore binCount if both binSize and binCount are given
            if ((binSize != null) && (binCount != null))
                binCount = null;

            bool ignoreOutOfBounds = cbIgnoreOutOfBounds.Checked;

            var hist = new ScottPlot.Statistics.Histogram(values, min, max, binSize, binCount, ignoreOutOfBounds);

            binSize = hist.bins[1] - hist.bins[0];
            if (nudMin.Enabled == false)
                nudMin.Value = (decimal)hist.bins[0];
            if (nudMax.Enabled == false)
                nudMax.Value = (decimal)(hist.bins[hist.bins.Length - 1] + binSize);
            if (nudBinSize.Enabled == false)
                nudBinSize.Value = (decimal)binSize;
            if (nudBinCount.Enabled == false)
                nudBinCount.Value = (decimal)hist.bins.Length;

            lbBins.Items.Clear();
            foreach (double bin in hist.bins)
                lbBins.Items.Add(bin.ToString());

            if (cbCount.Checked)
                PlotHistogramCount(hist.bins, hist.counts);
            else if (cbNorm.Checked)
                PlotHistogramFrac(hist.bins, hist.countsFrac);
            else if (cbCph.Checked)
                PlotHistogramCumulative(hist.bins, hist.cumulativeFrac);
        }

        private void PlotHistogramCount(double[] bins, double[] counts)
        {
            formsPlot2.plt.Clear();
            formsPlot2.plt.Title("Histogram");
            formsPlot2.plt.YLabel("Count (#)");
            formsPlot2.plt.XLabel("Value (units)");
            if (rbGraphBar.Checked)
                formsPlot2.plt.PlotBar(bins, counts, barWidth: bins[1] - bins[0]);
            else
                formsPlot2.plt.PlotStep(bins, counts);
            formsPlot2.plt.PlotHLine(0, Color.Black);
            formsPlot2.Render();
        }

        private void PlotHistogramFrac(double[] bins, double[] fracs)
        {
            formsPlot2.plt.Clear();
            formsPlot2.plt.Title("Probability Histogram");
            formsPlot2.plt.YLabel("Probability (fraction)");
            formsPlot2.plt.XLabel("Value (units)");
            if (rbGraphBar.Checked)
                formsPlot2.plt.PlotBar(bins, fracs);
            else
                formsPlot2.plt.PlotStep(bins, fracs);
            formsPlot2.plt.PlotHLine(0, Color.Black);
            formsPlot2.Render();
        }

        private void PlotHistogramCumulative(double[] bins, double[] cumFracs)
        {
            formsPlot2.plt.Clear();
            formsPlot2.plt.Title("Cumulative Probability Histogram");
            formsPlot2.plt.YLabel("Probability (fraction)");
            formsPlot2.plt.XLabel("Value (units)");
            if (rbGraphBar.Checked)
                formsPlot2.plt.PlotBar(bins, cumFracs);
            else
                formsPlot2.plt.PlotScatter(bins, cumFracs);
            formsPlot2.plt.PlotHLine(0, Color.Black);
            formsPlot2.Render();
        }

        private void BtnGenerateData_Click(object sender, EventArgs e)
        {
            GenerateData();
            PlotHistogram();
        }

        private void RbGraphBar_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }

        private void RbGraphStep_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }

        private void CbCount_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }

        private void CbNorm_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }

        private void CbCph_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }

        private void CbMinAuto_CheckedChanged(object sender, EventArgs e)
        {
            nudMin.Enabled = !cbMinAuto.Checked;
            PlotHistogram();
        }

        private void CbMaxAuto_CheckedChanged(object sender, EventArgs e)
        {
            nudMax.Enabled = !cbMaxAuto.Checked;
            PlotHistogram();
        }

        private void CbBinSizeAuto_CheckedChanged(object sender, EventArgs e)
        {
            nudBinSize.Enabled = !cbBinSizeAuto.Checked;
            PlotHistogram();
        }

        private void CbBinCountAuto_CheckedChanged(object sender, EventArgs e)
        {
            nudBinCount.Enabled = !cbBinCountAuto.Checked;
            PlotHistogram();
        }

        private void CbIgnoreOutOfBounds_CheckedChanged(object sender, EventArgs e)
        {
            PlotHistogram();
        }
    }
}
