namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Text : ICategory
{
    public string Name => "Text";

    public string Folder => "plottable-text";

    public string Description => "A text label that is placed at an X/Y coordinate on the plot " +
        "(not in pixel space like an Annotation).";
}
