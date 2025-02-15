namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class Histograms : ICategory
{
    public Chapter Chapter => Chapter.General;
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
        public override string Description => "A histogram can be created using manually defined bin sizes.";

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

    public class HistogramBars : RecipeBase
    {
        public override string Name => "Histogram Bars";
        public override string Description => "A helper method and plot type has been created " +
            "to simplify creating a bar plot that displays histogram counts. " +
            "Note that updates the histogram may appear in real time and the plot will " +
            "automatically update to display the latest data.";

        [Test]
        public override void Execute()
        {
            // create an empty histogram and display it as a bar plot
            var hist = ScottPlot.Statistics.Histogram.WithBinCount(count: 20, minValue: 140, maxValue: 220);
            var histPlot = myPlot.Add.Histogram(hist);
            histPlot.BarWidthFraction = 0.8;

            // histogram counts are updated automatically as new data is added
            double[] newData = SampleData.MaleHeights();
            hist.AddRange(newData);
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
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.GetProbability());

            // Customize the style of each bar
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillStyle.AntiAlias = false;
                bar.FillColor = Colors.C0.Lighten(.3);
            }

            // Plot the probability curve on top the histogram
            ScottPlot.Statistics.ProbabilityDensity pd = new(heights);
            double[] xs = Generate.Range(heights.Min(), heights.Max(), 1);
            double sumBins = hist.Bins.Select(x => pd.GetY(x)).Sum();
            double[] ys = pd.GetYs(xs, 1.0 / sumBins);

            var curve = myPlot.Add.ScatterLine(xs, ys);
            curve.LineWidth = 2;
            curve.LineColor = Colors.Black;
            curve.LinePattern = LinePattern.DenselyDashed;

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Probability (%)");
            myPlot.XLabel("Height (cm)");
        }
    }

    public class HistogramProbabilityCurveSecondAxis : RecipeBase
    {
        public override string Name => "Histogram with Second Axis Probability";
        public override string Description => "A probability curve may be placed on a secondary axis to allow counts " +
            "to be displayed alongside probabilities with percent units";

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

    public class HistogramMultiple : RecipeBase
    {
        public override string Name => "Multiple Histograms";
        public override string Description => "Demonstrates how to use semitransparent bars " +
            "to display histograms from overlapping datasets";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[][] heightsByGroup = { SampleData.MaleHeights(), SampleData.FemaleHeights() };
            string[] groupNames = { "Male", "Female" };
            Color[] groupColors = { Colors.Blue, Colors.Red };

            for (int i = 0; i < 2; i++)
            {
                double[] heights = heightsByGroup[i];
                var hist = ScottPlot.Statistics.Histogram.WithBinSize(1, heights);

                // Display the histogram as a bar plot
                var barPlot = myPlot.Add.Bars(hist.Bins, hist.GetProbability());

                // Customize the style of each bar
                foreach (var bar in barPlot.Bars)
                {
                    bar.Size = hist.FirstBinSize;
                    bar.LineWidth = 0;
                    bar.FillStyle.AntiAlias = false;
                    bar.FillColor = groupColors[i].WithAlpha(.2);
                }

                // Plot the probability curve on top the histogram
                ScottPlot.Statistics.ProbabilityDensity pd = new(heights);
                double[] xs = Generate.Range(heights.Min(), heights.Max(), 1);
                double scale = 1.0 / hist.Bins.Select(x => pd.GetY(x)).Sum();
                double[] ys = pd.GetYs(xs, scale);

                var curve = myPlot.Add.ScatterLine(xs, ys);
                curve.LineWidth = 2;
                curve.LineColor = groupColors[i];
                curve.LinePattern = LinePattern.DenselyDashed;
                curve.LegendText = groupNames[i];
            }

            // Customize plot style
            myPlot.Legend.Alignment = Alignment.UpperRight;
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Probability (%)");
            myPlot.XLabel("Height (cm)");
            myPlot.HideGrid();
        }
    }

    public class HistogramCPH : RecipeBase
    {
        public override string Name => "Cumulative Probability Histogram";
        public override string Description => "A cumulative probability histogram represents the cumulative sum of probabilities " +
            "or relative frequencies up to each bin, providing a running total of the probability distribution. " +
            "It is especially useful for evaluating and comparing the distribution of multiple populations.";

        [Test]
        public override void Execute()
        {
            // Create a histogram from a collection of values
            double[][] heightsByGroup = { SampleData.MaleHeights(100), SampleData.FemaleHeights(100) };
            string[] groupNames = { "Male", "Female" };
            Color[] groupColors = { Colors.Blue, Colors.Red };

            for (int i = 0; i < 2; i++)
            {
                var hist = ScottPlot.Statistics.Histogram.WithBinSize(1, firstBin: 140, lastBin: 200);
                hist.AddRange(heightsByGroup[i]);

                var curve = myPlot.Add.ScatterLine(hist.Bins, hist.GetCumulativeProbability(100));
                curve.LineWidth = 1.5f;
                curve.LineColor = groupColors[i];
                curve.LegendText = groupNames[i];
                curve.ConnectStyle = ConnectStyle.StepVertical;
            }

            // Customize plot style
            myPlot.Legend.Alignment = Alignment.LowerRight;
            myPlot.YLabel("Cumulative Probability (%)");
            myPlot.XLabel("Height (cm)");
        }
    }
}
