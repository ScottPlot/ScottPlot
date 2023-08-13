namespace ScottPlotTests.RenderTests;

internal class MultiPlotTests
{
    [Test]
    public void Test_Multiplot_Save()
    {
        // create a multiplot
        Multiplot multiplot = new();

        // customize the layout
        multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 3);

        // create plots and add them to the multiplot
        RandomDataGenerator gen = new();
        for (int i = 0; i < 6; i++)
        {
            Plot plot = new();
            plot.Add.Signal(gen.RandomWalk(100));
            plot.Title($"Plot {i + 1}");
            multiplot.Add(plot);
        }

        // apply the layout from the first plot to all subplots
        multiplot.SharedLayoutSourcePlot = multiplot.Plots.First();

        multiplot.Save("multiplot.png", 600, 400);
    }
}
