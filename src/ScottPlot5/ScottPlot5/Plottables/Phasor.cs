namespace ScottPlot.Plottables;

/// <summary>
/// A Phasor plot marks a collection of points in polar space using an 
/// arrow with its base centered at the origin.
/// </summary>
public class Phasor : IPlottable, IHasArrow, IHasLegendText
{
    /// <summary>
    /// A collection of points in polar space
    /// </summary>
    public List<PolarCoordinates> Points { get; } = [];

    /// <summary>
    /// If populated, <see cref="Points"/> will be labeled with these strings
    /// </summary>
    public List<string> Labels { get; } = [];

    /// <summary>
    /// Style for arrow labels defined in <see cref="Labels"/>
    /// </summary>
    public LabelStyle LabelStyle { get; } = new();

    /// <summary>
    /// Additional padding given to accommodate spoke labels
    /// </summary>
    public double PaddingFraction { get; set; } = 0.01;

    /// <summary>
    /// Additional padding given to accommodate labels
    /// </summary>
    public double PaddingArc { get; set; } = 5;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => [new LegendItem() { LineStyle = ArrowStyle.LineStyle, LabelText = LegendText }];
    public string LegendText { get; set; } = string.Empty;

    public ArrowStyle ArrowStyle { get; set; } = new() { LineWidth = 2, ArrowWidth = 3 };
    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }
    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }
    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }
    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public float ArrowWidth { get => ArrowStyle.ArrowWidth; set => ArrowStyle.ArrowWidth = value; }
    public float ArrowheadAxisLength { get => ArrowStyle.ArrowheadAxisLength; set => ArrowStyle.ArrowheadAxisLength = value; }
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }
    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    public IArrowShape ArrowShape { get; set; } = new ArrowShapes.Single();

    public AxisLimits GetAxisLimits()
    {
        if (Points.Count == 0)
        {
            return AxisLimits.NoLimits;
        }

        IEnumerable<Coordinates> points = Points
            .Select(i => i.ToCartesian())
            .Concat([Coordinates.Origin]);

        return new AxisLimits(points);
    }

    public virtual void Render(RenderPack rp)
    {
        if (Points.Count == 0)
        {
            return;
        }

        using SKPaint paint = new();

        Pixel pxBase = Axes.GetPixel(Coordinates.Origin);

        for (int i = 0; i < Points.Count; i++)
        {
            PolarCoordinates point = Points[i];

            Pixel pxTip = Axes.GetPixel(point.ToCartesian());
            PixelLine pxLine = new(pxBase, pxTip);
            if (ArrowOffset != 0)
            {
                pxLine = pxLine.BackedUpBy(ArrowOffset);
            }
            ArrowShape.Render(rp, pxLine, ArrowStyle);

            if (i < Labels.Count)
            {
                LabelStyle.Text = Labels[i];

                Angle angle = point.Radius > 0
                    ? Angle.FromRadians(point.Angle.Radians + PaddingArc / point.Radius)
                    : Angle.FromRadians(point.Angle.Radians);

                double padding = Math.Min(Axes.XAxis.Range.Span, Axes.YAxis.Range.Span) * PaddingFraction;
                PolarCoordinates labelPoint = new(point.Radius + padding, angle);
                Pixel labelPixel = Axes.GetPixel(labelPoint.ToCartesian());
                PixelRect labelRect = LabelStyle.Measure().Rect(Alignment.MiddleCenter);
                Pixel labelOffset = labelRect.Center - labelRect.TopLeft;
                labelPixel -= labelOffset;
                LabelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }
    }
}
