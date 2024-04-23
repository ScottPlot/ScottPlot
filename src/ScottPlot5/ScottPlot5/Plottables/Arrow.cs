namespace ScottPlot.Plottables;

public class Arrow : IPlottable, IHasArrow, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => [new LegendItem() { ArrowStyle = ArrowStyle, LabelText = LegendText }];

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }

    public string LegendText { get; set; } = string.Empty;

    /// <summary>
    /// ImagePosition of the base of the arrow in coordinate units
    /// </summary>
    public Coordinates Base { get; set; } = Coordinates.Origin;

    /// <summary>
    /// ImagePosition of the base of the arrowhead in coordinate units
    /// </summary>
    public Coordinates Tip { get; set; } = Coordinates.Origin;

    /// <summary>
    /// Advanced styling options
    /// </summary>
    public LineStyle LineStyle { get => ArrowStyle.LineStyle; set => ArrowStyle.LineStyle = value; }

    /// <summary>
    /// Color of the arrow line and arrowhead
    /// </summary>
    public Color Color { get => ArrowColor; set => ArrowColor = value; }

    /// <summary>
    /// Thickness of the line at the base of the arrow
    /// </summary>
    public float LineWidth { get => ArrowLineWidth; set => ArrowLineWidth = value; }

    /// <summary>
    /// Total width of the arrowhead in pixels
    /// </summary>
    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    /// <summary>
    /// Length of the arrowhead in pixels
    /// </summary>
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }

    /// <summary>
    /// The base of the arrow will be expanded away from the tip so its length is always at least this number of pixels
    /// </summary>
    public float MinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }

    /// <summary>
    /// Back the arrow away from its tip along its axis by this many pixels
    /// </summary>
    public float Offset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }

    public ArrowStyle ArrowStyle { get; set; } = new() { LineWidth = 2, LineColor = Colors.Black };
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }

    public AxisLimits GetAxisLimits() => new(
        Math.Min(Base.X, Tip.X),
        Math.Max(Base.X, Tip.X),
        Math.Min(Base.Y, Tip.Y),
        Math.Max(Base.Y, Tip.Y));

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible) { return; }

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        var px_base = Axes.GetPixel(Base);
        var px_tip = Axes.GetPixel(Tip);
        var dist0 = CalcDistance_(ref px_tip, ref px_base);
        var dist = dist0;

        double angle;
        SKPoint skpt_tip_offset;

        // Line
        {
            Pixel px_edge_base, px_edge_tip;
            if (dist < 1)
            {
                // To avoid getting incorrect angle when zooming far out, extend both base and tip to edges.

                var coord_edge_base = Coordinates.NaN;
                var coord_edge_tip = Coordinates.NaN;

                var x_dif = Base.X - Tip.X;
                var y_dif = Base.Y - Tip.Y;

                // Dot product
                var dp1 = x_dif * (Axes.XAxis.Max - Axes.XAxis.Min) + y_dif * (Axes.YAxis.Max - Axes.YAxis.Min);
                var dp2 = x_dif * (Axes.XAxis.Min - Axes.XAxis.Max) + y_dif * (Axes.YAxis.Max - Axes.YAxis.Min);

                if (dp1 > 0)
                {
                    if (dp2 > 0) // Top
                    {
                        coord_edge_base.X = Tip.X + x_dif * (Axes.YAxis.Max - Tip.Y) / y_dif;
                        coord_edge_base.Y = Axes.YAxis.Max;

                        coord_edge_tip.X = Tip.X - x_dif * (Tip.Y - Axes.YAxis.Min) / y_dif;
                        coord_edge_tip.Y = Axes.YAxis.Min;
                    }
                    else // Right
                    {
                        coord_edge_base.X = Axes.XAxis.Max;
                        coord_edge_base.Y = Tip.Y + y_dif * (Axes.XAxis.Max - Tip.X) / x_dif;

                        coord_edge_tip.X = Axes.XAxis.Min;
                        coord_edge_tip.Y = Tip.Y - y_dif * (Tip.X - Axes.XAxis.Min) / x_dif;
                    }
                }
                else
                {
                    if (dp2 > 0) // Left
                    {
                        coord_edge_base.X = Axes.XAxis.Min;
                        coord_edge_base.Y = Tip.Y - y_dif * (Tip.X - Axes.XAxis.Min) / x_dif;

                        coord_edge_tip.X = Axes.XAxis.Max;
                        coord_edge_tip.Y = Tip.Y + y_dif * (Axes.XAxis.Max - Tip.X) / x_dif;
                    }
                    else // Bottom
                    {
                        coord_edge_base.X = Tip.X - x_dif * (Tip.Y - Axes.YAxis.Min) / y_dif;
                        coord_edge_base.Y = Axes.YAxis.Min;

                        coord_edge_tip.X = Tip.X + x_dif * (Axes.YAxis.Max - Tip.Y) / y_dif;
                        coord_edge_tip.Y = Axes.YAxis.Max;
                    }
                }

                px_edge_base = Axes.GetPixel(coord_edge_base);
                px_edge_tip = Axes.GetPixel(coord_edge_tip);
            }
            else
            {
                px_edge_base = px_base;
                px_edge_tip = px_tip;
            }

            angle = Math.Atan2(px_edge_tip.Y - px_edge_base.Y, px_edge_tip.X - px_edge_base.X);

            if (Offset == 0)
            {
                skpt_tip_offset = px_tip.ToSKPoint();
            }
            else
            {
                skpt_tip_offset = Rotate_(px_tip.X - Offset, px_tip.Y, px_tip.X, px_tip.Y, angle);
                dist -= Offset;
            }

            SKPoint skpt_base_extended;
            if (dist < MinimumLength)
            {
                var m = MinimumLength / CalcDistance_(ref px_edge_base, ref px_edge_tip);
                skpt_base_extended = new(
                    skpt_tip_offset.X - (px_edge_tip.X - px_edge_base.X) * m,
                    skpt_tip_offset.Y - (px_edge_tip.Y - px_edge_base.Y) * m);
                dist = MinimumLength;
            }
            else
            {
                skpt_base_extended = px_base.ToSKPoint();
            }

            if (dist - ArrowheadLength >= 1)
            {
                var skpt_head_bottom = Rotate_(
                    skpt_tip_offset.X - ArrowheadLength,
                    skpt_tip_offset.Y,
                    skpt_tip_offset.X,
                    skpt_tip_offset.Y,
                    angle);
                rp.Canvas.DrawLine(skpt_base_extended, skpt_head_bottom, paint);
            }
        }

        // Head
        if (ArrowheadLength >= 1)
        {
            using SKPath path = new();
            path.MoveTo(skpt_tip_offset);
            path.LineTo(Rotate_(
                skpt_tip_offset.X - ArrowheadLength - 1,
                skpt_tip_offset.Y + ArrowheadWidth / 2,
                skpt_tip_offset.X,
                skpt_tip_offset.Y,
                angle));
            path.LineTo(Rotate_(
                skpt_tip_offset.X - ArrowheadLength - 1,
                skpt_tip_offset.Y - ArrowheadWidth / 2,
                skpt_tip_offset.X,
                skpt_tip_offset.Y,
                angle));
            path.LineTo(skpt_tip_offset);

            paint.Style = SKPaintStyle.Fill;
            rp.Canvas.DrawPath(path, paint);
        }

        static float CalcDistance_(ref Pixel px1, ref Pixel px2)
            => (float)Math.Sqrt(Math.Pow(px1.X - px2.X, 2) + Math.Pow(px1.Y - px2.Y, 2));

        static SKPoint Rotate_(float x, float y, float xCenter, float yCenter, double angleRadians)
        {
            var sin = Math.Sin(angleRadians);
            var cos = Math.Cos(angleRadians);
            var dx = x - xCenter;
            var dy = y - yCenter;

            return new SKPoint((float)(dx * cos - dy * sin + xCenter), (float)(dy * cos + dx * sin + yCenter));
        }
    }
}
