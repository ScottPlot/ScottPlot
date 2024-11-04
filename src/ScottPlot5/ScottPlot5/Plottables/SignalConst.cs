using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConst<T>(T[] ys, double period) : IPlottable, IHasLine, IHasMarker, IHasLegendText
    where T : struct, IComparable
{
    public SignalConstSource<T> Data { get; } = new(ys, period);

    public MarkerStyle MarkerStyle { get; set; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }
    /// <summary>
    /// Maximum size of the marker (in pixels) to display
    /// at each data point when the plot is zoomed far in.
    /// </summary>
    public float MaximumMarkerSize { get; set; } = 4;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public int MinRenderIndex { get => Data.MinRenderIndex; set => Data.MinRenderIndex = value; }
    public int MaxRenderIndex { get => Data.MaxRenderIndex; set => Data.MaxRenderIndex = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineColor = value;
            MarkerFillColor = value;
            MarkerLineColor = value;
        }
    }

    /// <summary>
    /// Setting this flag causes lines to be drawn between every visible point
    /// (similar to scatter plots) to improve anti-aliasing in static images.
    /// Setting this will decrease performance for large datasets 
    /// and is not recommended for interactive environments.
    /// </summary>
    public bool AlwaysUseLowDensityMode { get; set; } = false;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle, LineStyle);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        List<PixelColumn> cols = Data.GetPixelColumns(Axes);
        List<Pixel> points = [];
        double pointsPerPx = PointsPerPixel();
        bool useLowDensityMode = pointsPerPx < 1 || AlwaysUseLowDensityMode;

        if (!cols.Any())
            return;

        using SKPath path = new();
        path.MoveTo(cols.First().X, cols.First().Enter);

        foreach (PixelColumn col in cols)
        {
            path.LineTo(col.X, col.Enter);
            if ((int)col.Top != (int)col.Bottom)
            {
                path.MoveTo(col.X, col.Bottom);
                path.LineTo(col.X, col.Top);
                path.MoveTo(col.X, col.Exit);
            }
            if (useLowDensityMode)
            {
                points.Add(new(col.X, col.Enter));
            }
        }

        rp.Canvas.DrawPath(path, paint);

        if (useLowDensityMode)
        {
            paint.IsStroke = false;
            float radius = (float)Math.Min(Math.Sqrt(.2 / pointsPerPx), MaximumMarkerSize);
            MarkerSize = radius * MaximumMarkerSize * .2f;
            Drawing.DrawMarkers(rp.Canvas, paint, points, MarkerStyle);
        }

    }

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
}
