namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class SignalXY : ICategory
{
    public string Name => "SignalXY";

    public string Folder => "plottable-signalxy";

    public string Description => "SignalXY is a speed-optimized plot for displaying Y vaues " +
        "with unevenly-spaced (but always increasing) X positions.";
}
