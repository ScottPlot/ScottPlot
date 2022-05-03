namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class VectorField : ICategory
{
    public string Name => "Vector Field";

    public string Folder => "plottable-vector-field";

    public string Description => "Vector fields use arrows to show direction and magnitude " +
        "of data points in a 2D array and are ideal for visualizing data explained by differential equations.";
}
