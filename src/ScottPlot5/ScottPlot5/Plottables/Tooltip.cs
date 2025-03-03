namespace ScottPlot.Plottables;

/// <summary>
/// A tooltip displays a text bubble pointing to a specific location in X/Y space.
/// The position of the bubble moves according to the axis limits to best display the text in the data area.
/// </summary>
public class Tooltip : Text, IPlottable, IHasLine, IHasFill
{
    public Coordinates TargetPoint { get; set; }

    private float _TailWidthPercentage = 0.5F;
    public float TailWidthPercentage { get => _TailWidthPercentage; set => _TailWidthPercentage = value > 1.0F ? 1.0F : value; }
    public float Radius { get; set; } = 5;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public override AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Location)
            .Expanded(TargetPoint);
    }

    private static bool GetPerpendicularPoints(
        SKPoint a, SKPoint b, float length, out SKPoint c, out SKPoint d)
    {
        c = default;
        d = default;

        SKPoint dPt = b - a;
        float abLength = (float)Math.Sqrt(dPt.X * dPt.X + dPt.Y * dPt.Y);
        if (abLength <= 0)
        {
            return false;
        }

        float unitX = -dPt.Y / abLength;
        float unitY = dPt.X / abLength;
        float halfLength = length / 2.0F;
        c = new(b.X + halfLength * unitX, b.Y + halfLength * unitY);
        d = new(b.X - halfLength * unitX, b.Y - halfLength * unitY);
        return true;
    }

    protected virtual void RenderTooltipShape(RenderPack rp)
    {
        using SKPaint paint = new();

        MeasuredText measured = LabelStyle.Measure(LabelStyle.Text, paint);
        PixelRect bubbleBodyRect = measured.Rect(Alignment)
            .Expand(new PixelPadding(
                Radius + LabelPixelPadding.Left,
                Radius + LabelPixelPadding.Right,
                Radius + LabelPixelPadding.Bottom,
                Radius + LabelPixelPadding.Top));

        Pixel px = Axes.GetPixel(Location);
        bubbleBodyRect = bubbleBodyRect.WithOffset(new(px.X, px.Y));

        using SKPath bubbleBodyPath = new();
        bubbleBodyPath.AddRoundRect(new(bubbleBodyRect.ToSKRect(), Radius));

        var targetPoint = Axes.GetPixel(TargetPoint).ToSKPoint();
        float tailWidth = Math.Min(bubbleBodyRect.Width, bubbleBodyRect.Height)
            * TailWidthPercentage;
        if (!GetPerpendicularPoints(
                targetPoint,
                bubbleBodyRect.Center.ToSKPoint(),
                tailWidth,
                out SKPoint tailPt1,
                out SKPoint tailPt2))
        {
            Drawing.FillPath(rp.Canvas, paint, bubbleBodyPath, FillStyle);
            Drawing.DrawPath(rp.Canvas, paint, bubbleBodyPath, LineStyle);
            return;
        }

        using SKPath bubbleTailPath = new();
        bubbleTailPath.AddPoly([targetPoint, tailPt1, tailPt2], true);

        using SKPath tooltipShapePath = bubbleBodyPath.Op(bubbleTailPath, SKPathOp.Union);
        Drawing.FillPath(rp.Canvas, paint, tooltipShapePath, FillStyle);
        Drawing.DrawPath(rp.Canvas, paint, tooltipShapePath, LineStyle);
    }

    public override void Render(RenderPack rp)
    {
        RenderTooltipShape(rp);
        base.Render(rp);
    }
}
