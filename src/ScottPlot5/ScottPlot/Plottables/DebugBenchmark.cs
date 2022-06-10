using SkiaSharp;

namespace ScottPlot.Plottables;

internal class DebugBenchmark : IPlottable
{
    public bool IsVisible { get; set; } = false;

    public double ElapsedMilliseconds;
    public HorizontalAxis? XAxis { get; set; }
    public VerticalAxis? YAxis { get; set; }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        string message = $"Rendered in {ElapsedMilliseconds:0.000} ms ({1e3 / ElapsedMilliseconds:N0} FPS)";

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("consolas")
        };

        float textWidth = paint.MeasureText(message);
        float margin = 5;
        SKRect textRect = new(
            left: dataRect.Left + margin,
            top: dataRect.Bottom - paint.TextSize * .9f - 5 - margin,
            right: dataRect.Left + 5 * 2 + textWidth + margin,
            bottom: dataRect.Bottom - margin);

        paint.Color = SKColors.Yellow;
        paint.IsStroke = false;
        surface.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = false;
        surface.Canvas.DrawText(message, dataRect.Left + 4 + margin, dataRect.Bottom - 4 - margin, paint);
    }
}
