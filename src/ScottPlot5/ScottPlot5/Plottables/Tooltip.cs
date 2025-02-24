namespace ScottPlot.Plottables;

/// <summary>
/// A tooltip displays a text bubble pointing to a specific location in X/Y space.
/// The position of the bubble moves according to the axis limits to best display the text in the data area.
/// </summary>
public class Tooltip : Text, IPlottable, IHasLine, IHasFill
{
    public Coordinates TailPoint { get; set; }

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
            .Expanded(TailPoint);
    }

    private static (SKPoint, SKPoint) GetTailBasePoints(SKPoint a, SKPoint b, float length)
    {
        SKPoint dPt = b - a;
        float unitX = -dPt.Y / dPt.Length;
        float unitY = dPt.X / dPt.Length;
        float halfLength = length / 2.0F;
        return (new(b.X + halfLength * unitX, b.Y + halfLength * unitY),
                new(b.X - halfLength * unitX, b.Y - halfLength * unitY));
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

        var tailPoint = Axes.GetPixel(TailPoint).ToSKPoint();
        float tailWidth = Math.Min(bubbleBodyRect.Width, bubbleBodyRect.Height)
            * TailWidthPercentage;

        SKPath getTooltipShapePath()
        {
            var bubbleCenter = bubbleBodyRect.Center.ToSKPoint();
            if (tailPoint == bubbleCenter)
            {
                return bubbleBodyPath;
            }

            (SKPoint tailBase1, SKPoint tailBase2) =
                GetTailBasePoints(tailPoint, bubbleCenter, tailWidth);
            using SKPath bubbleTailPath = new();
            bubbleTailPath.AddPoly([tailPoint, tailBase1, tailBase2], true);

            return bubbleBodyPath.Op(bubbleTailPath, SKPathOp.Union);
        }

        using SKPath tooltipShapePath = getTooltipShapePath();

        // Get a filled shape that does not overlap with the stroke,
        // avoiding overlapping of non-opaque colors.
        LineStyle.ApplyToPaint(paint);
        using SKPath fillPath = tooltipShapePath
            .Op(paint.GetFillPath(tooltipShapePath), SKPathOp.Difference);
        Drawing.FillPath(rp.Canvas, paint, fillPath, FillStyle);

        Drawing.DrawPath(rp.Canvas, paint, tooltipShapePath, LineStyle);
    }

    public override void Render(RenderPack rp)
    {
        RenderTooltipShape(rp);
        base.Render(rp);
    }
}
