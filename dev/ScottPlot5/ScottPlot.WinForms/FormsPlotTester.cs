using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Text;
using System.Diagnostics;

namespace ScottPlot.WinForms
{
    public partial class FormsPlotTester : Form
    {
        public FormsPlotTester()
        {
            InitializeComponent();
            SinePlots(51);
            formsPlot1.Plot.Benchmark(true);
        }

        private void SinePlots(int pointCount)
        {
            formsPlot1.Plot.Clear();
            double[] xs = ScottPlot.Generate.Consecutive(pointCount);
            double[] ys1 = ScottPlot.Generate.Sin(pointCount);
            double[] ys2 = ScottPlot.Generate.Cos(pointCount);
            formsPlot1.Plot.AddScatter(xs, ys1);
            formsPlot1.Plot.AddScatter(xs, ys2);
            formsPlot1.Plot.Autoscale();
            formsPlot1.Redraw();
        }

        private void btnScatterBasic_Click(object sender, EventArgs e) => SinePlots(51);

        private void btnScatter100k_Click(object sender, EventArgs e) => SinePlots(10_000);

        private void cbBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.Benchmark(cbBenchmark.Checked);
            formsPlot1.Redraw();
        }
    }
}