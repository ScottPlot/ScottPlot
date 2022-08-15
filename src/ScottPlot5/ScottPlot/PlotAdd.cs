namespace ScottPlot;

/// <summary>
/// This class contains helper methods for creating and adding new <see cref="IPlottable"/> objects to a <see cref="Plot"/>
/// </summary>
public class PlotAdd
{
    private readonly Plot Plot;

    public IPalette Palette { get; set; } = new Palettes.Category10();

    public PlotAdd(Plot plot)
    {
        Plot = plot;
    }

    private void Add(IPlottable plottable)
    {
        Plot.Plottables.Add(plottable);
    }

    private Color GetNextColor()
    {
        return Palette.GetColor(Plot.Plottables.Count);
    }

    public Plottables.Scatter Scatter(double[] xs, double[] ys, Color? color = null)
    {
        color ??= GetNextColor();
        Plottables.Scatter scatter = new(xs, ys) { Color = color.Value };

        Add(scatter);

        return scatter;
    }

    public Plottables.Pie Pie(IList<Plottables.PieSlice> slices)
    {
        Plottables.Pie pie = new(slices);
        Add(pie);
        return pie;
    }

}
