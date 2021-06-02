using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class HistogramCount : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_histogram";
        public string Title => "Histogram";
        public string Description =>
            "The Histogram class makes it easy to get binned population information.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate sample heights are based on https://ourworldindata.org/human-height
            Random rand = new(0);
            double[] heights = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1234, mean: 178.4, stdDev: 7.6);

            // create a histogram
            var hist = new ScottPlot.Statistics.Histogram(heights, min: 140, max: 220, binSize: 1);

            // display the histogram counts as a bar plot
            var bar = plt.AddBar(hist.counts, hist.bins);
            bar.BarWidth = hist.binSize;

            // customize the plot style
            plt.YAxis.Label("Count (#)");
            plt.XAxis.Label("Height (cm)");
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class HistogramProbability : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_histogramProbability";
        public string Title => "Histogram Probability";
        public string Description =>
            "Binned probability can be displayed instead of raw counts. " +
            "The probability density is also available for every bin.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate sample heights are based on https://ourworldindata.org/human-height
            Random rand = new(0);
            double[] heights = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1234, mean: 178.4, stdDev: 7.6);

            // create a histogram
            var hist = new ScottPlot.Statistics.Histogram(heights, min: 140, max: 220, binSize: 1);

            // display histogram probabability as a bar plot
            var bar = plt.AddBar(values: hist.countsFrac, positions: hist.bins);
            bar.FillColor = ColorTranslator.FromHtml("#9bc3eb");
            bar.BorderColor = ColorTranslator.FromHtml("#82add9");
            bar.BarWidth = hist.binSize;

            // display histogram distribution curve as a line plot
            plt.AddScatterLines(
                xs: hist.bins,
                ys: hist.probability,
                color: Color.Black,
                lineWidth: 2,
                lineStyle: LineStyle.Dash);

            // customize the plot style
            plt.Title("Adult Male Height");
            plt.YAxis.Label("Probability");
            plt.XAxis.Label("Height (cm)");
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class TwoHistograms : IRecipe
    {
        public string Category => "Statistics";
        public string ID => "stats_histogram2";
        public string Title => "Multiple Histograms";
        public string Description => "This example demonstrates two histograms on the same plot. " +
            "Note the use of fractional units on the vertical axis, allowing easy comparison of datasets " +
            "with different numbers of points. Unlike the previous example, this one does not use multiple axes.";

        public void ExecuteRecipe(Plot plt)
        {
            // male and female heights are based on https://ourworldindata.org/human-height
            Random rand = new(0);
            double[] heightsMale = ScottPlot.DataGen.RandomNormal(rand, pointCount: 2345, mean: 178.4, stdDev: 7.6);
            double[] heightsFemale = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1234, mean: 164.7, stdDev: 7.1);

            // calculate histograms for male and female datasets
            var histMale = new ScottPlot.Statistics.Histogram(heightsMale, min: 140, max: 210, binSize: 1);
            var histFemale = new ScottPlot.Statistics.Histogram(heightsFemale, min: 140, max: 210, binSize: 1);

            // plot histograms
            var barMale = plt.AddBar(values: histMale.countsFrac, positions: histMale.bins);
            barMale.BarWidth = histMale.binSize;
            barMale.FillColor = Color.FromArgb(50, Color.Blue);
            barMale.BorderLineWidth = 0;

            var barFemale = plt.AddBar(values: histFemale.countsFrac, positions: histFemale.bins);
            barFemale.BarWidth = histFemale.binSize;
            barFemale.FillColor = Color.FromArgb(50, Color.Red);
            barFemale.BorderLineWidth = 0;

            // plot probability function curves
            plt.AddScatterLines(
                xs: histMale.bins,
                ys: histMale.probability,
                color: Color.FromArgb(150, Color.Blue),
                lineWidth: 3,
                label: $"Male (n={heightsMale.Length:N0})");

            plt.AddScatterLines(
                xs: histFemale.bins,
                ys: histFemale.probability,
                color: Color.FromArgb(150, Color.Red),
                lineWidth: 3,
                label: $"Female (n={heightsFemale.Length:N0})");

            // customize styling
            plt.Title("Human Height by Sex");
            plt.YLabel("Probability");
            plt.XLabel("Height (cm)");
            plt.Legend(location: ScottPlot.Alignment.UpperLeft);
            plt.SetAxisLimits(yMin: 0);
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
