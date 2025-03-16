
namespace ScottPlot.Plottables;

/// <summary>
/// A tooltip displays a text bubble pointing to a specific location in X/Y space.
/// The position of the bubble moves according to the axis limits to best display the text in the data area.
/// </summary>
public class Tooltip : LabelStyleProperties, IPlottable, IHasLine, IHasFill
{
    /// <summary>
    /// Location on the chart for the text of the tooltip.
    /// 
    /// </summary>
    public Coordinates LabelLocation { get; set; }
    public override LabelStyle LabelStyle { get; set; } = new() { FontSize = 14 };

    public Coordinates TipLocation { get; set; }

    public double TailWidthPercentage { get; set; } = 0.5;
    public float Radius { get; set; } = 5;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(LabelLocation)
            .Expanded(TipLocation);
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

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        MeasuredText measured = LabelStyle.Measure(LabelStyle.Text, paint);
        PixelRect bubbleBodyRect = measured.Rect(LabelAlignment)
            .Expand(new PixelPadding(
                Radius + LabelPixelPadding.Left,
                Radius + LabelPixelPadding.Right,
                Radius + LabelPixelPadding.Bottom,
                Radius + LabelPixelPadding.Top));

        Pixel px = Axes.GetPixel(LabelLocation);
        bubbleBodyRect = bubbleBodyRect.WithOffset(new(px.X, px.Y));

        using SKPath bubbleBodyPath = new();
        bubbleBodyPath.AddRoundRect(new(bubbleBodyRect.ToSKRect(), Radius));

        var tailPoint = Axes.GetPixel(TipLocation).ToSKPoint();
        float tailWidth = Math.Min(bubbleBodyRect.Width, bubbleBodyRect.Height)
            * (float)NumericConversion.Clamp(TailWidthPercentage, 0, 1);

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

        using SKPath path = getTooltipShapePath();
        Drawing.FillPath(rp.Canvas, paint, path, FillStyle);
        Drawing.DrawPath(rp.Canvas, paint, path, LineStyle);
        LabelStyle.Render(rp.Canvas, px, paint, LabelText);
    }
}
