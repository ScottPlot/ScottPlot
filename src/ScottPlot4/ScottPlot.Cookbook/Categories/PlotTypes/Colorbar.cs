namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Colorbar : ICategory
{
    public string Name => "Colorbar";

    public string Folder => "plottable-colorbar";

    public string Description => "A colorbar displays a colormap beside the data area. " +
        "Colorbars are typically added to plots containing heatmaps.";
}
