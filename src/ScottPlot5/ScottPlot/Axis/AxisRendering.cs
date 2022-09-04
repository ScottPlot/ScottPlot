using SkiaSharp;
using System.ComponentModel;

namespace ScottPlot.Axis;

/// <summary>
/// Helper methods for rendering common components
/// </summary>
public static class AxisRendering
{
    /// <summary>
    /// Draw a line along the edge of an axis on the side of the data area
    /// </summary>
    public static void DrawFrame(SKSurface surface, PixelRect dataRect, Edge edge, Color color, float offset)
    {
        float x1 = edge switch
        {
            Edge.Left => dataRect.Left - offset,
            Edge.Right => dataRect.Right + offset,
            Edge.Top => dataRect.Left,
            Edge.Bottom => dataRect.Left,
            _ => throw new InvalidEnumArgumentException(),
        };

        float x2 = edge switch
        {
            Edge.Left => dataRect.Left - offset,
            Edge.Right => dataRect.Right + offset,
            Edge.Top => dataRect.Right,
            Edge.Bottom => dataRect.Right,
            _ => throw new InvalidEnumArgumentException(),
        };

        float y1 = edge switch
        {
            Edge.Left => dataRect.Top,
            Edge.Right => dataRect.Top,
            Edge.Top => dataRect.Top - offset,
            Edge.Bottom => dataRect.Bottom + offset,
            _ => throw new InvalidEnumArgumentException(),
        };

        float y2 = edge switch
        {
            Edge.Left => dataRect.Bottom,
            Edge.Right => dataRect.Bottom,
            Edge.Top => dataRect.Top - offset,
            Edge.Bottom => dataRect.Bottom + offset,
            _ => throw new InvalidEnumArgumentException(),
        };

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsAntialias = true,
        };

        surface.Canvas.DrawLine(x1, y1, x2, y2, paint);
    }

    public static void DrawLabel(SKSurface surface, PixelRect dataRect, Edge edge, Label label, float offset, float pixelSize)
    {
        using var paint = label.MakePaint();

        Pixel px = edge switch
        {
            Edge.Left => new(
                x: dataRect.Left - offset - pixelSize,
                y: dataRect.VerticalCenter),

            Edge.Right => new(
                x: dataRect.Right + offset + pixelSize - paint.FontSpacing,
                y: dataRect.VerticalCenter),

            Edge.Bottom => new(
                x: dataRect.HorizontalCenter,
                y: dataRect.Bottom + paint.FontSpacing + offset),

            Edge.Top => new(
                x: dataRect.HorizontalCenter,
                y: dataRect.Top - paint.FontSpacing - 16 - offset), // tick label size

            _ => throw new InvalidEnumArgumentException()
        };

        label.Draw(surface, px, paint);
    }

    public static void DrawTicksBottom(SKSurface surface, PixelRect dataRect, Color color, float offset, IEnumerable<Tick> ticks, IXAxis axis)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Color = color.ToSKColor(),
        };

        foreach (Tick tick in ticks)
        {
            float xPx = axis.GetPixel(tick.Position, dataRect);
            float yEdge = dataRect.Bottom + offset;
            surface.Canvas.DrawLine(xPx, yEdge, xPx, yEdge + 3, paint);
            surface.Canvas.DrawText(tick.Label, xPx, yEdge + 3 + paint.FontSpacing, paint);
        }
    }

    public static void DrawTicksTop(SKSurface surface, PixelRect dataRect, Color color, float offset, IEnumerable<Tick> ticks, IXAxis axis)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Color = color.ToSKColor(),
        };

        foreach (Tick tick in ticks)
        {
            float xPx = axis.GetPixel(tick.Position, dataRect);
            float yEdge = dataRect.Top - offset;
            surface.Canvas.DrawLine(xPx, yEdge, xPx, yEdge - 3, paint);
            surface.Canvas.DrawText(tick.Label, xPx, yEdge - 7, paint);
        }
    }

    public static void DrawTicksLeft(SKSurface surface, PixelRect dataRect, Color color, float offset, IEnumerable<Tick> ticks, IYAxis axis)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Right,
            Color = color.ToSKColor(),
        };

        foreach (Tick tick in ticks)
        {
            float x = dataRect.Left - offset;
            float y = axis.GetPixel(tick.Position, dataRect);

            float majorTickLength = 5;
            surface.Canvas.DrawLine(x, y, x - majorTickLength, y, paint);

            float majorTickLabelPadding = 7;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x - majorTickLabelPadding, y + paint.TextSize * .4f, paint);
        }
    }

    public static void DrawTicksRight(SKSurface surface, PixelRect dataRect, Color color, float offset, IEnumerable<Tick> ticks, IYAxis axis)
    {
        using SKPaint paint = new()
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Left,
            Color = color.ToSKColor(),
        };

        foreach (Tick tick in ticks)
        {
            float x = dataRect.Right + offset;
            float y = axis.GetPixel(tick.Position, dataRect);

            float majorTickLength = 5;
            surface.Canvas.DrawLine(x, y, x + majorTickLength, y, paint);

            float majorTickLabelPadding = 7;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, x + majorTickLabelPadding, y + paint.TextSize * .4f, paint);
        }
    }
}
