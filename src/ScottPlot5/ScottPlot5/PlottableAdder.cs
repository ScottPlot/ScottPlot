using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.DataSources;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableAdder
{
    private readonly Plot Plot;

    public IPalette Palette { get; set; } = new Palettes.Category10();

    public Color GetNextColor()
    {
        return Palette.Colors[Plot.PlottableList.Count % Palette.Colors.Length];
    }

    public PlottableAdder(Plot plot)
    {
        Plot = plot;
    }

    public BarPlot Bar(Bar bar)
    {
        BarPlot bp = new(bar);
        Plottable(bp);
        return bp;
    }

    public BarPlot Bar(double position, double value, double error = 0)
    {
        Bar bar = new()
        {
            Position = position,
            Value = value,
            Error = error,
            FillColor = GetNextColor(),
        };
        return Bar(bar);
    }

    public BarPlot Bars(IEnumerable<Bar> bars)
    {
        BarPlot bp = new(bars);
        Plottable(bp);
        return bp;
    }

    public BarPlot Bars(double[] values)
    {
        var positions = Enumerable.Range(0, values.Length).Select(x => (double)x);
        return Bars(positions, values);
    }

    public BarPlot Bars(IEnumerable<double> positions, IEnumerable<double> values)
    {
        if (positions.Count() != values.Count())
        {
            throw new ArgumentException($"{nameof(positions)} and {nameof(positions)} have different lengths");
        }

        Color color = GetNextColor();

        List<Bar> bars = new();
        foreach (var item in positions.Zip(values, (a, b) => new { a, b }))
        {
            bars.Add(new Bar()
            {
                Position = item.a,
                Value = item.b,
                FillColor = color
            });
        }

        return Bars(bars);
    }

    public BoxPlot Box(IList<Box> boxes)
    {
        BoxGroup singleGroup = new()
        {
            Boxes = boxes,
        };

        singleGroup.Fill.Color = GetNextColor();

        IList<BoxGroup> groups = new List<BoxGroup>() { singleGroup };

        return Box(groups);
    }

    public BoxPlot Box(IList<BoxGroup> groups)
    {
        BoxGroups boxGroups = new()
        {
            Series = groups,
        };

        BoxPlot boxPlot = new()
        {
            Groups = boxGroups,
        };

        Plot.PlottableList.Add(boxPlot);
        return boxPlot;
    }

    public CandlestickPlot Candlestick(IList<IOHLC> ohlcs)
    {
        OHLCSource dataSource = new(ohlcs);
        CandlestickPlot candlestickPlot = new(dataSource);
        Plot.PlottableList.Add(candlestickPlot);
        return candlestickPlot;
    }

    public Crosshair Crosshair(double x, double y)
    {
        Crosshair ch = new()
        {
            Position = new(x, y)
        };
        ch.LineStyle.Color = GetNextColor();
        Plot.PlottableList.Add(ch);
        return ch;
    }

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new(source, edge);

        Plot.Panels.Add(colorBar);
        return colorBar;
    }

    public ErrorBar ErrorBar(IReadOnlyList<double> xs, IReadOnlyList<double> ys, IReadOnlyList<double> yErrors)
    {
        ErrorBar eb = new(xs, ys, null, null, yErrors, yErrors)
        {
            Color = GetNextColor(),
        };

        Plot.PlottableList.Add(eb);
        return eb;
    }

    /// <summary>
    /// Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(double[] xs, double[] ys1, double[] ys2)
    {
        List<(double, double, double)> data = new();

        for (int i = 0; i < xs.Length; i++)
        {
            data.Add((xs[i], ys1[i], ys2[i]));
        }

        return FillY(data);
    }

    /// <summary>
    /// Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(Scatter scatter1, Scatter scatter2)
    {
        FillY rangePlot = new(scatter1, scatter2);
        rangePlot.FillStyle.Color = GetNextColor();
        Plot.PlottableList.Add(rangePlot);
        return rangePlot;
    }

    /// <summary>
    /// Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(ICollection<(double X, double Top, double Bottom)> data)
    {
        FillY rangePlot = new();
        rangePlot.FillStyle.Color = GetNextColor();
        rangePlot.SetDataSource(data);
        Plot.PlottableList.Add(rangePlot);
        return rangePlot;
    }

    /// <summary>
    /// Fill the vertical range between two Y points for each X point
    /// This overload uses a custom function to calculate X, Y1, and Y2 values
    /// </summary>
    public FillY FillY<T>(ICollection<T> data, Func<T, (double X, double Top, double Bottom)> function)
    {
        var rangePlot = new FillY();
        rangePlot.FillStyle.Color = GetNextColor();
        rangePlot.SetDataSource(data, function);
        Plot.PlottableList.Add(rangePlot);
        return rangePlot;
    }

    public Heatmap Heatmap(double[,] intensities)
    {
        Heatmap heatmap = new(intensities);
        Plot.PlottableList.Add(heatmap);
        return heatmap;
    }

    public HorizontalLine HorizontalLine(double y, float width = 2, Color? color = null, LinePattern pattern = LinePattern.Solid)
    {
        HorizontalLine line = new();
        line.LineStyle.Width = width;
        line.LineStyle.Color = color ?? GetNextColor();
        line.LineStyle.Pattern = pattern;
        line.Y = y;
        Plot.PlottableList.Add(line);
        return line;
    }

    public Scatter Line(double x1, double y1, double x2, double y2)
    {
        Coordinates[] coordinates = { new(x1, y1), new(x2, y2) };
        return Scatter(coordinates);
    }

    public Scatter Line(Coordinates pt1, Coordinates pt2)
    {
        Coordinates[] coordinates = { pt1, pt2 };
        return Scatter(coordinates);
    }

    public Marker Marker(double x, double y, MarkerShape shape, float size, Color color)
    {
        MarkerStyle markerStyle = new MarkerStyle(shape, size, color);
        Coordinates location = new(x, y);
        return Marker(location, markerStyle);
    }
    public Marker Marker(Coordinates location, MarkerShape shape, float size, Color color)
    {
        MarkerStyle markerStyle = new MarkerStyle(shape, size, color);
        return Marker(location, markerStyle);
    }

    public Marker Marker(double x, double y, MarkerStyle markerStyle)
    {
        Coordinates location = new(x, y);
        return Marker(location, markerStyle);
    }
    
    public Marker Marker(Coordinates location, MarkerStyle markerStyle)
    {
        Marker marker = new();
        marker.MarkerStyle = markerStyle;
        marker.Location = location;
        Plot.PlottableList.Add(marker);
        return marker;
    }

    public OhlcPlot OHLC(IList<IOHLC> ohlcs)
    {
        OHLCSource dataSource = new(ohlcs);
        OhlcPlot ohlc = new(dataSource);
        Plot.PlottableList.Add(ohlc);
        return ohlc;
    }

    public Pie Pie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Plot.PlottableList.Add(pie);
        return pie;
    }

    public Pie Pie(IEnumerable<double> values)
    {
        var slices = values.Select(v => new PieSlice
        {
            Value = v,
            Fill = new() { Color = GetNextColor() },
        }).ToList();
        var pie = Pie(slices);
        Plot.PlottableList.Add(pie);
        return pie;
    }

    public Polygon Polygon(Coordinates[] coordinates)
    {
        Polygon poly = new(coordinates);
        Plot.PlottableList.Add(poly);
        return poly;
    }

    public IPlottable Plottable(IPlottable plottable)
    {
        Plot.PlottableList.Add(plottable);
        return plottable;
    }

    public Scatter Scatter(IScatterSource data, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        Scatter scatter = new(data);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.Fill.Color = nextColor;
        Plot.PlottableList.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(double x, double y, Color? color = null)
    {
        double[] xs = { x };
        double[] ys = { y };
        return Scatter(new ScatterSourceXsYs(xs, ys), color);
    }

    public Scatter Scatter(IReadOnlyList<double> xs, IReadOnlyList<double> ys, Color? color = null)
    {
        return Scatter(new ScatterSourceXsYs(xs, ys), color);
    }

    public Scatter Scatter(Coordinates point, Color? color = null)
    {
        Coordinates[] coordinates = { point };
        return Scatter(new ScatterSourceCoordinates(coordinates), color);
    }

    public Scatter Scatter(IReadOnlyList<Coordinates> coordinates, Color? color = null)
    {
        return Scatter(new ScatterSourceCoordinates(coordinates), color);
    }

    public Signal Signal(IReadOnlyList<double> ys, double period = 1, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        SignalSource data = new(ys, period);
        var sig = new Signal(data);
        sig.LineStyle.Color = nextColor;
        sig.Marker.Fill.Color = nextColor;
        Plot.PlottableList.Add(sig);
        return sig;
    }

    public Text Text(string text, Coordinates location)
    {
        return Text(text, location.X, location.Y);
    }

    public Text Text(string text, double x, double y)
    {
        Text txt = new();
        txt.Label.Text = text;
        txt.Label.BackColor = Colors.Transparent;
        txt.Label.BorderColor = Colors.Transparent;
        txt.Location = new(x, y);
        Plot.PlottableList.Add(txt);
        return txt;
    }

    public VerticalLine VerticalLine(double x, float width = 2, Color? color = null, LinePattern pattern = LinePattern.Solid)
    {
        VerticalLine line = new();
        line.LineStyle.Width = width;
        line.LineStyle.Color = color ?? GetNextColor();
        line.LineStyle.Pattern = pattern;
        line.X = x;
        Plot.PlottableList.Add(line);
        return line;
    }
}
