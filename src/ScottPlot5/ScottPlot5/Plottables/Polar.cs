namespace ScottPlot.Plottables;

public class Polar :
    PolarBase, IHasMarker, IHasLegendText
{
    public string LegendText { get; set; } = string.Empty;

    public override IEnumerable<LegendItem> LegendItems
        => LegendItem.Single(LegendText, MarkerStyle);

    public MarkerStyle MarkerStyle { get; set; } = new()
    {
        LineWidth = 1,
        Size = 5,
        Shape = MarkerShape.FilledCircle,
    };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public override IRadialAxis RadialAxis { get; }
    public override ICircularAxis CircularAxis { get; }
    public IReadOnlyList<PolarCoordinates> DataPoints { get; }

    public Polar(IEnumerable<PolarCoordinates> dataPoints)
    {
        DataPoints = (dataPoints is null)
            ? []
            : [.. dataPoints.OrderBy(i => Angle.NormalizeDegrees(i.Angle))];

        double maxRadius = 1.0;
        if (DataPoints.Count > 0)
        {
            maxRadius = DataPoints.Max(i => i.Radial);
        }

        RadialAxis = new PolarAxes.RadialAxis(maxRadius);
        CircularAxis = new PolarAxes.CircularAxis(
            Enumerable.Range(1, 4).Select(i => maxRadius * i / 4.0));
    }

    public override AxisLimits GetAxisLimits()
    {
        AxisLimits limit = base.GetAxisLimits();
        if (DataPoints.Count < 1)
        {
            return limit;
        }

        IEnumerable<Coordinates> pts = DataPoints.Select(i => (Coordinates)i);
        return new AxisLimits(
            Math.Min(pts.Min(i => i.X), limit.Left),
            Math.Max(pts.Max(i => i.X), limit.Right),
            Math.Max(pts.Max(i => i.Y), limit.Bottom),
            Math.Min(pts.Min(i => i.Y), limit.Top));
    }

    public override void Render(RenderPack rp)
    {
        base.Render(rp);

        if (DataPoints.Count < 1)
        {
            return;
        }

        using SKPaint paint = new();
        using SKAutoCanvasRestore _ = new(rp.Canvas);

        Pixel origin = Axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        var markerPixels = new Pixel[DataPoints.Count];
        for (int i = 0; i < DataPoints.Count; i++)
        {
            markerPixels[i] = Axes.GetPixel(DataPoints[i]) - origin;
        }

        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }
}
