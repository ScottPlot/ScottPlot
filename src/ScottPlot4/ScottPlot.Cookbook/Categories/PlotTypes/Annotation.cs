namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Annotation : ICategory
{
    public string Name => "Annotation";

    public string Folder => "plottable-annotation";

    public string Description => "An Annotation is a text label that is placed " +
        "on the plot in pixel space (not in coordinate space like a Text object).";
}
