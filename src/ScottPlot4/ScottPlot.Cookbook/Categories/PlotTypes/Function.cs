namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Function : ICategory
{
    public string Name => "Function";

    public string Folder => "plottable-function";

    public string Description => "Function plots accept a Func (not distinct X/Y data points) " +
        "to create line plots which can be zoomed infinitely.";
}
