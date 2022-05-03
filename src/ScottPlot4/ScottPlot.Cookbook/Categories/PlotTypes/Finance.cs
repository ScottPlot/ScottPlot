namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Finance : ICategory
{
    public string Name => "Finance";

    public string Folder => "plottable-finance";

    public string Description => "Finance charts represent price over a binned time range " +
        "using OHLC (open, high, low, close) data format.";
}
