namespace ScottPlot_Tests.Cookbook;

public static class RecipeCategories
{
    public static RecipeCategory Quickstart => new()
    {
        Name = "Quickstart",
        Description = "An introduction to ScottPlot5",
    };

    public static RecipeCategory PlotTypes => new()
    {
        Name = "Plot Types",
        Description = "Example use cases for the primary plot types",
    };
}
