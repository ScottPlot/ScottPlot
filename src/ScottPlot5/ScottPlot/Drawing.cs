using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Common operations using the default rendering system
/// </summary>
public static class Drawing
{
    /// <summary>
    /// A semi-transparent filled rectangle with an outline and X over it
    /// </summary>
    public static void DebugRect(SKSurface surface, PixelRect rect)
    {
        SKColor fillColor = SKColors.Blue.WithAlpha(100);
        SKColor lineColor = SKColors.Magenta;

        SKPaint paint = new()
        {
            IsAntialias = true,
        };

        paint.Color = fillColor;
        paint.IsStroke = false;
        surface.Canvas.DrawRect(rect.ToSKRect(), paint);

        paint.Color = lineColor;
        paint.IsStroke = true;
        surface.Canvas.DrawRect(rect.ToSKRect(), paint);
        surface.Canvas.DrawLine(rect.TopLeft.ToSKPoint(), rect.BottomRight.ToSKPoint(), paint);
        surface.Canvas.DrawLine(rect.BottomLeft.ToSKPoint(), rect.TopRight.ToSKPoint(), paint);
    }

    /* WARNING: Never call MeasureText() anywhere in the codebase except this one time in this function.
     * 
     * This way one day we can make a version which is independent of system fonts for development and testing.
     * 
     * It's not easy writing pixel-perfect unit tests when font sizes changes on every platform.
     * 
     */
    public static PixelSize MeasureString(string text, SKPaint paint)
    {
        float width = paint.MeasureText(text);
        float height = paint.TextSize;
        return new PixelSize(width, height);
    }
}
