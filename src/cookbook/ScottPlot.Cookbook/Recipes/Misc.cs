using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    class MiscGaussian : IRecipe
    {
        public string Category => "Misc";
        public string ID => "misc_histogram";
        public string Title => "Histogram";
        public string Description => "The Histogram class makes it easy to get binned population information.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            double[] values = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            var hist = new ScottPlot.Statistics.Histogram(values, min: 0, max: 100);

            // plot the bins as a bar graph (on the primary Y axis)
            var bar = plt.AddBar(hist.counts, hist.bins);
            bar.barWidth = hist.binSize * 1.2; // oversize to reduce render artifacts
            bar.borderLineWidth = 0;
            bar.VerticalAxisIndex = 0;
            plt.YAxis.Label = "Count (#)";
            plt.YAxis.Configure(color: bar.fillColor, ticks: true);

            // plot the mean curve as a scatter plot (on the secondary Y axis)
            var sp = plt.AddScatter(hist.bins, hist.countsFracCurve);
            sp.markerSize = 0;
            sp.lineWidth = 2;
            sp.VerticalAxisIndex = 1;
            plt.YAxis2.Label = "Fraction";
            plt.YAxis2.Configure(color: sp.color, ticks: true);

            // decorate the plot
            plt.XAxis2.Label = "Normal Random Data";
            plt.XAxis.Label = "Value (units)";
            plt.SetAxisLimits(yMin: 0);
            plt.Grid(lineStyle: LineStyle.Dot);
        }
    }

    class MiscCPH : IRecipe
    {
        public string Category => "Misc";
        public string ID => "misc_cph";
        public string Title => "CPH";
        public string Description =>
            "This example demonstrates how to plot a cumulative probability " +
            "histogram (CPH) to compare the distribution of two datasets.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data for two datasets
            Random rand = new Random(0);
            double[] values1 = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            double[] values2 = DataGen.RandomNormal(rand, pointCount: 1000, mean: 45, stdDev: 25);
            var hist1 = new ScottPlot.Statistics.Histogram(values1, min: 0, max: 100);
            var hist2 = new ScottPlot.Statistics.Histogram(values2, min: 0, max: 100);

            // display datasets as step plots
            var sp1 = plt.AddScatter(hist1.bins, hist1.cumulativeFrac);
            sp1.label = "Sample A";
            sp1.stepDisplay = true;
            sp1.markerSize = 0;

            var sp2 = plt.AddScatter(hist2.bins, hist2.cumulativeFrac);
            sp2.label = "Sample B";
            sp2.stepDisplay = true;
            sp2.markerSize = 0;

            // decorate the plot
            plt.Legend();
            plt.SetAxisLimits(yMin: 0, yMax: 1);
            plt.Grid(lineStyle: LineStyle.Dot);
            plt.Title("Cumulative Probability Histogram");
            plt.YLabel("Probability (fraction)");
            plt.XLabel("Value (units)");
        }
    }


    class MiscLinearRegression : IRecipe
    {
        public string Category => "Misc";
        public string ID => "misc_linearRegression";
        public string Title => "Linear Regression";
        public string Description =>
            "A regression module is available to simplify the act " +
            "of creating a linear regression line fitted to the data.";

        public void ExecuteRecipe(Plot plt)
        {
            // Create some linear but noisy data
            double[] ys = DataGen.NoisyLinear(null, pointCount: 100, noise: 30);
            double[] xs = DataGen.Consecutive(ys.Length);
            double x1 = xs[0];
            double x2 = xs[xs.Length - 1];

            // use the linear regression fitter to fit these data
            var model = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);

            // plot the original data and add the regression line
            plt.Title($"Y = {model.slope:0.0000}x + {model.offset:0.0}\nR² = {model.rSquared:0.0000}");
            plt.AddScatter(xs, ys, lineWidth: 0);
            plt.AddLine(model.slope, model.offset, (x1, x2), lineWidth: 2);
        }
    }


    class MiscInterpolation : IRecipe
    {
        public string Category => "Misc";
        public string ID => "misc_interpolation";
        public string Title => "Spline Interpolation";
        public string Description =>
            "Interpolated splines create curves with many X/Y points to smoothly connect a limited number of input points.";

        public void ExecuteRecipe(Plot plt)
        {
            // create a small number of X/Y data points and display them
            double[] xs = { 0, 10, 20, 30 };
            double[] ys = { 65, 25, 55, 80 };
            plt.AddScatter(xs, ys, Color.Black, markerSize: 10, lineWidth: 0, label: "Original Data");

            // Calculate the interpolated splines using three different methods:
            //   Natural splines are "stiffer" than a polynomial interpolations and are less likely to oscillate.
            //   Periodic splines are natural splines whose first and last point slopes are matched.
            //   End slope splines let you define first and last data point slopes (defaults to zero).
            var nsi = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, ys, resolution: 20);
            var psi = new ScottPlot.Statistics.Interpolation.PeriodicSpline(xs, ys, resolution: 20);
            var esi = new ScottPlot.Statistics.Interpolation.EndSlopeSpline(xs, ys, resolution: 20);

            // plot the interpolated Xs and Ys
            plt.AddScatter(nsi.interpolatedXs, nsi.interpolatedYs, Color.Red, markerSize: 3, label: "Natural Spline");
            plt.AddScatter(psi.interpolatedXs, psi.interpolatedYs, Color.Green, markerSize: 3, label: "Periodic Spline");
            plt.AddScatter(esi.interpolatedXs, esi.interpolatedYs, Color.Blue, markerSize: 3, label: "End Slope Spline");

            plt.Legend();
        }
    }
}
