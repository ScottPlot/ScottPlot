﻿namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Histograms : ICategory
{
    public string Chapter => "Statistics";
    public string CategoryName => "Histogram";
    public string CategoryDescription => "Histograms graphically represent the distribution of numerical data " +
        "by grouping values into ranges (bins) and displaying the frequency or count of points in each bin.";

    public class HistogramQuickstart : RecipeBase
    {
        public override string Name => "Histogram Quickstart";
        public override string Description => "A histogram can be created from a collection of values.";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[] heights = SampleData.MaleHeights();
            var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, heights);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.Counts);

            // Size each bar slightly less than the width of a bin
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize * .8;
            }

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Number of People");
            myPlot.XLabel("Height (cm)");
        }
    }

    public class HistogramFixedSizeBins : RecipeBase
    {
        public override string Name => "Histogram with Fixed Size Bins";
        public override string Description => "A histogram can be created from a collection of values.";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[] heights = SampleData.MaleHeights();
            var hist = ScottPlot.Statistics.Histogram.WithBinSize(2, heights);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.Counts);

            // Size each bar slightly less than the width of a bin
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize * .8;
            }

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Number of People");
            myPlot.XLabel("Height (cm)");
        }
    }

    public class HistogramFilled : RecipeBase
    {
        public override string Name => "Filled Histogram";
        public override string Description => "A filled histogram (one with no visible gaps between bars) can be achieved " +
            "by setting the bar width to the bin size. However, anti-aliasing artifacts may cause white lines to appear between bars. " +
            "Disable anti-aliasing for each bar to improve appearance of such plots.";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[] heights = SampleData.MaleHeights();
            var hist = ScottPlot.Statistics.Histogram.WithBinSize(1, heights);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.Counts);

            // Customize the style of each bar
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillStyle.AntiAlias = false;
            }

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Number of People");
            myPlot.XLabel("Height (cm)");
        }
    }

    public class HistogramProbability : RecipeBase
    {
        public override string Name => "Histogram of Probabilities";
        public override string Description => "Histograms may be displayed as the probability for each value falling inside a bin";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[] heights = SampleData.MaleHeights();
            var hist = ScottPlot.Statistics.Histogram.WithBinCount(10, heights);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.GetProbability(100));

            // Customize the style of each bar
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize * 0.8;
            }

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Probability (%)");
            myPlot.XLabel("Height (cm)");
        }
    }

    public class HistogramProbabilityCurve : RecipeBase
    {
        public override string Name => "Histogram with Probability Curve";
        public override string Description => "A probability curve may be generated for a Gaussian distributed sample.";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[] heights = SampleData.MaleHeights();
            var hist = ScottPlot.Statistics.Histogram.WithBinCount(100, heights);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.Counts);

            // Customize the style of each bar
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillStyle.AntiAlias = false;
            }

            // Add a probability curve to a secondary axis
            ScottPlot.Statistics.ProbabilityDensity pd = new(heights);
            double[] xs = Generate.Range(heights.Min(), heights.Max(), 1);
            double[] ys = pd.GetYs(xs, 100);

            var curve = myPlot.Add.ScatterLine(xs, ys);
            curve.Axes.YAxis = myPlot.Axes.Right;
            curve.LineWidth = 2;
            curve.LineColor = Colors.Black;
            curve.LinePattern = LinePattern.DenselyDashed;

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Number of People");
            myPlot.XLabel("Height (cm)");
            myPlot.Axes.Right.Label.Text = "Probability (%)";
        }
    }
}
