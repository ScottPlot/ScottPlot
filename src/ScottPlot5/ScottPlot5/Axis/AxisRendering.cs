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
    public static void DrawFrame(SKSurface surface, PixelRect panelRect, Edge edge, LineStyle lineStyle)
    {
        using SKPaint framePaint = new()
        {
            Color = lineStyle.Color.ToSKColor(),
            IsAntialias = true,
            StrokeWidth = lineStyle.Width,
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

    private static void DrawTicksHorizontalAxis(SKSurface surface, FontStyle font, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickStyle majorStyle, TickStyle minorStyle)
    {
        if (axis.Edge != Edge.Bottom && axis.Edge != Edge.Top)
        {
            throw new InvalidEnumArgumentException();
        }

        using SKPaint paint = new();
        font.ApplyToPaint(paint);

        paint.TextAlign = SKTextAlign.Center;

        foreach (Tick tick in ticks)
        {
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;

            float xPx = axis.GetPixel(tick.Position, panelRect);
            float y = axis.Edge == Edge.Bottom ? panelRect.Top : panelRect.Bottom;
            float yEdge = axis.Edge == Edge.Bottom ? y + tickLength : y - tickLength;
            float fontSpacing = axis.Edge == Edge.Bottom ? paint.TextSize : -4;

            surface.Canvas.DrawLine(xPx, y, xPx, yEdge, paint);

            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, xPx, yEdge + fontSpacing, paint);
        }
    }

    private static void DrawTicksVerticalAxis(SKSurface surface, FontStyle font, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickStyle majorStyle, TickStyle minorStyle)
    {
        if (axis.Edge != Edge.Left && axis.Edge != Edge.Right)
        {
            throw new InvalidEnumArgumentException();
        }

        using SKPaint paint = new();
        font.ApplyToPaint(paint);

        paint.TextAlign = axis.Edge == Edge.Left ? SKTextAlign.Right : SKTextAlign.Left;

        foreach (Tick tick in ticks)
        {
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;

            float x = axis.Edge == Edge.Left ? panelRect.Right : panelRect.Left;
            float y = axis.GetPixel(tick.Position, panelRect);
            float xEdge = axis.Edge == Edge.Left ? x - tickLength : x + tickLength;
            surface.Canvas.DrawLine(x, y, xEdge, y, paint);

            float majorTickLabelPadding = 7;
            float labelPos = axis.Edge == Edge.Left ? x - majorTickLabelPadding : x + majorTickLabelPadding;
            if (!string.IsNullOrWhiteSpace(tick.Label))
                surface.Canvas.DrawText(tick.Label, labelPos, y + paint.TextSize * .4f, paint);
        }
    }

    public static void DrawTicks(SKSurface surface, FontStyle font, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickStyle majorStyle, TickStyle minorStyle)
    {
        if (axis.Edge.IsVertical())
            DrawTicksVerticalAxis(surface, font, panelRect, ticks, axis, majorStyle, minorStyle);
        else
            DrawTicksHorizontalAxis(surface, font, panelRect, ticks, axis, majorStyle, minorStyle);
    }
}
