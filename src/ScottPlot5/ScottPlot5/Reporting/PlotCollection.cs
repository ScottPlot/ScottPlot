namespace ScottPlot.Reporting;

public class PlotCollection
{
    public List<PlotFigure> Figures { get; } = [];

    public void Add(Plot plot, string? title = null, string? description = null)
    {
        Figures.Add(new(plot, title ?? string.Empty, description ?? string.Empty));
    }
}
