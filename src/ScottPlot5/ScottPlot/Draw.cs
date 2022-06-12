using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Common render tasks
/// </summary>
public static class Draw
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
}
