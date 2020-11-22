using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Advanced
{
    class Statistics
    {
        public class Histogram : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Histogram";
            public string description { get; } = "This example demonstrates how to plot the histogram of a dataset.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] values = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
                var hist = new ScottPlot.Statistics.Histogram(values, min: 0, max: 100);

                double barWidth = hist.binSize * 1.2; // slightly over-side to reduce anti-alias rendering artifacts

                plt.PlotBar(hist.bins, hist.countsFrac, barWidth: barWidth, outlineWidth: 0);
                plt.PlotScatter(hist.bins, hist.countsFracCurve, markerSize: 0, lineWidth: 2, color: Color.Black);
                plt.Title("Normal Random Data");
                plt.YLabel("Frequency (fraction)");
                plt.XLabel("Value (units)");
                plt.Axis(null, null, 0, null);
                plt.Grid(lineStyle: LineStyle.Dot);
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
                plt.Grid(lineStyle: LineStyle.Dot);
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

                // plot the original data and add the regression line
                plt.Title($"Y = {model.slope:0.0000}x + {model.offset:0.0}\nR² = {model.rSquared:0.0000}");
                plt.PlotScatter(xs, ys, lineWidth: 0);
                plt.PlotLine(model.slope, model.offset, (x1, x2), lineWidth: 2);
            }
        }

        public class Population : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Population Statistics";
            public string description { get; } = "The Population class makes it easy to work with population statistics. Instantiate the Population class with a double array of values, then access its properties and methods as desired.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores
                Random rand = new Random(0);
                double[] scores = DataGen.RandomNormal(rand, 250, 85, 5);

                // create a Population object from the data
                var pop = new ScottPlot.Statistics.Population(scores);

                // display the original values scattered vertically
                double[] ys = DataGen.RandomNormal(rand, pop.values.Length, stdDev: .15);
                plt.PlotScatter(pop.values, ys, markerSize: 10,
                    markerShape: MarkerShape.openCircle, lineWidth: 0);

                // display the bell curve for this distribution
                double[] curveXs = DataGen.Range(pop.minus3stDev, pop.plus3stDev, .1);
                double[] curveYs = pop.GetDistribution(curveXs, normalize: false);
                plt.PlotScatter(curveXs, curveYs, markerSize: 0, lineWidth: 2);

                // improve the style of the plot
                plt.Title($"Test Scores (mean: {pop.mean:0.00} +/- {pop.stDev:0.00}, n={pop.n})");
                plt.XLabel("Score");
                plt.Grid(lineStyle: LineStyle.Dot);
            }
        }

        public class SplineInterpolation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Spline Interpolation";
            public string description { get; } = "Interpolated splines create curves with many X/Y points to smoothly connect a limited number of input points.";

            public void Render(Plot plt)
            {
                // create a small number of X/Y data points and display them
                double[] xs = { 0, 10, 20, 30 };
                double[] ys = { 65, 25, 55, 80 };
                plt.PlotScatter(xs, ys, Color.Black, markerSize: 10, lineWidth: 0, label: "Original Data");

                // Calculate the interpolated splines using three different methods:
                //   Natural splines are "stiffer" than a polynomial interpolations and are less likely to oscillate.
                //   Periodic splines are natural splines whose first and last point slopes are matched.
                //   End slope splines let you define first and last data point slopes (defaults to zero).
                var nsi = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, ys, resolution: 20);
                var psi = new ScottPlot.Statistics.Interpolation.PeriodicSpline(xs, ys, resolution: 20);
                var esi = new ScottPlot.Statistics.Interpolation.EndSlopeSpline(xs, ys, resolution: 20);

                // plot the interpolated Xs and Ys
                plt.PlotScatter(nsi.interpolatedXs, nsi.interpolatedYs, Color.Red, markerSize: 3, label: "Natural Spline");
                plt.PlotScatter(psi.interpolatedXs, psi.interpolatedYs, Color.Green, markerSize: 3, label: "Periodic Spline");
                plt.PlotScatter(esi.interpolatedXs, esi.interpolatedYs, Color.Blue, markerSize: 3, label: "End Slope Spline");

                plt.Legend();
            }
        }
    }
}
