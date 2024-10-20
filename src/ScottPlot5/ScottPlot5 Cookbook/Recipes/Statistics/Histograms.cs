namespace ScottPlotCookbook.Recipes.PlotTypes;

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
            double[] heights = Generate.RandomNormal(count: 1000, mean: 160, stdDev: 20);
            ScottPlot.Statistics.LiveHistogram hist = new(heights, 20);

            // Display the histogram as a bar plot
            var barPlot = myPlot.Add.Bars(hist.Bins, hist.Counts);

            // Size each bar slightly less than the width of a bin
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.BinSize * .8;
            }

            // Customize plot style
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Number of People");
            myPlot.XLabel("Height (cm)");
        }
    }
}
