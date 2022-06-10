using SkiaSharp;

namespace ScottPlot.Plottables;

public class DebugPoint : IPlottable
{
    public Coordinate Position { get; set; }

    public SKColor Color { get; set; } = SKColors.White;

    public DebugPoint()
    {

    }

    public DebugPoint(double x, double y, SKColor color)
    {
        Position = new(x, y);
        Color = color;
    }

    public void Render(SKSurface surface, PixelRect dataRect, HorizontalAxis xAxis, VerticalAxis yAxis)
    {
        surface.Canvas.ClipRect(dataRect.ToSKRect());

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Color = Color.WithAlpha(200),
            StrokeWidth = 1,
            IsStroke = true,
            PathEffect = SKPathEffect.CreateDash(new float[] { 4, 4, }, 0),
        };

        float x = xAxis.GetPixel(Position.X, dataRect.Left, dataRect.Right);
        float y = yAxis.GetPixel(Position.Y, dataRect.Bottom, dataRect.Top);

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
