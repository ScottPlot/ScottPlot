using ScottPlot.Plottables;

namespace ScottPlot;

/// <summary>
/// This class a list of plottables with extra helper methods for creating and adding new plottables
/// </summary>
public class PlottableList : List<IPlottable>
{
    public IPalette Palette { get; set; } = new Palettes.Category10();

    private Color GetNextColor() => Palette.GetColor(Count);

    #region Helper methods for creating and adding new plottables

    public Pie AddPie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Add(pie);
        return pie;
    }

    public Scatter AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        DataSource.ScatterSourceXsYs data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(List<double> xs, List<double> ys, Color? color = null)
    {
        DataSource.ScatterSourceXsYs data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(Coordinates[] coordinates, Color? color = null)
    {
        DataSource.ScatterSourceCoordinates data = new(coordinates);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Scatter AddScatter(List<Coordinates> coordinates, Color? color = null)
    {
        DataSource.ScatterSourceCoordinates data = new(coordinates);
        Scatter scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    public Signal AddSignal(double[] ys, double period = 1, Color? color = null)
    {
        DataSource.SignalSource data = new(ys, period);
        Signal scatter = new(data)
        {
            Color = color ?? GetNextColor()
        };
        Add(scatter);
        return scatter;
    }

    #endregion
}
