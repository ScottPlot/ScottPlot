using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.DataSources;
using ScottPlot.Plottable;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableAdder(Plot plot)
{
    private readonly Plot Plot = plot;

    /// <summary>
    /// Color set used for adding new plottables
    /// </summary>
    public IPalette Palette { get; set; } = new Palettes.Category10();

    public Color GetNextColor()
    {
        return Palette.Colors[Plot.PlottableList.Count % Palette.Colors.Length];
    }

    public Annotation Annotation(string text, Alignment alignment = Alignment.UpperLeft)
    {
        Annotation an = new()
        {
            Alignment = alignment,
        };

        an.Label.Text = text;
        an.Label.BackColor = Colors.Yellow.WithAlpha(.75);
        an.Label.BorderColor = Colors.Black;
        an.Label.Padding = 5;

        Plot.PlottableList.Add(an);

        return an;
    }

    public Arrow Arrow(Coordinates @base, Coordinates tip)
    {
        Arrow ar = new()
        {
            Color = GetNextColor(),
            Base = @base,
            Tip = tip,
        };
        Plot.PlottableList.Add(ar);
        return ar;
    }

    public Arrow Arrow(double xBase, double yBase, double xTip, double yTip)
    {
        Coordinates cBase = new(xBase, yBase);
        Coordinates cTip = new(xTip, yTip);
        return Arrow(cBase, cTip);
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

    public BoxPlot Box(Box box)
    {
        BoxPlot bp = new();
        bp.Boxes.Add(box);
        bp.FillColor = GetNextColor();
        Plot.PlottableList.Add(bp);
        return bp;
    }

    public BoxPlot Boxes(IEnumerable<Box> boxes)
    {
        BoxPlot bp = new();
        bp.Boxes.AddRange(boxes);
        bp.FillColor = GetNextColor();
        Plot.PlottableList.Add(bp);
        return bp;
    }

    public CandlestickPlot Candlestick(List<OHLC> ohlcs)
    {
        OHLCSource dataSource = new(ohlcs);
        CandlestickPlot candlestickPlot = new(dataSource);
        Plot.PlottableList.Add(candlestickPlot);
        return candlestickPlot;
    }

    public Ellipse Circle(Coordinates center, double radius)
    {
        return Ellipse(center, radius, radius);
    }

    public Ellipse Circle(double xCenter, double yCenter, double radius)
    {
        return Circle(new(xCenter, yCenter), radius);
    }

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new(source, edge);

        Plot.Axes.Panels.Add(colorBar);
        return colorBar;
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

    public DataLogger DataLogger()
    {
        DataLogger logger = new()
        {
            Color = GetNextColor(),
        };

        Plot.PlottableList.Add(logger);

        return logger;
    }

    public DataStreamer DataStreamer(int points, double period = 1)
    {
        double[] data = new double[points];

        DataStreamer streamer = new(Plot, data)
        {
            Color = GetNextColor(),
            Period = period,
        };

        Plot.PlottableList.Add(streamer);

        return streamer;
    }

    public Ellipse Ellipse(Coordinates center, double radiusX, double radiusY, float rotation = 0)
    {
        var ellipse = new Ellipse()
        {
            Center = center,
            RadiusX = radiusX,
            RadiusY = radiusY,
            Rotation = rotation
        };
        ellipse.LineStyle.Color = GetNextColor();

        Plot.PlottableList.Add(ellipse);
        return ellipse;
    }

    public Ellipse Ellipse(double xCenter, double yCenter, double radiusX, double radiusY, float rotation = 0)
    {
        return Ellipse(new Coordinates(xCenter, yCenter), radiusX, radiusY, rotation);
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

    public FunctionPlot Function(IFunctionSource functionSource)
    {
        FunctionPlot functionPlot = new(functionSource);
        functionPlot.LineStyle.Color = GetNextColor();

        Plot.PlottableList.Add(functionPlot);
        return functionPlot;
    }

    public FunctionPlot Function(Func<double, double> func)
    {
        var functionSource = new FunctionSource(func);
        return Function(functionSource);
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
        line.Label.BackColor = line.LineStyle.Color;
        line.LineStyle.Pattern = pattern;
        line.Y = y;
        Plot.PlottableList.Add(line);
        return line;
    }

    public HorizontalSpan HorizontalSpan(double x1, double x2, Color? color = null)
    {
        HorizontalSpan span = new() { X1 = x1, X2 = x2 };
        span.FillStyle.Color = color ?? GetNextColor().WithAlpha(.2);
        span.LineStyle.Color = span.FillStyle.Color.WithAlpha(.5);
        Plot.PlottableList.Add(span);
        return span;
    }

    public LinePlot Line(Coordinates start, Coordinates end)
    {
        LinePlot lp = new()
        {
            Start = start,
            End = end,
        };

        lp.LineStyle.Color = GetNextColor();
        lp.MarkerStyle.Fill.Color = lp.LineStyle.Color;

        Plot.PlottableList.Add(lp);

        return lp;
    }

    public LinePlot Line(CoordinateLine line)
    {
        return Line(line.Start, line.End);
    }

    public LinePlot Line(double x1, double y1, double x2, double y2)
    {
        Coordinates start = new(x1, y1);
        Coordinates end = new(x2, y2);
        return Line(start, end);
    }

    public Marker Marker(double x, double y, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        Plottables.Marker mp = new()
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
            Location = new Coordinates(x, y),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Marker Marker(Coordinates location, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        return Marker(location.X, location.Y, shape, size, color);
    }

    public Plottables.Markers Markers(double[] xs, double[] ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceDoubleArray ss = new(xs, ys);

        Plottables.Markers mp = new(ss)
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(Coordinates[] coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesArray ss = new(coordinates);

        Plottables.Markers mp = new(ss)
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(List<Coordinates> coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesList ss = new(coordinates);

        Plottables.Markers mp = new(ss)
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(TX[] xs, TY[] ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericArray<TX, TY> ss = new(xs, ys);

        Plottables.Markers mp = new(ss)
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(List<TX> xs, List<TY> ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericList<TX, TY> ss = new(xs, ys);

        Plottables.Markers mp = new(ss)
        {
            MarkerStyle = new MarkerStyle(shape, size, color ?? GetNextColor()),
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public OhlcPlot OHLC(List<OHLC> ohlcs)
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
        List<PieSlice> slices = new();
        foreach (double value in values)
        {
            PieSlice slice = new()
            {
                Value = value,
                FillColor = Palette.GetColor(slices.Count),
            };

            slices.Add(slice);
        }

        Pie pie = new(slices);
        Plot.PlottableList.Add(pie);
        return pie;
    }

    public IPlottable Plottable(IPlottable plottable)
    {
        Plot.PlottableList.Add(plottable);
        return plottable;
    }

    public Polygon Polygon(Coordinates[] coordinates)
    {
        Polygon poly = new(coordinates);
        Plot.PlottableList.Add(poly);
        return poly;
    }

    public Polygon Polygon<TX, TY>(IEnumerable<TX> xs, IEnumerable<TY> ys)
    {
        Coordinates[] coordinates = NumericConversion.GenericToCoordinates(xs, ys);
        Polygon poly = new(coordinates);
        Plot.PlottableList.Add(poly);
        return poly;
    }

    public RadialGaugePlot RadialGaugePlot(IEnumerable<double> values)
    {
        Color[] colors = Enumerable.Range(0, values.Count()).Select(x => Palette.GetColor(x)).ToArray();
        RadialGaugePlot radialGaugePlot = new(values.ToArray(), colors);
        Plot.PlottableList.Add(radialGaugePlot);
        Plot.HideGrid();
        Plot.Layout.Frameless();
        return radialGaugePlot;
    }

    public Rectangle Rectangle(CoordinateRect rect)
    {
        return Rectangle(rect.Left, rect.Right, rect.Top, rect.Bottom);
    }

    public Rectangle Rectangle(double left, double right, double bottom, double top)
    {
        Rectangle rp = new()
        {
            X1 = left,
            X2 = right,
            Y1 = bottom,
            Y2 = top,
        };

        rp.FillStyle.Color = GetNextColor();
        Plot.PlottableList.Add(rp);
        return rp;
    }

    public Scatter Scatter(IScatterSource source, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        Scatter scatter = new(source);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.Fill.Color = nextColor;
        Plot.PlottableList.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(double x, double y, Color? color = null)
    {
        double[] xs = { x };
        double[] ys = { y };
        ScatterSourceDoubleArray source = new(xs, ys);
        return Scatter(source, color);
    }

    public Scatter Scatter(double[] xs, double[] ys, Color? color = null)
    {
        ScatterSourceDoubleArray source = new(xs, ys);
        return Scatter(source, color);
    }

    public Scatter Scatter(Coordinates point, Color? color = null)
    {
        Coordinates[] coordinates = { point };
        ScatterSourceCoordinatesArray source = new(coordinates);
        return Scatter(source, color);
    }

    public Scatter Scatter(Coordinates[] coordinates, Color? color = null)
    {
        ScatterSourceCoordinatesArray source = new(coordinates);
        return Scatter(source, color);
    }

    public Scatter Scatter(List<Coordinates> coordinates, Color? color = null)
    {
        ScatterSourceCoordinatesList source = new(coordinates);
        return Scatter(source, color);
    }

    public Scatter Scatter<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        ScatterSourceGenericArray<T1, T2> source = new(xs, ys);
        Scatter scatter = new(source);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.Fill.Color = nextColor;
        Plot.PlottableList.Add(scatter);
        return scatter;
    }

    public Scatter Scatter<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        ScatterSourceGenericList<T1, T2> source = new(xs, ys);
        Scatter scatter = new(source);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.Fill.Color = nextColor;
        Plot.PlottableList.Add(scatter);
        return scatter;
    }

    public Scatter ScatterLine(IScatterSource source, Color? color = null)
    {
        var scatter = Scatter(source, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterLine(double[] xs, double[] ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterLine(Coordinates[] coordinates, Color? color = null)
    {
        var scatter = Scatter(coordinates, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterLine(List<Coordinates> coordinates, Color? color = null)
    {
        var scatter = Scatter(coordinates, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterLine<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterLine<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;
        return scatter;
    }

    public Scatter ScatterPoints(IScatterSource source, Color? color = null)
    {
        var scatter = Scatter(source, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Scatter ScatterPoints(double[] xs, double[] ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Scatter ScatterPoints(Coordinates[] coordinates, Color? color = null)
    {
        var scatter = Scatter(coordinates, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Scatter ScatterPoints(List<Coordinates> coordinates, Color? color = null)
    {
        var scatter = Scatter(coordinates, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Scatter ScatterPoints<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Scatter ScatterPoints<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        var scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;
        return scatter;
    }

    public Signal Signal(ISignalSource source, Color? color = null)
    {
        Signal sig = new(source)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public Signal Signal(double[] ys, double period = 1, Color? color = null)
    {
        SignalSourceDouble source = new(ys, period);

        Signal sig = new(source)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public Signal Signal<T>(T[] ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericArray<T> source = new(ys, period);

        Signal sig = new(source)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public Signal Signal<T>(List<T> ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericList<T> source = new(ys, period);

        Signal sig = new(source)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalConst<T> SignalConst<T>(T[] ys, double period = 1, Color? color = null)
        where T : struct, IComparable
    {
        SignalConst<T> sig = new(ys, period)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalXY SignalXY(double[] xs, double[] ys, Color? color = null)
    {
        SignalXYSourceDoubleArray dataSource = new(xs, ys);

        SignalXY sig = new(dataSource)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalXY SignalXY<TX, TY>(TX[] xs, TY[] ys, Color? color = null)
    {
        var dataSource = new SignalXYSourceGenericArray<TX, TY>(xs, ys);

        SignalXY sig = new(dataSource)
        {
            Color = color ?? GetNextColor()
        };

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
        line.Label.BackColor = line.LineStyle.Color;
        line.LineStyle.Pattern = pattern;
        line.X = x;
        Plot.PlottableList.Add(line);
        return line;
    }

    public VerticalSpan VerticalSpan(double y1, double y2, Color? color = null)
    {
        VerticalSpan span = new() { Y1 = y1, Y2 = y2 };
        span.FillStyle.Color = color ?? GetNextColor().WithAlpha(.2);
        span.LineStyle.Color = span.FillStyle.Color.WithAlpha(.5);
        Plot.PlottableList.Add(span);
        return span;
    }
}
