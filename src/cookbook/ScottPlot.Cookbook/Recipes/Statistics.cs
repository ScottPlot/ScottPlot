using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class StatsGaussian : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_histogram";
        public string Title => "Histogram";
        public string Description => "The Histogram class makes it easy to get binned population information.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            double[] values = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            var hist = new ScottPlot.Statistics.Histogram(values, min: 0, max: 100);

            // plot the bins as a bar graph (on the primary Y axis)
            var bar = plt.AddBar(hist.counts, hist.bins);
            bar.BarWidth = hist.binSize * 1.2; // oversize to reduce render artifacts
            bar.BorderLineWidth = 0;
            bar.YAxisIndex = 0;
            plt.YAxis.Label("Count (#)");
            plt.YAxis.Color(bar.FillColor);

            // plot the mean curve as a scatter plot (on the secondary Y axis)
            var sp = plt.AddScatter(hist.bins, hist.countsFracCurve);
            sp.MarkerSize = 0;
            sp.LineWidth = 2;
            sp.YAxisIndex = 1;
            plt.YAxis2.Label("Fraction");
            plt.YAxis2.Color(sp.Color);
            plt.YAxis2.Ticks(true);

            // decorate the plot
            plt.XAxis2.Label("Normal Random Data", bold: true);
            plt.XAxis.Label("Value (units)");
            plt.SetAxisLimits(yMin: 0);
            plt.Grid(lineStyle: LineStyle.Dot);
        }
    }

    public class StatsCPH : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_cph";
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
            sp1.Label = "Sample A";
            sp1.StepDisplay = true;
            sp1.MarkerSize = 0;

            var sp2 = plt.AddScatter(hist2.bins, hist2.cumulativeFrac);
            sp2.Label = "Sample B";
            sp2.StepDisplay = true;
            sp2.MarkerSize = 0;

            // decorate the plot
            plt.Legend();
            plt.SetAxisLimits(yMin: 0, yMax: 1);
            plt.Grid(lineStyle: LineStyle.Dot);
            plt.Title("Cumulative Probability Histogram");
            plt.XAxis.Label("Probability (fraction)");
            plt.YAxis.Label("Value (units)");
        }
    }

    public class StatsLinearRegression : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_linearRegression";
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
            plt.Title("Linear Regression\n" +
                $"Y = {model.slope:0.0000}x + {model.offset:0.0} " +
                $"(R² = {model.rSquared:0.0000})");
            plt.AddScatter(xs, ys, lineWidth: 0);
            plt.AddLine(model.slope, model.offset, (x1, x2), lineWidth: 2);
        }
    }

    public class StatsOrderStatistics : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_orderStatistics";
        public string Title => "Nth Order Statistics";
        public string Description =>
            "The Nth order statistic of a set is the Nth smallest value of the set (indexed from 1).";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 500;
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.Random(rand, pointCount);

            int n = 200;
            double nthValue = Statistics.Common.NthOrderStatistic(ys, n);

            plt.Title($"{n}th Smallest Value (of {pointCount})");
            plt.AddScatter(xs, ys, lineWidth: 0, markerShape: MarkerShape.openCircle);
            plt.AddHorizontalLine(nthValue, width: 3, style: LineStyle.Dash);

        }
    }

    public class StatsPercentiles : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_percentiles";
        public string Title => "Percentiles";
        public string Description =>
            "Percentiles are a good tool to analyze the distribution of your data and filter out extreme values.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 500;
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.Random(rand, pointCount);

            double tenthPercentile = Statistics.Common.Percentile(ys, 10);

            plt.Title("10th Percentile");
            plt.AddScatter(xs, ys, lineWidth: 0, markerShape: MarkerShape.openCircle);
            plt.AddHorizontalLine(tenthPercentile, width: 3, style: LineStyle.Dash);
        }
    }

    public class StatsQuantiles : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_quantiles";
        public string Title => "Quantiles";
        public string Description =>
            "A q-Quantile is a generalization of quartiles and percentiles to any number of buckets.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 500;
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.Random(rand, pointCount);

            // A septile is a 7-quantile
            double secondSeptile = Statistics.Common.Quantile(ys, 2, 7);

            plt.Title("Second Septile");
            plt.AddScatter(xs, ys, lineWidth: 0, markerShape: MarkerShape.openCircle);
            plt.AddHorizontalLine(secondSeptile, width: 3, style: LineStyle.Dash);
        }
    }
}
