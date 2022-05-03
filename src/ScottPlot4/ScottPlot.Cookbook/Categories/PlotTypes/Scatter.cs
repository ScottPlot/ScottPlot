namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Scatter : ICategory
{
    public string Name => "Scatter Plot";

    public string Folder => "plottable-scatter-plot";

    public string Description => "Scatter plots display small numbers of paired X/Y data points. " +
        "Signal plots are much faster than scatter plots and should be used when X data is evenly spaced.";
}
