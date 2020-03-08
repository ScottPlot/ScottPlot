using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Examples
{
    class Stats
    {
        public class Histogram : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Histogram";
            public string description { get; } = "This example demonstrates how to plot the histogram of a dataset.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] values = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
                var hist = new Statistics.Histogram(values, min: 0, max: 100);

                plt.PlotBar(hist.bins, hist.countsFrac);
                plt.PlotScatter(hist.bins, hist.countsFracCurve, markerSize: 0, lineWidth: 2, color: Color.Black);
                plt.Title("Normal Random Data");
                plt.YLabel("Frequency (fraction)");
                plt.XLabel("Value (units)");
                plt.Axis(null, null, 0, null);
            }
        }

        public class CPH : PlotDemo, IPlotDemo
        {
            public string name { get; } = "CPH";
            public string description { get; } = "This example demonstrates how to plot a cumulative probability histogram (CPH) to compare the distribution of two datasets.";

            public void Render(Plot plt)
            {
                // create sample data for two datasets
                Random rand = new Random(0);
                double[] values1 = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
                double[] values2 = DataGen.RandomNormal(rand, pointCount: 1000, mean: 45, stdDev: 25);
                var hist1 = new ScottPlot.Statistics.Histogram(values1, min: 0, max: 100);
                var hist2 = new ScottPlot.Statistics.Histogram(values2, min: 0, max: 100);

                // display datasets as step plots
                plt.Title("Cumulative Probability Histogram");
                plt.YLabel("Probability (fraction)");
                plt.XLabel("Value (units)");
                plt.PlotStep(hist1.bins, hist1.cumulativeFrac, lineWidth: 1.5, label: "sample A");
                plt.PlotStep(hist2.bins, hist2.cumulativeFrac, lineWidth: 1.5, label: "sample B");
                plt.Legend();
                plt.Axis(null, null, 0, 1);
            }
        }

        public class LinReg : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Linear Regression";
            public string description { get; } = "This example shows how to create a linear regression line for X/Y data.";

            public void Render(Plot plt)
            {
                // Create some linear but noisy data
                Random rand = new Random(0);
                double[] ys = ScottPlot.DataGen.NoisyLinear(rand, pointCount: 100, noise: 30);
                double[] xs = ScottPlot.DataGen.Consecutive(ys.Length);
                double x1 = xs[0];
                double x2 = xs[xs.Length - 1];

                // use the linear regression fitter to fit these data
                var model = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);
                double y1 = model.GetValueAt(x1);
                double y2 = model.GetValueAt(x2);

                // plot the original data and add the regression line
                plt.PlotScatter(xs, ys, lineWidth: 0, label: "original data");
                plt.PlotLine(x1, y1, x2, y2, lineWidth: 3, label: "linear regression");
                plt.Legend();
                plt.Title(model.ToString());
            }
        }
    }
}
