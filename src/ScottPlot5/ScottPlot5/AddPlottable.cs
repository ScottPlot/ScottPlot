using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.DataSources;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class AddPlottable
{
    private readonly Plot Plot;

    public IPalette Palette { get; set; } = new Palettes.Category10();

    private Color NextColor => Palette.GetColor(Plot.Plottables.Count);

    public AddPlottable(Plot plot)
    {
        Plot = plot;
    }

    public Heatmap Heatmap(double[,] intensities)
    {
        Heatmap heatmap = new(intensities);
        Plot.Plottables.Add(heatmap);
        return heatmap;
    }

    public Pie Pie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Plot.Plottables.Add(pie);
        return pie;
    }

    public Pie Pie(IEnumerable<double> values)
    {
        var slices = values.Select(v => new PieSlice
        {
            Value = v,
            Fill = new() { Color = NextColor },
        }).ToList();
        return Pie(slices);
    }

    public Scatter Scatter(IScatterSource data, Color? color = null)
    {
        Color nextColor = color ?? NextColor;
        Scatter scatter = new(data);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.Fill.Color = nextColor;
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(IReadOnlyList<double> xs, IReadOnlyList<double> ys, Color? color = null)
    {
        return Scatter(new ScatterSourceXsYs(xs, ys), color);
    }

    public Scatter Scatter(IReadOnlyList<Coordinates> coordinates, Color? color = null)
    {
        return Scatter(new ScatterSourceCoordinates(coordinates), color);
    }

    public Signal Signal(IReadOnlyList<double> ys, double period = 1, Color? color = null)
    {
        Color nextColor = color ?? NextColor;
        SignalSource data = new(ys, period);
        var sig = new Signal(data);
        sig.LineStyle.Color = nextColor;
        sig.Marker.Fill.Color = nextColor;
        Plot.Plottables.Add(sig);
        return sig;
    }

    public BarPlot Bar(double[] values)
    {
        IList<Bar> bars = values.Select(x => new Bar() { Value = x }).ToList();
        return Bar(bars);
    }

    public BarPlot Bar(IList<BarSeries> series)
    {
        var barPlot = new BarPlot(series);
        Plot.Plottables.Add(barPlot);
        return barPlot;
    }

    public BarPlot Bar(IList<Bar> bars, Color? color = null, string? label = null)
    {
        var series = new BarSeries()
        {
            Bars = bars,
            Color = color ?? NextColor,
            Label = label
        };

        List<BarSeries> seriesList = new() { series };

        return Bar(seriesList);
    }

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new(source, edge);

        Plot.Panels.Add(colorBar);
        return colorBar;
    }

    public ErrorBar ErrorBar(IReadOnlyList<double> xs, IReadOnlyList<double> ys, IReadOnlyList<double>? xErrorPositive = null, IReadOnlyList<double>? xErrorNegative = null, IReadOnlyList<double>? yErrorPositive = null, IReadOnlyList<double>? yErrorNegative = null, Color? color = null)
    {
        color = color ?? NextColor;
        ErrorBar errorBar = new(xs, ys, xErrorPositive, xErrorNegative, yErrorPositive, yErrorNegative, color.Value);
        Plot.Plottables.Add(errorBar);
        return errorBar;
    }
}
