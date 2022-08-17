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
        DataSource.ScatterXYArrays data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(List<double> xs, List<double> ys, Color? color = null)
    {
        DataSource.ScatterXYLists data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(Coordinates[] coordinates, Color? color = null)
    {
        DataSource.ScatterCoordinatesArray data = new(coordinates);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(List<Coordinates> coordinates, Color? color = null)
    {
        DataSource.ScatterCoordinatesList data = new(coordinates);
        Scatter scatter = new(data)
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
