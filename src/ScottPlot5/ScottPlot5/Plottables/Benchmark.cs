namespace ScottPlot.Plottables;

public class Benchmark : IPlottable
{
    public bool IsVisible { get; set; }
    public object Tag { get; set; } = new();

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        string message = $"Rendered in {rp.Elapsed.TotalMilliseconds:0.000} ms ({1e3 / rp.Elapsed.TotalMilliseconds:N0} FPS)";

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("consolas")
        };

        PixelSize textSize = Drawing.MeasureString(message, paint);
        float margin = 5;
        SKRect textRect = new(
            left: rp.DataRect.Left + margin,
            top: rp.DataRect.Bottom - paint.TextSize * .9f - 5 - margin,
            right: rp.DataRect.Left + 5 * 2 + textSize.Width + margin,
            bottom: rp.DataRect.Bottom - margin);

        paint.Color = SKColors.Yellow;
        paint.IsStroke = false;
        rp.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = true;
        rp.Canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = false;
        rp.Canvas.DrawText(
            text: message,
            x: rp.DataRect.Left + 4 + margin,
            y: rp.DataRect.Bottom - 4 - margin,
            paint: paint);
    }
}
