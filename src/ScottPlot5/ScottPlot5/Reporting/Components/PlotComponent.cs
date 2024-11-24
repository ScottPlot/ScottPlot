namespace ScottPlot.Reporting.Components;

public class PlotComponent(Plot plot) : IComponent
{
    public Plot Plot { get; } = plot;
    public int Width { get; set; } = 600;
    public int Height { get; set; } = 400;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

