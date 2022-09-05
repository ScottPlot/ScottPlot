using ScottPlot.Axis;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class DebugBenchmark : IPlottable
{
    // TODO: replace this with a string so any text can be displayed
    public double ElapsedMilliseconds { get; set; }
    public bool IsVisible { get; set; } = false;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public void Render(SKSurface surface)
    {
        string message = $"Rendered in {ElapsedMilliseconds:0.000} ms ({1e3 / ElapsedMilliseconds:N0} FPS)";

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("consolas")
        };

        PixelSize textSize = Drawing.MeasureString(message, paint);
        float margin = 5;
        SKRect textRect = new(
            left: Axes.DataRect.Left + margin,
            top: Axes.DataRect.Bottom - paint.TextSize * .9f - 5 - margin,
            right: Axes.DataRect.Left + 5 * 2 + textSize.Width + margin,
            bottom: Axes.DataRect.Bottom - margin);

        paint.Color = SKColors.Yellow;
        paint.IsStroke = false;
        surface.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = false;
        surface.Canvas.DrawText(
            text: message,
            x: Axes.DataRect.Left + 4 + margin,
            y: Axes.DataRect.Bottom - 4 - margin,
            paint: paint);
    }
}
