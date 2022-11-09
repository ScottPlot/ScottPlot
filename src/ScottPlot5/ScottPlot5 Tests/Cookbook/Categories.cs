namespace ScottPlot_Tests.Cookbook;

public static class Categories
{
    public static Category Quickstart => new()
    {
        Name = "Quickstart",
        Description = "An introduction to ScottPlot5",
    };

    public static Category PlotTypes => new()
    {
        Name = "Plot Types",
        Description = "Example use cases for the primary plot types",
    };
}
