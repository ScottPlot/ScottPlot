namespace ScottPlot.Plottables;

public class Signal(ISignalSource data) : IPlottable, IHasLine, IHasMarker, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public readonly ISignalSource Data = data;

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 5, Shape = MarkerShape.FilledCircle };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public int MinRenderIndex { get => Data.MinimumIndex; set => Data.MinimumIndex = value; }
    public int MaxRenderIndex { get => Data.MaximumIndex; set => Data.MaximumIndex = value; }

    /// <summary>
    /// Maximum size of the marker (in pixels) to display
    /// at each data point when the plot is zoomed far in.
    /// </summary>
    public float MaximumMarkerSize { get; set; } = 4;

    public Color Color
    {
        get => LineColor;
        set
        {
            LineColor = value;
            MarkerFillColor = value;
            MarkerLineColor = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle, LineStyle);

    private CoordinateRange GetVisibleXRange(PixelRect dataRect)
    {
        // TODO: put GetRange in axis translator
        double xViewLeft = Axes.GetCoordinateX(dataRect.Left);
        double xViewRight = Axes.GetCoordinateX(dataRect.Right);
        return (xViewLeft <= xViewRight)
            ? new CoordinateRange(xViewLeft, xViewRight)
            : new CoordinateRange(xViewRight, xViewLeft);
    }

    private double PointsPerPixel()
    {
        return GetVisibleXRange(Axes.DataRect).Span / Axes.DataRect.Width / Data.Period;
    }

    public virtual void Render(RenderPack rp)
    {
        if (!Data.GetYs().Any())
        {
            return;
        }

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
            float y = Axes.GetPixelY(Data.GetY(i) * Data.YScale + Data.YOffset);
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
            MarkerSize = radius * MaximumMarkerSize * .2f;
            Drawing.DrawMarkers(rp.Canvas, paint, points, MarkerStyle);
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
