﻿using SkiaSharp;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public float LineWidth = 1;
    public Color LineColor = Colors.Black.WithAlpha(20);

    private SKPaint MakeMajorGridLinePaint()
    {
        return new SKPaint()
        {
            Color = LineColor.ToSKColor(),
            IsStroke = true,
            IsAntialias = true,
            StrokeWidth = LineWidth,
        };
    }

    public void Render(SKSurface surface, PixelRect dataRect, AxisViews.IAxisView axisView)
    {
        if (axisView.Axis.IsHorizontal)
        {
            RenderVerticalGridLines(surface, dataRect, axisView);
        }
        else
        {
            RenderHorizontalGridLines(surface, dataRect, axisView);
        }
    }

    private void RenderHorizontalGridLines(SKSurface surface, PixelRect dataRect, AxisViews.IAxisView axisView)
    {
        using SKPaint paint = MakeMajorGridLinePaint();

        foreach (Tick tick in axisView.GetVisibleTicks())
        {
            float y = axisView.Axis.GetPixel(tick.Position, dataRect);
            surface.Canvas.DrawLine(dataRect.Left, y, dataRect.Right, y, paint);
        }
    }

    private void RenderVerticalGridLines(SKSurface surface, PixelRect dataRect, AxisViews.IAxisView axisView)
    {
        using SKPaint paint = MakeMajorGridLinePaint();

        foreach (Tick tick in axisView.GetVisibleTicks())
        {
            float x = axisView.Axis.GetPixel(tick.Position, dataRect);
            surface.Canvas.DrawLine(x, dataRect.Bottom, x, dataRect.Top, paint);
        }
    }
}
