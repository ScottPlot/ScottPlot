namespace ScottPlot;

/// <summary>
/// Logic that manages a collection of subplots with logic that can perform
/// custom actions when subplots are added or removed.
/// </summary>
public class SubplotCollection
{
    readonly List<Plot> Plots = [];

    public Action<List<Plot>> PlotAddedAction = CopyStyleOntoLastPlot;

    public int Count => Plots.Count;

    public void Add(Plot plot)
    {
        Plots.Add(plot);

        if (Plots.Count > 0)
        {
            plot.PlotControl = Plots.First().PlotControl;
        }

        PlotAddedAction.Invoke(Plots);
    }

    public void Remove(Plot plot) => Plots.Remove(plot);
    public void RemoveAt(int index) => Plots.RemoveAt(index);

    public Plot GetPlot(int index) => Plots[index];
    public Plot[] GetPlots() => [.. Plots];

    public static void CopyStyleOntoLastPlot(List<Plot> plots)
    {
        plots.Last().FigureBackground.Color = plots.First().FigureBackground.Color;
        plots.Last().DataBackground.Color = plots.First().DataBackground.Color;
    }
}
