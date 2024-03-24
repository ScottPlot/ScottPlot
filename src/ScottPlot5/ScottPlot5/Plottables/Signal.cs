/* Minimal case signal plot for testing only
 * !! Avoid temptation to use generics or generic math at this early stage of development
 */

namespace ScottPlot.Plottables;

public class Signal : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public readonly ISignalSource Data;

    public string? Label { get; set; }

    public readonly MarkerStyle Marker;

    public readonly LineStyle LineStyle;

    /// <summary>
    /// Maximum size of the marker (in pixels) to display
    /// at each data point when the plot is zoomed far in.
    /// </summary>
    public float MaximumMarkerSize { get; set; } = 4;

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            Marker.Fill.Color = value;
            Marker.Outline.Color = value;
        }
    }

    public Signal(ISignalSource data)
    {
        Data = data;

        Marker = new(MarkerShape.FilledCircle, 5)
        {
            Outline = LineStyle.None
        };

        LineStyle = new();
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
        new LegendItem
        {
            Label = Label,
            Marker = Marker,
            Line = LineStyle,
        });

    private CoordinateRange GetVisibleXRange(PixelRect dataRect)
    {
        // TODO: put GetRange in axis translator
        double xViewLeft = Axes.GetCoordinateX(dataRect.Left);
        double xViewRight = Axes.GetCoordinateX(dataRect.Right);
        return new CoordinateRange(xViewLeft, xViewRight);
    }

    private double PointsPerPixel()
    {
        return GetVisibleXRange(Axes.DataRect).Span / Axes.DataRect.Width / Data.Period;
    }

    public void Render(RenderPack rp)
    {
        if (PointsPerPixel() < 1)
        {
            RenderLowDensity(rp);
        }
        else
        {
            RenderHighDensity(rp);
        }
    }

    /// <summary>
    /// Renders each point connected by a single line, like a scatter plot.
    /// Call this when zoomed in enough that no pixel could contain two points.
    /// </summary>
    private void RenderLowDensity(RenderPack rp)
    {
        CoordinateRange visibleXRange = GetVisibleXRange(Axes.DataRect);
        int i1 = Data.GetIndex(visibleXRange.Min, true);
        int i2 = Data.GetIndex(visibleXRange.Max + Data.Period, true);

        List<Pixel> points = [];

        for (int i = i1; i <= i2; i++)
        {
            float x = Axes.GetPixelX(Data.GetX(i));
            float y = Axes.GetPixelY(Data.GetY(i) + Data.YOffset);
            Pixel px = new(x, y);
            points.Add(px);
        }

        using SKPath path = new();
        path.MoveTo(points[0].ToSKPoint());
        foreach (Pixel point in points)
            path.LineTo(point.ToSKPoint());

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        rp.Canvas.DrawPath(path, paint);

        double pointsPerPx = PointsPerPixel();

        if (pointsPerPx < 1)
        {
            paint.IsStroke = false;
            float radius = (float)Math.Min(Math.Sqrt(.2 / pointsPerPx), MaximumMarkerSize);
            Marker.Size = radius * MaximumMarkerSize * .2f;
            Drawing.DrawMarkers(rp.Canvas, paint, points, Marker);
        }
    }

    /// <summary>
    /// Renders the plot by filling-in pixel columns according the extremes of Y data ranges.
    /// Call this when zoomed out enough that one X pixel column may contain two or more points.
    /// </summary>
    private void RenderHighDensity(RenderPack rp)
    {
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        IEnumerable<PixelColumn> cols = Enumerable.Range(0, (int)Axes.DataRect.Width)
            .Select(x => Data.GetPixelColumn(Axes, x))
            .Where(x => x.HasData);

        if (!cols.Any())
            return;

        using SKPath path = new();
        path.MoveTo(cols.First().X, cols.First().Enter);

        foreach (PixelColumn col in cols)
        {
            path.LineTo(col.X, col.Enter);
            path.MoveTo(col.X, col.Bottom);
            path.LineTo(col.X, col.Top);
            path.MoveTo(col.X, col.Exit);
        }

        rp.Canvas.DrawPath(path, paint);
    }
}
