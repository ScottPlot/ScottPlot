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
    public static void DrawFrame(SKSurface surface, PixelRect dataRect, Edge edge, Color color)
    {
        float x1 = edge switch
        {
            Edge.Left => dataRect.Left,
            Edge.Right => dataRect.Right,
            Edge.Top => dataRect.Left,
            Edge.Bottom => dataRect.Left,
            _ => throw new InvalidEnumArgumentException(),
        };

        float x2 = edge switch
        {
            Edge.Left => dataRect.Left,
            Edge.Right => dataRect.Right,
            Edge.Top => dataRect.Right,
            Edge.Bottom => dataRect.Right,
            _ => throw new InvalidEnumArgumentException(),
        };

        float y1 = edge switch
        {
            Edge.Left => dataRect.Top,
            Edge.Right => dataRect.Top,
            Edge.Top => dataRect.Top,
            Edge.Bottom => dataRect.Bottom,
            _ => throw new InvalidEnumArgumentException(),
        };

        float y2 = edge switch
        {
            Edge.Left => dataRect.Bottom,
            Edge.Right => dataRect.Bottom,
            Edge.Top => dataRect.Top,
            Edge.Bottom => dataRect.Bottom,
            _ => throw new InvalidEnumArgumentException(),
        };

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsAntialias = true,
        };

        surface.Canvas.DrawLine(x1, y1, x2, y2, paint);
    }

    public static void DrawLabel(SKSurface surface, PixelRect dataRect, Edge edge, Label label, float pixelSize)
    {
        using var paint = label.MakePaint();

        Pixel px = edge switch
        {
            Edge.Left => new(
                x: dataRect.Left - pixelSize + 5,
                y: dataRect.VerticalCenter),

            Edge.Right => new(
                x: dataRect.Right + pixelSize - paint.FontSpacing - 5,
                y: dataRect.VerticalCenter),

            Edge.Bottom => new(
                x: dataRect.HorizontalCenter,
                y: dataRect.Bottom + pixelSize - paint.FontSpacing - 5),

            Edge.Top => new(
                x: dataRect.HorizontalCenter,
                y: dataRect.Top - pixelSize),

            _ => throw new InvalidEnumArgumentException()
        };

        label.Draw(surface, px, paint);
    }

    private static void DrawTicksHorizontalAxis(SKSurface surface, SKFont font, PixelRect dataRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
    {
        if (axis.Edge != Edge.Bottom && axis.Edge != Edge.Top)
        {
            throw new InvalidEnumArgumentException();
        }

        using SKPaint paint = new(font)
        {
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Color = color.ToSKColor(),
        };


        foreach (Tick tick in ticks)
        {
            float xPx = axis.GetPixel(tick.Position, dataRect);
            float y = axis.Edge == Edge.Bottom ? dataRect.Bottom : dataRect.Top;
            float yEdge = axis.Edge == Edge.Bottom ? y + 3 : y - 3;
            float fontSpacing = axis.Edge == Edge.Bottom ? paint.TextSize : -4;

            surface.Canvas.DrawLine(xPx, y, xPx, yEdge, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, xPx, yEdge + fontSpacing, paint);
        }
    }

    private static void DrawTicksVerticalAxis(SKSurface surface, SKFont font, PixelRect dataRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
    {
        if (axis.Edge != Edge.Left && axis.Edge != Edge.Right)
        {
            throw new InvalidEnumArgumentException();
        }

        using SKPaint paint = new(font)
        {
            IsAntialias = true,
            TextAlign = axis.Edge == Edge.Left ? SKTextAlign.Right : SKTextAlign.Left,
            Color = color.ToSKColor(),
        };

        foreach (Tick tick in ticks)
        {
            float x = axis.Edge == Edge.Left ? dataRect.Left : dataRect.Right;
            float y = axis.GetPixel(tick.Position, dataRect);

            float majorTickLength = 5;
            float xEdge = axis.Edge == Edge.Left ? x - majorTickLength : x + majorTickLength;
            surface.Canvas.DrawLine(x, y, xEdge, y, paint);

            float majorTickLabelPadding = 7;
            float labelPos = axis.Edge == Edge.Left ? x - majorTickLabelPadding : x + majorTickLabelPadding;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, labelPos, y + paint.TextSize * .4f, paint);
        }
    }

    public static void DrawTicks(SKSurface surface, SKFont font, PixelRect dataRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
    {
        if (axis.Edge == Edge.Left || axis.Edge == Edge.Right)
            DrawTicksVerticalAxis(surface, font, dataRect, color, ticks, axis);
        else if (axis.Edge == Edge.Bottom || axis.Edge == Edge.Top)
            DrawTicksHorizontalAxis(surface, font, dataRect, color, ticks, axis);
        else
            throw new InvalidEnumArgumentException();
    }
}
