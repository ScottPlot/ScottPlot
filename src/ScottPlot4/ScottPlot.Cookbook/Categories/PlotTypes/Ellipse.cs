namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Ellipse : ICategory
{
    public string Name => "Ellipse";

    public string Folder => "plottable-ellipse";

    public string Description => "Ellipses are cuves with a defined center and distinct X and Y radii. " +
        "A circle is an ellipse with an X radius equal to its Y radius.";
}
