using ScottPlot.DataSources;
using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.Statistics;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableAdder(Plot plot)
{
    public Plot Plot { get; } = plot;

    /// <summary>
    /// Color set used for adding new plottables
    /// </summary>
    public IPalette Palette { get; set; } = new Palettes.Category10();

    private int NextColorIndex = 0;

    /// <summary>
    /// Return the next color of the <see cref="Palette"/>.
    /// Colors reset if <see cref="Plot.PlottableList"/> is cleared.
    /// </summary>
    public Color GetNextColor(bool incrementCounter = true)
    {
        if (Plot.PlottableList.Count == 0)
            NextColorIndex = 0;

        Color color = Palette.GetColor(NextColorIndex);

        if (incrementCounter)
            NextColorIndex++;

        return color;
    }

    public Annotation Annotation(string text, Alignment alignment = Alignment.UpperLeft)
    {
        Annotation an = new()
        {
            Alignment = alignment,
            Text = text,
            LabelBackgroundColor = Colors.Yellow.WithAlpha(.75),
            LabelBorderColor = Colors.Black,
            LabelPadding = 5,
        };

        Plot.PlottableList.Add(an);

        return an;
    }

    public Ellipse AnnularEllipticalSector(
        Coordinates center,
        double outerRadiusX, double outerRadiusY, double innerRadiusX, double innerRadiusY,
        Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        Color color = GetNextColor();

        Ellipse ellipticalSector = new()
        {
            Center = center,
            RadiusX = outerRadiusX,
            RadiusY = outerRadiusY,
            InnerRadiusX = innerRadiusX,
            InnerRadiusY = innerRadiusY,
            StartAngle = startAngle,
            SweepAngle = sweepAngle,
            Rotation = rotation ?? Angle.FromDegrees(0),
            LineColor = color,
            IsSector = true,
        };

        Plot.PlottableList.Add(ellipticalSector);
        return ellipticalSector;
    }

    public Ellipse AnnularEllipticalSector(
        int xCenter, int yCenter,
        double outerRadiusX, double outerRadiusY, double innerRadiusX, double innerRadiusY,
        Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        return AnnularEllipticalSector(new(xCenter, yCenter), outerRadiusX, outerRadiusY, innerRadiusX, innerRadiusY, startAngle, sweepAngle, rotation);
    }

    public Ellipse AnnularSector(
        Coordinates center,
        double outerRadius, double innerRadius,
        Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        return AnnularEllipticalSector(center, outerRadius, outerRadius, innerRadius, innerRadius, startAngle, sweepAngle, rotation);
    }

    public Ellipse AnnularSector(
        int xCenter, int yCenter,
        double outerRadius, double innerRadius,
        Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        return AnnularSector(new(xCenter, yCenter), outerRadius, innerRadius, startAngle, sweepAngle, rotation);
    }

    public Ellipse Arc(Coordinates center, double radius, Angle startAngle, Angle sweepAngle)
    {
        return EllipticalArc(center, radius, radius, startAngle, sweepAngle);
    }

    public Ellipse Arc(double xCenter, double yCenter, double radius, Angle startAngle, Angle sweepAngle)
    {
        return Arc(new(xCenter, yCenter), radius, startAngle, sweepAngle);
    }

    public Arrow Arrow(Coordinates arrowBase, Coordinates arrowTip)
    {
        Color color = GetNextColor();

        Arrow arrow = new()
        {
            Base = arrowBase,
            Tip = arrowTip,
            ArrowLineColor = color,
            ArrowFillColor = color.WithAlpha(.3),
        };

        Plot.PlottableList.Add(arrow);

        return arrow;
    }

    public Arrow Arrow(double xBase, double yBase, double xTip, double yTip)
    {
        Coordinates arrowBase = new(xBase, yBase);
        Coordinates arrowTip = new(xTip, yTip);
        return Arrow(arrowBase, arrowTip);
    }

    public Arrow Arrow(CoordinateLine line)
    {
        return Arrow(line.Start, line.End);
    }

    public Annotation BackgroundText(string text, Color? color = null, double size = 48)
    {
        Annotation an = new()
        {
            Text = text,
            LabelFontColor = color ?? Colors.Gray.WithAlpha(.3),
            LabelFontSize = (float)size,
            LabelBackgroundColor = Colors.Transparent,
            LabelShadowColor = Colors.Transparent,
            LabelBorderColor = Colors.Transparent,
            Alignment = Alignment.MiddleCenter,
            OffsetX = 0,
            OffsetY = 0,
        };

        Plot.PlottableList.Insert(0, an);

        return an;
    }

    public (Annotation line1, Annotation line2) BackgroundText(string line1, string line2, Color? color = null, double size1 = 48, double size2 = 24)
    {
        Annotation an1 = BackgroundText(line1, color, size1);
        an1.Alignment = Alignment.LowerCenter;
        an1.FractionRect = FractionRect.Row(0, 2);

        Annotation an2 = BackgroundText(line2, color, size2);
        an2.Alignment = Alignment.UpperCenter;
        an2.FractionRect = FractionRect.Row(1, 2);

        return (an1, an2);
    }

    public BarPlot Bar(Bar bar)
    {
        List<Bar> bars = [bar];
        BarPlot bp = new(bars);
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

    public BarPlot Bars(List<Bar> bars)
    {
        BarPlot bp = new(bars);
        Plottable(bp);
        return bp;
    }

    public BarPlot Bars(Bar[] bars)
    {
        BarPlot bp = new([.. bars]);
        Plottable(bp);
        return bp;
    }

    public BarPlot Bars(double[] values)
    {
        var positions = Enumerable.Range(0, values.Length).Select(x => (double)x);
        return Bars(positions, values);
    }

    public BarPlot Bars<T>(IEnumerable<double> positions, IEnumerable<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return Bars(positions, values2);
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

    public Bracket Bracket(Coordinates point1, Coordinates point2, string? label = null)
    {
        Bracket bracket = new()
        {
            Point1 = point1,
            Point2 = point2,
            Text = label ?? string.Empty,
        };

        Plot.PlottableList.Add(bracket);

        return bracket;
    }

    public Bracket Bracket(double x1, double y1, double x2, double y2, string? label = null)
    {
        Coordinates point1 = new(x1, y1);
        Coordinates point2 = new(x2, y2);
        return Bracket(point1, point2, label);
    }

    public Bracket Bracket(CoordinateLine line, string? label = null)
    {
        return Bracket(line.Start, line.End, label);
    }

    public Callout Callout(string text, double textX, double textY, double tipX, double tipY)
    {
        Coordinates labelCoordinates = new(textX, textY);
        Coordinates lineCoordinates = new(tipX, tipY);
        return Callout(text, labelCoordinates, lineCoordinates);
    }

    public Callout Callout(string text, Coordinates textLocation, Coordinates tipLocation)
    {
        Color color = GetNextColor();

        Callout callout = new()
        {
            Text = text,
            TextCoordinates = textLocation,
            TipCoordinates = tipLocation,
            ArrowLineColor = Colors.Transparent,
            ArrowFillColor = color,
            TextBackgroundColor = color.Lighten(.5),
            TextBorderColor = color,
            TextBorderWidth = 2,
            TextColor = Colors.Black,
            FontSize = 14,
        };

        Plot.PlottableList.Add(callout);

        return callout;
    }

    public CandlestickPlot Candlestick(OHLC[] ohlcs)
    {
        OHLCSourceArray dataSource = new(ohlcs);
        CandlestickPlot candlestickPlot = new(dataSource);
        Plot.PlottableList.Add(candlestickPlot);
        return candlestickPlot;
    }

    public CandlestickPlot Candlestick(List<OHLC> ohlcs)
    {
        OHLCSourceList dataSource = new(ohlcs);
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

    public Ellipse CircleSector(Coordinates center, double radius, Angle startAngle, Angle sweepAngle)
    {
        return EllipticalSector(center, radius, radius, startAngle, sweepAngle);
    }

    public Ellipse CircleSector(int xCenter, int yCenter, int radius, Angle startAngle, Angle sweepAngle)
    {
        return CircleSector(new(xCenter, yCenter), radius, startAngle, sweepAngle);
    }

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new(source, edge);

        Plot.Axes.Panels.Add(colorBar);
        return colorBar;
    }

    public ContourLines ContourLines(Coordinates3d[,] coordinates, int count = 10)
    {
        ContourLines contour = new() { ContourLineCount = count };
        contour.Update(coordinates);
        Plot.PlottableList.Add(contour);
        return contour;
    }

    public ContourLines ContourLines(Coordinates3d[] coordinates, int count = 10)
    {
        ContourLines contour = new() { ContourLineCount = count };
        contour.Update(coordinates);
        Plot.PlottableList.Add(contour);
        return contour;
    }

    public Coxcomb Coxcomb(IList<PieSlice> slices)
    {
        Coxcomb coxcomb = new(slices);
        Plot.PlottableList.Add(coxcomb);
        return coxcomb;
    }

    public Coxcomb Coxcomb(IEnumerable<double> values)
    {
        List<PieSlice> slices = new();
        foreach (double value in values)
        {
            PieSlice slice = new()
            {
                Value = value,
                FillColor = Palette.GetColor(slices.Count).WithOpacity(0.5),
            };

            slices.Add(slice);
        }

        Coxcomb coxcomb = new(slices);
        Plot.PlottableList.Add(coxcomb);
        return coxcomb;
    }

    public Crosshair Crosshair(double x, double y)
    {
        Crosshair ch = new()
        {
            Position = new(x, y)
        };
        Color color = GetNextColor();
        ch.LineColor = color;
        ch.TextColor = color;
        Plot.PlottableList.Add(ch);
        return ch;
    }

    public DataLogger DataLogger()
    {
        List<Coordinates> coordinates = [];
        return DataLogger(coordinates);
    }

    public DataLogger DataLogger(List<Coordinates> coordinates)
    {
        DataLogger logger = new(coordinates)
        {
            Color = GetNextColor(),
        };

        Plot.PlottableList.Add(logger);

        return logger;
    }

    public DataStreamer DataStreamer(int points, double period = 1)
    {
        double[] data = Generate.NaN(points);

        DataStreamer streamer = new(Plot, data)
        {
            Color = GetNextColor(),
            Period = period,
        };

        Plot.PlottableList.Add(streamer);

        return streamer;
    }

    public DataStreamerXY DataStreamerXY(int capacity)
    {
        DataStreamerXY streamer = new(capacity)
        {
            Color = GetNextColor(),
        };

        Plot.PlottableList.Add(streamer);

        return streamer;
    }

    public Ellipse Ellipse(Coordinates center, double radiusX, double radiusY, Angle? rotation = null)
    {
        Color color = GetNextColor();

        Ellipse ellipse = new()
        {
            Center = center,
            RadiusX = radiusX,
            RadiusY = radiusY,
            Rotation = rotation ?? Angle.FromDegrees(0),
            LineColor = color,
        };

        Plot.PlottableList.Add(ellipse);
        return ellipse;
    }

    public Ellipse Ellipse(double xCenter, double yCenter, double radiusX, double radiusY, Angle? rotation = null)
    {
        return Ellipse(new Coordinates(xCenter, yCenter), radiusX, radiusY, rotation);
    }

    public Ellipse EllipticalArc(Coordinates center, double radiusX, double radiusY, Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        Color color = GetNextColor();

        Ellipse ellipticalSector = new()
        {
            Center = center,
            RadiusX = radiusX,
            RadiusY = radiusY,
            StartAngle = startAngle,
            SweepAngle = sweepAngle,
            Rotation = rotation ?? Angle.FromDegrees(0),
            LineColor = color,
            IsSector = false,
        };

        Plot.PlottableList.Add(ellipticalSector);
        return ellipticalSector;
    }

    public Ellipse EllipticalArc(double xCenter, double yCenter, double radiusX, double radiusY, Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        return EllipticalArc(new Coordinates(xCenter, yCenter), radiusX, radiusY, startAngle, sweepAngle, rotation);
    }

    public Ellipse EllipticalSector(Coordinates center, double radiusX, double radiusY, Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        Color color = GetNextColor();

        Ellipse ellipticalSector = new()
        {
            Center = center,
            RadiusX = radiusX,
            RadiusY = radiusY,
            StartAngle = startAngle,
            SweepAngle = sweepAngle,
            Rotation = rotation ?? Angle.FromDegrees(0),
            LineColor = color,
            IsSector = true,
        };

        Plot.PlottableList.Add(ellipticalSector);
        return ellipticalSector;
    }

    public Ellipse EllipticalSector(double xCenter, double yCenter, double radiusX, double radiusY, Angle startAngle, Angle sweepAngle, Angle? rotation = null)
    {
        return EllipticalSector(new Coordinates(xCenter, yCenter), radiusX, radiusY, startAngle, sweepAngle, rotation);
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

    public Heatmap Heatmap(Coordinates3d[,] values)
    {
        int height = values.GetLength(0);
        int width = values.GetLength(1);
        double[,] intensities = new double[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                intensities[y, x] = values[y, x].Z;
            }
        }
        return Heatmap(intensities);
    }

    public HistogramBars Histogram(Histogram histogram, Color? color = null, bool disableBottomPadding = true)
    {
        HistogramBars hb = new(histogram);
        Plot.PlottableList.Add(hb);

        foreach (var bar in hb.Bars)
        {
            bar.LineWidth = 0;
            bar.FillStyle.AntiAlias = false;
            bar.FillColor = color ?? Palette.GetColor(0);
        }

        if (disableBottomPadding)
            Plot.Axes.Margins(bottom: 0);

        return hb;
    }

    public HorizontalLine HorizontalLine(double y, float width = 2, Color? color = null, LinePattern pattern = default)
    {
        Color color2 = color ?? GetNextColor();
        HorizontalLine line = new()
        {
            LineWidth = width,
            LineColor = color2,
            LabelBackgroundColor = color2,
            LabelFontColor = Colors.White,
            LinePattern = pattern,
            Y = y
        };
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

    public ImageMarker ImageMarker(Coordinates location, Image image, float scale = 1)
    {
        ImageMarker marker = new()
        {
            Location = location,
            Image = image,
            Scale = scale,
        };

        Plot.PlottableList.Add(marker);

        return marker;
    }

    public ImageRect ImageRect(Image image, CoordinateRect rect)
    {
        ImageRect marker = new()
        {
            Image = image,
            Rect = rect,
        };

        Plot.PlottableList.Add(marker);

        return marker;
    }

    public Legend Legend()
    {
        Legend legend = new(Plot) { IsVisible = true };
        Plot.PlottableList.Add(legend);
        return legend;
    }

    public LinePlot Line(Coordinates start, Coordinates end)
    {
        LinePlot lp = new()
        {
            Start = start,
            End = end,
        };

        lp.LineStyle.Color = GetNextColor();
        lp.MarkerStyle.FillColor = lp.LineStyle.Color;

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

    public LollipopPlot Lollipop(double[] values)
    {
        var coordinates = Enumerable.Range(0, values.Length).Select(x => new Coordinates(x, values[x]));
        return Lollipop(coordinates);
    }

    public LollipopPlot Lollipop(double[] values, double[] positions)
    {
        Coordinates[] coordinates = Coordinates.Zip(positions, values);
        return Lollipop(coordinates);
    }

    public LollipopPlot Lollipop(IEnumerable<Coordinates> coordinates)
    {
        LollipopPlot plottable = new(coordinates) { Color = GetNextColor() };
        Plot.PlottableList.Add(plottable);
        return plottable;
    }

    public Marker Marker(double x, double y, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        Marker mp = new()
        {
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor(),
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
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(Coordinates[] coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesArray ss = new(coordinates);

        Plottables.Markers mp = new(ss)
        {
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(List<Coordinates> coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesList ss = new(coordinates);

        Plottables.Markers mp = new(ss)
        {
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(TX[] xs, TY[] ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericArray<TX, TY> ss = new(xs, ys);

        Plottables.Markers mp = new(ss)
        {
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(List<TX> xs, List<TY> ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericList<TX, TY> ss = new(xs, ys);

        Plottables.Markers mp = new(ss)
        {
            MarkerShape = shape,
            MarkerSize = size,
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public OhlcPlot OHLC(List<OHLC> ohlcs)
    {
        OHLCSourceList dataSource = new(ohlcs);
        OhlcPlot ohlc = new(dataSource);
        Plot.PlottableList.Add(ohlc);
        return ohlc;
    }

    public Phasor Phasor()
    {
        Phasor phasor = new();

        Color color = GetNextColor().WithAlpha(0.7);
        phasor.ArrowFillColor = color;
        phasor.ArrowLineColor = color;
        phasor.LabelStyle.ForeColor = phasor.ArrowFillColor;

        Plot.PlottableList.Add(phasor);
        return phasor;
    }

    public Phasor Phasor(IEnumerable<PolarCoordinates> points)
    {
        Phasor phasor = Phasor();
        phasor.Points.AddRange(points);
        return phasor;
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

    public PolarAxis PolarAxis(double radius = 1.0)
    {
        PolarAxis polarAxis = new() { };
        polarAxis.SetCircles(radius, 5);
        polarAxis.SetSpokes(12, radius * 1.1);

        Plot.PlottableList.Add(polarAxis);
        Plot.HideAxesAndGrid();
        return polarAxis;
    }

    public Polygon Polygon(Coordinates[] coordinates)
    {
        Color color = GetNextColor();
        Polygon poly = new(coordinates)
        {
            LineColor = color,
            FillColor = color.WithAlpha(.5),
        };
        Plot.PlottableList.Add(poly);
        return poly;
    }

    public Polygon Polygon<TX, TY>(IEnumerable<TX> xs, IEnumerable<TY> ys)
    {
        Coordinates[] coordinates = NumericConversion.GenericToCoordinates(xs, ys);
        return Polygon(coordinates);
    }

    public PopulationSymbol Population(double[] values, double x = 0)
    {
        Color color = GetNextColor();
        Population pop = new(values);
        PopulationSymbol sym = new(pop);
        sym.X = x;
        sym.Bar.FillColor = color;
        sym.Box.FillColor = Colors.Black.WithLightness(.8f);
        sym.Marker.MarkerLineColor = color;
        sym.Marker.MarkerFillColor = color;
        Plot.PlottableList.Add(sym);
        return sym;
    }

    public Radar Radar()
    {
        Radar radar = new();

        Plot.PlottableList.Add(radar);

        Plot.HideAxesAndGrid();
        return radar;
    }

    public Radar Radar(double[] values)
    {
        List<double[]> values2 = [values];

        return Radar(values2);
    }

    public Radar Radar(double[,] values)
    {
        List<double[]> valuesList = [];

        for (int i = 0; i < values.GetLength(0); i++)
        {
            double[] row = new double[values.GetLength(1)];
            for (int j = 0; j < row.Length; j++)
            {
                row[j] = values[i, j];
            }
            valuesList.Add(row);
        }

        return Radar(valuesList);
    }

    public Radar Radar(IEnumerable<IEnumerable<double>> series)
    {
        double spokeCount = series.First().Count();
        Radar radar = new();

        int seriesIndex = 0;
        foreach (var values in series)
        {
            Color color = Palette.GetColor(seriesIndex++);

            double[] valuesArray = values.ToArray();
            if (valuesArray.Length != spokeCount)
                throw new InvalidOperationException("Every collection in the series must have the same number of items");

            RadarSeries rs = new()
            {
                Values = valuesArray,
                FillColor = color.WithOpacity(0.5),
                LineColor = color.WithOpacity(1),
            };
            radar.Series.Add(rs);
        }

        double maxValue = series.Select(x => x.Max()).Max();
        radar.PolarAxis.SetCircles(maxValue, 4);

        radar.PolarAxis.SetSpokes(series.First().Count(), maxValue * 1.1, degreeLabels: false);

        Plot.PlottableList.Add(radar);

        Plot.HideAxesAndGrid();
        return radar;
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

    /// <summary>
    /// Create a bar plot to represent a collection of named ranges
    /// </summary>
    public BarPlot Ranges(List<(string name, CoordinateRange range)> ranges, Color? color = null, bool horizontal = false)
    {
        Color barColor = color ?? GetNextColor();

        // create a bar plot from the collection of ranges
        Bar[] bars = new Bar[ranges.Count];
        for (int i = 0; i < ranges.Count; i++)
        {
            bars[i] = new()
            {
                ValueBase = ranges[i].range.Min,
                Value = ranges[i].range.Max,
                Position = i,
                FillColor = barColor,
            };
        }
        BarPlot bp = Bars(bars);
        bp.Horizontal = horizontal;

        // use manaul tick labels displaying category names
        double[] positions = bars.Select(x => x.Position).ToArray();
        string[] labels = ranges.Select(x => x.name).ToArray();
        if (horizontal)
        {
            Plot.Axes.Left.SetTicks(positions, labels);
        }
        else
        {
            Plot.Axes.Bottom.SetTicks(positions, labels);
        }

        return bp;
    }

    public Rectangle Rectangle(CoordinateRect rect)
    {
        return Rectangle(rect.Left, rect.Right, rect.Top, rect.Bottom);
    }

    public Rectangle Rectangle(double left, double right, double bottom, double top)
    {
        Color color = GetNextColor();
        Rectangle rp = new()
        {
            X1 = left,
            X2 = right,
            Y1 = bottom,
            Y2 = top,
            LineColor = color,
            FillColor = color.WithAlpha(.5),
        };

        Plot.PlottableList.Add(rp);
        return rp;
    }

    public ScaleBar ScaleBar(double width, double height)
    {
        ScaleBar scalebar = new()
        {
            Width = width,
            Height = height,
        };

        Plot.PlottableList.Add(scalebar);
        return scalebar;
    }

    public Scatter Scatter(IScatterSource source, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        Scatter scatter = new(source)
        {
            LineColor = nextColor,
            MarkerFillColor = nextColor,
            MarkerLineColor = nextColor,
        };
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
        scatter.MarkerStyle.FillColor = nextColor;
        Plot.PlottableList.Add(scatter);
        return scatter;
    }

    public Scatter Scatter<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        ScatterSourceGenericList<T1, T2> source = new(xs, ys);
        Scatter scatter = new(source);
        scatter.LineStyle.Color = nextColor;
        scatter.MarkerStyle.FillColor = nextColor;
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
        return Signal(source, color);
    }

    public Signal Signal<T>(T[] ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericArray<T> source = new(ys, period);
        return Signal(source, color);
    }

    public Signal Signal<T>(IReadOnlyList<T> ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericList<T> source = new(ys, period);
        return Signal(source, color);
    }

    public Signal SignalConst<T>(T[] ys, double period = 1, Color? color = null)
        where T : struct, IComparable
    {
        SignalConstSource<T> source = new(ys, period);
        return Signal(source, color);
    }

    public SignalXY SignalXY(ISignalXYSource source, Color? color = null)
    {
        SignalXY sig = new(source)
        {
            Color = color ?? GetNextColor()
        };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalXY SignalXY(double[] xs, double[] ys, Color? color = null)
    {
        SignalXYSourceDoubleArray source = new(xs, ys);
        return SignalXY(source, color);
    }

    public SignalXY SignalXY<TX, TY>(TX[] xs, TY[] ys, Color? color = null)
    {
        var source = new SignalXYSourceGenericArray<TX, TY>(xs, ys);
        return SignalXY(source, color);
    }

    public SignalXY SignalXY<TX, TY>(IReadOnlyList<TX> xs, IReadOnlyList<TY> ys, Color? color = null)
    {
        SignalXYSourceGenericList<TX, TY> source = new(xs, ys);
        return SignalXY(source, color);
    }

    public SmithChartAxis SmithChartAxis()
    {
        SmithChartAxis smithChartAxis = new();

        double[] realTicks = [30, 5, 2, 1, 0.5, 0.2, 0];
        foreach (var position in realTicks)
        {
            var realPart = smithChartAxis.AddRealTick(position);
            if (position == 1 || position == 0)
            {
                realPart.LineStyle.Color = Colors.Black;
            }
        }

        double[] imaginaryTicks = [30, 5, 2, 1, 0.5, 0.2, 0.0, -0.2, -0.5, -1, -2, -5, -30];
        foreach (var position in imaginaryTicks)
        {
            var imaginaryPart = smithChartAxis.AddImaginaryTick(position);
            if (position == 0)
            {
                imaginaryPart.LineStyle.Color = Colors.Black;
            }
        }

        Plot.PlottableList.Add(smithChartAxis);

        Plot.HideAxesAndGrid();
        return smithChartAxis;
    }

    /// <summary>
    /// Place a stacked bar chart at a single position
    /// </summary>
    public BarPlot[] StackedRanges(List<(string name, double[] edgeValues)> ranges, IPalette? palette = null, bool horizontal = false)
    {
        BarPlot[] bps = new BarPlot[ranges.Count];
        for (int i = 0; i < ranges.Count; i++)
        {
            double[] edgeValues = ranges[i].edgeValues;
            Bar[] bars = new Bar[edgeValues.Length - 1];
            for (int j = 0; j < bars.Length; j++)
            {
                bars[j] = new()
                {
                    ValueBase = edgeValues[j],
                    Value = edgeValues[j + 1],
                    Position = i,
                    FillColor = (palette ?? Palette).GetColor(j),
                };
            }

            bps[i] = Bars(bars);
            bps[i].Horizontal = horizontal;
        }

        string[] labels = ranges.Select(x => x.name).ToArray();
        double[] positions = Generate.Consecutive(labels.Length);
        if (horizontal)
        {
            Plot.Axes.Left.SetTicks(positions, labels);
        }
        else
        {
            Plot.Axes.Bottom.SetTicks(positions, labels);
        }

        return bps;
    }

    public Text Text(string text, Coordinates location)
    {
        return Text(text, location.X, location.Y);
    }

    public Text Text(string text, double x, double y)
    {
        Text txt = new()
        {
            LabelText = text ?? string.Empty,
            LabelBackgroundColor = Colors.Transparent,
            LabelBorderColor = Colors.Transparent,
            Location = new(x, y)
        };
        Plot.PlottableList.Add(txt);

        return txt;
    }

    public TriangularAxis TriangularAxis(bool clockwise = true, bool hideAxisAndGrid = true, bool useSquareAxisUnits = true)
    {
        TriangularAxis ta = new(clockwise);
        Plot.PlottableList.Add(ta);

        if (hideAxisAndGrid)
            Plot.HideAxesAndGrid();

        if (useSquareAxisUnits)
            Plot.Axes.SquareUnits();

        return ta;
    }

    public VectorField VectorField(IList<RootedCoordinateVector> vectors, Color? color = null)
    {
        VectorFieldDataSourceCoordinatesList vs = new(vectors);
        VectorField field = new(vs);
        field.ArrowStyle.LineStyle.Color = color ?? GetNextColor();
        field.ArrowStyle.LineStyle.Width = 2;
        Plot.PlottableList.Add(field);
        return field;
    }

    public VerticalLine VerticalLine(double x, float width = 2, Color? color = null, LinePattern pattern = default)
    {
        Color color2 = color ?? GetNextColor();
        VerticalLine line = new()
        {
            LineWidth = width,
            LineColor = color2,
            LabelBackgroundColor = color2,
            LinePattern = pattern,
            X = x
        };
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
