namespace ScottPlot.Plottables;

/// <summary>
/// A polygon is a collection of X/Y points that are all connected to form a closed shape.
/// Polygons can be optionally filled with a color or a gradient.
/// </summary>
public class Polygon : IPlottable, IHasLine, IHasFill, IHasMarker, IHasLegendText
{
    public static Polygon Empty => new();

    public bool IsEmpty { get; private set; } = false;

    // TODO: replace with a generic data source
    public Coordinates[] Coordinates { get; private set; } = Array.Empty<Coordinates>();

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public bool IsVisible { get; set; } = true;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public MarkerStyle MarkerStyle { get; set; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }


    public int PointCount { get => Coordinates.Length; }

    public IAxes Axes { get; set; } = new Axes();

    private AxisLimits limits;

    public IEnumerable<LegendItem> LegendItems => [new LegendItem()
    {
        LabelText = LegendText,
        FillStyle = FillStyle,
        OutlineStyle = LineStyle,
        MarkerStyle = MarkerStyle,
    }];

    private Polygon()
    {
        Coordinates = new Coordinates[0];
        IsEmpty = true;
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="coords">The axis dependant vertex coordinates.</param>
    public Polygon(Coordinates[] coords)
    {
        UpdateCoordinates(coords);
    }

    public override string ToString()
    {
        string label = string.IsNullOrWhiteSpace(LegendText) ? "" : $" ({LegendText})";
        return $"PlottablePolygon{label} with {PointCount} points";
    }

    public void UpdateCoordinates(Coordinates[] newCoordinates)
    {
        Coordinates = newCoordinates;

        limits = AxisLimits.NoLimits;
        IsEmpty = !Coordinates.Any();
        if (IsEmpty) return;

        double xMin = Coordinates[0].X;
        double xMax = Coordinates[0].X;
        double yMin = Coordinates[0].Y;
        double yMax = Coordinates[0].Y;

        foreach (var coord in Coordinates)
        {
            if (coord.X > xMax) xMax = coord.X;
            if (coord.X < xMin) xMin = coord.X;
            if (coord.Y > yMax) yMax = coord.Y;
            if (coord.Y < yMin) yMin = coord.Y;
        }

        limits = new AxisLimits(xMin, xMax, yMin, yMax);
    }

    public AxisLimits GetAxisLimits()
    {
        return limits;
    }

    public virtual void Render(RenderPack rp)
    {
        if (IsEmpty)
            return;

        bool close = true; // TODO: make property
        var coordinates = close
            ? Coordinates.Concat(new[] { Coordinates.First() })
            : Coordinates;
        IEnumerable<Pixel> pixels = coordinates.Select(Axes.GetPixel);

        // TODO: stop using skia primitives directly
        IEnumerable<SKPoint> skPoints = pixels.Select(x => x.ToSKPoint());
        using SKPath path = new();
        SKPoint firstPoint = skPoints.First();
        path.MoveTo(firstPoint);
        float xMax, xMin, yMax, yMin;
        xMax = xMin = firstPoint.X;
        yMax = yMin = firstPoint.Y;
        foreach (SKPoint p in skPoints.Skip(1))
        {
            xMax = Math.Max(xMax, p.X);
            xMin = Math.Min(xMin, p.X);
            yMax = Math.Max(yMax, p.Y);
            yMin = Math.Min(yMin, p.Y);
            path.LineTo(p);
        }

        using var paint = new SKPaint();
        if (FillStyle.HasValue)
        {
            FillStyle.ApplyToPaint(paint, new PixelRect(xMin, xMax, yMin, yMax));
            paint.Style = SKPaintStyle.Fill;
            rp.Canvas.DrawPath(path, paint);
        }

        if (LineStyle is { IsVisible: true, Width: > 0 })
        {
            paint.Style = SKPaintStyle.Stroke;
            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);
            Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        }

        if (MarkerStyle.IsVisible)
        {
            Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
        }
    }
}
