namespace ScottPlot;

/// <summary>
/// A circle centered at the origin
/// </summary>
public class PolarAxisCircle(double radius) : IHasLine
{
    public double Radius { get; set; } = radius;

    public LineStyle LineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.5),
    };

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public LinePattern LinePattern
    {
        get => LineStyle.Pattern;
        set => LineStyle.Pattern = value;
    }

    public Color LineColor
    {
        get => LineStyle.Color;
        set => LineStyle.Color = value;
    }

    public void Render(RenderPack rp, IAxes axes, SKPaint paint)
    {
        float pixelX = axes.GetPixelX(Radius) - axes.XAxis.GetPixel(0, rp.DataRect);
        float pixelY = axes.GetPixelY(Radius) - axes.YAxis.GetPixel(0, rp.DataRect);
        PixelRect circleRect = new(-pixelX, pixelX, pixelY, -pixelY);
        Drawing.DrawOval(rp.Canvas, paint, LineStyle, circleRect);
    }
}
