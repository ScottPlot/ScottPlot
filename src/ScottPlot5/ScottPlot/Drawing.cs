using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Common operations using the default rendering system.
/// </summary>
public static class Drawing
{
    /* WARNING: Never call MeasureText() anywhere in the codebase except this one time in this function.
     * This way one day we can make a version which is independent of system fonts for development and testing.
     * It's not easy writing pixel-perfect unit tests when font sizes changes on every platform.
     */
    public static PixelSize MeasureString(string text, SKPaint paint)
    {
        float width = paint.MeasureText(text);
        float height = paint.TextSize;
        return new PixelSize(width, height);
    }

    public static PixelRect MeasureLargestString(string[] strings, SkiaSharp.SKPaint paint)
    {
        float maxWidth = 0;
        float maxHeight = 0;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize tickSize = MeasureString(strings[i], paint);
            maxWidth = Math.Max(maxWidth, tickSize.Width);
            maxHeight = Math.Max(maxHeight, tickSize.Height);
        }

        return new PixelRect(maxWidth, maxHeight);
    }

    public static void DrawLines(SKSurface surface, Pixel[] starts, Pixel[] ends, Color color, float width = 1, bool antiAlias = true)
    {
        if (starts.Length != ends.Length)
            throw new ArgumentException($"{nameof(starts)} and {nameof(ends)} must have same length");

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = true,
            IsAntialias = antiAlias,
            StrokeWidth = width,
        };

        using SKPath path = new();

        for (int i = 0; i < starts.Length; i++)
        {
            path.MoveTo(starts[i].X, starts[i].Y);
            path.LineTo(ends[i].X, ends[i].Y);
        }

        surface.Canvas.DrawPath(path, paint);
    }
}
