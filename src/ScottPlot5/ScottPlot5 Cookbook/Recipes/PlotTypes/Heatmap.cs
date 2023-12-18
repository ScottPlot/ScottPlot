namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Heatmap : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Heatmap";
    public string CategoryDescription => "Heatmaps display values from 2D data " +
        "as an image with cells of different intensities";

    public class HeatmapQuickstart : RecipeBase
    {
        public override string Name => "Heatmap Quickstart";
        public override string Description => "Heatmaps can be created from 2D arrays";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();
            myPlot.Add.Heatmap(data);
        }
    }

    public class HeatmapInverted : RecipeBase
    {
        public override string Name => "Inverted Heatmap";
        public override string Description => "Heatmaps can be inverted by " +
            "reversing the order of colors in the colormap";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Viridis();
            hm1.Extent = new(0, 65, 0, 100);

            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Colormap = new ScottPlot.Colormaps.Viridis().Reversed();
            hm2.Extent = new(100, 165, 0, 100);
        }
    }
}
