using ScottPlot.Plottables;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableFactory
{
    private readonly Plot Plot;

    public IPalette Palette { get; set; } = new Palettes.Category10();
    private Color NextColor => Palette.GetColor(Plot.Plottables.Count);

    public PlottableFactory(Plot plot)
    {
        Plot = plot;
    }

    public Heatmap Heatmap(double[,] intensities)
    {
        Heatmap heatmap = new(intensities);
        Plot.Plottables.Add(heatmap);
        return heatmap;
    }

    public Legend Legend(IList<LegendItem>? legendItems = null)
    {
        Legend legend = new(legendItems ?? Plot.LegendItems());
        Plot.Plottables.Add(legend);
        return legend;
    }

    public Pie Pie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Plot.Plottables.Add(pie);
        return pie;
    }

    public Scatter Scatter(double[] xs, double[] ys, Color? color = null)
    {
        DataSource.ScatterSourceXsYs data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? NextColor
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(List<double> xs, List<double> ys, Color? color = null)
    {
        DataSource.ScatterSourceXsYs data = new(xs, ys);
        Scatter scatter = new(data)
        {
            Color = color ?? NextColor
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(Coordinates[] coordinates, Color? color = null)
    {
        DataSource.ScatterSourceCoordinates data = new(coordinates);
        Scatter scatter = new(data)
        {
            Color = color ?? NextColor
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(List<Coordinates> coordinates, Color? color = null)
    {
        DataSource.ScatterSourceCoordinates data = new(coordinates);
        Scatter scatter = new(data)
        {
            Color = color ?? NextColor
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Signal Signal(double[] ys, double period = 1, Color? color = null)
    {
        DataSource.SignalSource data = new(ys, period);
        Signal scatter = new(data)
        {
            Color = color ?? NextColor
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }
}
