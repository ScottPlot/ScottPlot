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
    public static void DrawFrame(SKSurface surface, PixelRect panelRect, Edge edge, Color color)
    {
        using SKPaint framePaint = new()
        {
            Color = color.ToSKColor(),
            IsAntialias = true,
        };

        if (edge == Edge.Left)
        {
            surface.Canvas.DrawLine(
                x0: panelRect.Right,
                y0: panelRect.Bottom,
                x1: panelRect.Right,
                y1: panelRect.Top,
                paint: framePaint);
        }
        else if (edge == Edge.Right)
        {
            surface.Canvas.DrawLine(
                x0: panelRect.Left,
                y0: panelRect.Bottom,
                x1: panelRect.Left,
                y1: panelRect.Top,
                paint: framePaint);
        }
        else if (edge == Edge.Bottom)
        {
            surface.Canvas.DrawLine(
                x0: panelRect.Left,
                y0: panelRect.Top,
                x1: panelRect.Right,
                y1: panelRect.Top,
                paint: framePaint);
        }
        else if (edge == Edge.Top)
        {
            surface.Canvas.DrawLine(
                x0: panelRect.Left,
                y0: panelRect.Bottom,
                x1: panelRect.Right,
                y1: panelRect.Bottom,
                paint: framePaint);
        }
        else
        {
            throw new NotImplementedException(edge.ToString());
        }
    }

    private static void DrawTicksHorizontalAxis(SKSurface surface, SKFont font, PixelRect panelRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
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
            float xPx = axis.GetPixel(tick.Position, panelRect);
            float y = axis.Edge == Edge.Bottom ? panelRect.Top : panelRect.Bottom;
            float yEdge = axis.Edge == Edge.Bottom ? y + 3 : y - 3;
            float fontSpacing = axis.Edge == Edge.Bottom ? paint.TextSize : -4;

            surface.Canvas.DrawLine(xPx, y, xPx, yEdge, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, xPx, yEdge + fontSpacing, paint);
        }
    }

    private static void DrawTicksVerticalAxis(SKSurface surface, SKFont font, PixelRect panelRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
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
            float x = axis.Edge == Edge.Left ? panelRect.Right : panelRect.Left;
            float y = axis.GetPixel(tick.Position, panelRect);

            float majorTickLength = 5;
            float xEdge = axis.Edge == Edge.Left ? x - majorTickLength : x + majorTickLength;
            surface.Canvas.DrawLine(x, y, xEdge, y, paint);

            float majorTickLabelPadding = 7;
            float labelPos = axis.Edge == Edge.Left ? x - majorTickLabelPadding : x + majorTickLabelPadding;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, labelPos, y + paint.TextSize * .4f, paint);
        }
    }

    public static void DrawTicks(SKSurface surface, SKFont font, PixelRect panelRect, Color color, IEnumerable<Tick> ticks, IAxis axis)
    {
        if (axis.Edge.IsVertical())
            DrawTicksVerticalAxis(surface, font, panelRect, color, ticks, axis);
        else
            DrawTicksHorizontalAxis(surface, font, panelRect, color, ticks, axis);
    }
}
