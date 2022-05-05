namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Coxcomb : ICategory
{
    public string Name => "Coxcomb Chart";

    public string Folder => "plottable-coxcomb";

    public string Description => "A Coxcomb chart is a pie graph " +
        "where the angle of slices is constant but the radii are not.";
}
