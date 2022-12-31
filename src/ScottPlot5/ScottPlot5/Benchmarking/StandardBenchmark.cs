namespace ScottPlot.Benchmarking;

internal class StandardBenchmark : IBenchmark
{
    public bool IsVisible { get; set; } = false;
    public TimeSpan Elapsed => SW.Elapsed;

    private readonly Stopwatch SW = new();
    public void Reset() => SW.Reset();
    public void Restart() => SW.Restart();
    public void Start() => SW.Start();
    public void Stop() => SW.Stop();

    public void Render(SKCanvas canvas, PixelRect dataRect)
    {
        if (!IsVisible)
            return;

        string message = $"Rendered in {Elapsed.TotalMilliseconds:0.000} ms ({1e3 / Elapsed.TotalMilliseconds:N0} FPS)";

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("consolas")
        };

        PixelSize textSize = Drawing.MeasureString(message, paint);
        float margin = 5;
        SKRect textRect = new(
            left: dataRect.Left + margin,
            top: dataRect.Bottom - paint.TextSize * .9f - 5 - margin,
            right: dataRect.Left + 5 * 2 + textSize.Width + margin,
            bottom: dataRect.Bottom - margin);

        paint.Color = SKColors.Yellow;
        paint.IsStroke = false;
        canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = true;
        canvas.DrawRect(textRect, paint);

        paint.Color = SKColors.Black;
        paint.IsStroke = false;
        canvas.DrawText(
            text: message,
            x: dataRect.Left + 4 + margin,
            y: dataRect.Bottom - 4 - margin,
            paint: paint);
    }
}
