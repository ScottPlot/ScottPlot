namespace ScottPlot.Plottables;

public class PhasorLinesPlot(IEnumerable<PolarCoordinates> points, IEnumerable<string>? lineNames = null) :
    IPlottable, IHasLine, IHasMarker, IHasLegendText
{
    public IEnumerable<string>? LineNames { get; } = lineNames;
    public IEnumerable<PolarCoordinates> Points { get; } = points;

    public readonly LabelStyle LabelStyle = new();

    /// <summary>
    /// Additional padding given to accommodate labels
    /// </summary>
    public double PaddingFraction { get; set; } = 0.01;

    /// <summary>
    /// Additional padding given to accommodate labels
    /// </summary>
    public double PaddingArc { get; set; } = 5;

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 12 };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle);

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
        }
    }

    public AxisLimits GetAxisLimits()
    {
        IEnumerable<Coordinates> pts =
             Points?.Select(i => i.CartesianCoordinates) ?? [];
        pts = pts.Concat([Coordinates.Origin]);

        return new AxisLimits(
            pts.Min(i => i.X),
            pts.Max(i => i.X),
            pts.Max(i => i.Y),
            pts.Min(i => i.Y));
    }

    public virtual void Render(RenderPack rp)
    {
        if (Points is null ||
            !Points.Any())
        {
            return;
        }

        using SKPaint paint = new();
        for (int i = 0; i < Points.Count(); i++)
        {
            PolarCoordinates point = Points.ElementAt(i);
            Coordinates pt = point.CartesianCoordinates;
            CoordinateLine line = new(Coordinates.Origin, pt);
            PixelLine pxLine = Axes.GetPixelLine(line);

            MarkerStyle.Rotate = Angle.FromDegrees(-point.Angle.Degrees + 90);
            Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(pt), MarkerStyle);
            Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);

            if (LineNames is not null &&
                i < LineNames.Count())
            {
                LabelStyle.Text = LineNames.ElementAt(i);

                var angle = point.Radius > 0
                    ? Angle.FromRadians(point.Angle.Radians + PaddingArc / point.Radius)
                    : Angle.FromRadians(point.Angle.Radians);

                var padding = Math.Min(Axes.XAxis.Range.Span, Axes.YAxis.Range.Span) * PaddingFraction;
                PolarCoordinates labelPoint = new(point.Radius + padding, angle);
                Pixel labelPixel = Axes.GetPixel(labelPoint.CartesianCoordinates);
                PixelRect labelRect = LabelStyle.Measure().Rect(Alignment.MiddleCenter);
                Pixel labelOffset = labelRect.Center - labelRect.TopLeft;
                labelPixel -= labelOffset;
                LabelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }
    }
}
