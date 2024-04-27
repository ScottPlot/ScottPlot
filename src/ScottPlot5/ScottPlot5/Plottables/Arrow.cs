namespace ScottPlot.Plottables;

public class Arrow : IPlottable, IHasArrow, IHasLegendText
{
    public Coordinates Base { get; set; } = Coordinates.Origin;
    public Coordinates Tip { get; set; } = Coordinates.Origin;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => [new LegendItem() { ArrowStyle = ArrowStyle, LabelText = LegendText }];
    public string LegendText { get; set; } = string.Empty;

    public ArrowStyle ArrowStyle { get; set; } = new() { LineWidth = 2, LineColor = Colors.Black };
    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }
    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }
    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }
    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }

    #region obsolete

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }

    [Obsolete("use ArrowLineColor or ArrowFillColor", true)]
    public Color ArrowColor { get; set; }

    [Obsolete("use ArrowLineColor or ArrowFillColor", true)]
    public Color Color { get; set; }

    [Obsolete("use ArrowOffset", true)]
    public float Offset { get; set; }

    [Obsolete("use ArrowMinimumLength", true)]
    public float MinimumLength { get; set; }

    [Obsolete("use ArrowLineWidth", true)]
    public float LineWidth { get; set; }

    [Obsolete("use ArrowLineWidth, ArrowLineColor, or ArrowStyle.LineStyle", true)]
    public LineStyle LineStyle { get => ArrowStyle.LineStyle; set => ArrowStyle.LineStyle = value; }

    #endregion


    public AxisLimits GetAxisLimits() => new(
        Math.Min(Base.X, Tip.X),
        Math.Max(Base.X, Tip.X),
        Math.Min(Base.Y, Tip.Y),
        Math.Max(Base.Y, Tip.Y));

    public IArrow ArrowRenderer { get; set; } = new Arrows.Single();

    public virtual void Render(RenderPack rp)
    {
        Pixel pxBase = Axes.GetPixel(Base);
        Pixel pxTip = Axes.GetPixel(Tip);
        ArrowRenderer.Render(rp, pxBase, pxTip, ArrowStyle);
    }

    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }

    public virtual void Render_OLD(RenderPack rp)
    {
        if (!IsVisible) { return; }

        using SKPaint paint = new();
        ArrowStyle.LineStyle.ApplyToPaint(paint);

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

            if (ArrowOffset == 0)
            {
                skpt_tip_offset = px_tip.ToSKPoint();
            }
            else
            {
                skpt_tip_offset = Rotate_(px_tip.X - ArrowOffset, px_tip.Y, px_tip.X, px_tip.Y, angle);
                dist -= ArrowOffset;
            }

            SKPoint skpt_base_extended;
            if (dist < ArrowMinimumLength)
            {
                var m = ArrowMinimumLength / CalcDistance_(ref px_edge_base, ref px_edge_tip);
                skpt_base_extended = new(
                    skpt_tip_offset.X - (px_edge_tip.X - px_edge_base.X) * m,
                    skpt_tip_offset.Y - (px_edge_tip.Y - px_edge_base.Y) * m);
                dist = ArrowMinimumLength;
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
