namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class SignalConst : ICategory
{
    public string Name => "SignalConst";

    public string Folder => "plottable-signalconst";

    public string Description => "SignalConst plots pre-processes data to render much faster " +
        "than Signal plots. Pre-processing takes time up-front and requires 4x the memory of Signal.";
}
