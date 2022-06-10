using SkiaSharp;

namespace ScottPlot.Plottables;

public class DebugPoint : PlottableBase
{
    public Coordinate Position { get; set; }
    public SKColor Color { get; set; } = SKColors.Magenta;

    public DebugPoint()
    {

    }

    public DebugPoint(double x, double y, SKColor color)
    {
        Position = new(x, y);
        Color = color;
    }

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Color = Color.WithAlpha(200),
            StrokeWidth = 1,
            IsStroke = true,
            PathEffect = SKPathEffect.CreateDash(new float[] { 4, 4, }, 0),
        };

        float x = XAxis.GetPixel(Position.X, dataRect);
        float y = YAxis.GetPixel(Position.Y, dataRect);

        SKCanvas canvas = surface.Canvas;
        canvas.DrawLine(x, dataRect.Top, x, dataRect.Bottom, paint);
        canvas.DrawLine(dataRect.Left, y, dataRect.Right, y, paint);
        canvas.DrawCircle(x, y, 5, paint);

        paint.Color = Color;
        paint.IsStroke = false;
        SKTypeface tf = SKTypeface.FromFamilyName("consolas");
        SKFont font = new(tf, size: 12);
        canvas.DrawText($"X={Position.X:N3}", x + 3, y - 1.5f * font.Size, font, paint);
        canvas.DrawText($"Y={Position.Y:N3}", x + 3, y - .5f * font.Size, font, paint);
    }
}
