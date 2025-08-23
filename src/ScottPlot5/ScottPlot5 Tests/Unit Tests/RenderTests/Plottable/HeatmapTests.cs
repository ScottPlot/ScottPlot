namespace ScottPlotTests.RenderTests.Plottable;

internal class HeatmapTests
{
    [Test]
    public void Test_Heatmap_SvgGradient()
    {
        ScottPlot.Plot plot = new();
        double[,] data = {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 },
        };

        var heatmap = plot.Add.Heatmap(data);
        plot.Title("default");
        plot.SaveTestSvg(400, 300, "default");

        heatmap.RenderStrategy = new ScottPlot.Plottables.Heatmap.RenderStrategies.Rectangles();
        plot.Title("RenderCellsAsRectangles");
        plot.SaveTestSvg(400, 300, "enabled");
    }
}
