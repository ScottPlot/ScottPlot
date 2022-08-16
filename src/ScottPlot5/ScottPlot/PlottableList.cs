using ScottPlot.Plottables;

namespace ScottPlot;

/// <summary>
/// This class a list of plottables with extra helper methods for creating and adding new plottables
/// </summary>
public class PlottableList : List<IPlottable>
{
    public IPalette Palette { get; set; } = new Palettes.Category10();

    private Color GetNextColor() => Palette.GetColor(Count);

    public Scatter AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        Scatter scatter = new(xs, ys)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Pie AddPie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Add(pie);
        return pie;
    }
}
