using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Rendering;

/// <summary>
/// Common rendering tasks
/// </summary>
public static class Common
{
    public static void ReplaceNullAxesWithDefaults(Plot plot)
    {
        foreach (var plottable in plot.GetPlottables())
        {
            if (plottable.XAxis is null)
                plottable.XAxis = plot.XAxis;
            if (plottable.YAxis is null)
                plottable.YAxis = plot.YAxis;
        }
    }

    public static void AutoAxisAnyUnsetAxes(Plot plot)
    {
        if (!plot.XAxis.HasBeenSet || !plot.YAxis.HasBeenSet)
        {
            plot.AutoScale();
        }
    }

    public static PixelRect CalculateLayout(PixelRect figureRect, Plot plot)
    {
        return plot.GetDataAreaRect(figureRect);
    }

    public static void RenderBackground(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        surface.Canvas.Clear(SKColors.White);
    }

    public static void RenderGrids(SKSurface surface, PixelRect dataRect, Plot plot, bool beneathPlottables)
    {
        foreach (IGrid grid in plot.Grids.Where(x => x.IsBeneathPlottables == beneathPlottables))
        {
            grid.Render(surface, dataRect, plot.BottomAxisView);
            grid.Render(surface, dataRect, plot.LeftAxisView);
        }
    }

    public static void RenderPlottables(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        foreach (var plottable in plot.GetPlottables().Where(x => x.IsVisible))
        {
            surface.Canvas.Save();
            plottable.Render(surface, dataRect);
            surface.Canvas.Restore();
        }
    }

    public static void RenderAxes(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        // draw frame around data area
        using SKPaint paint = new()
        {
            IsAntialias = true,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        paint.Color = SKColors.Black;
        surface.Canvas.DrawRect(dataRect.ToSKRect(), paint);

        // draw each axis view
        plot.LeftAxisView.Render(surface, dataRect);
        plot.BottomAxisView.Render(surface, dataRect);
    }

    public static void RenderZoomRectangle(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        if (plot.ZoomRectangle.IsVisible)
        {
            plot.ZoomRectangle.Render(surface, dataRect);
        }
    }

    public static void RenderDebugInfo(SKSurface surface, PixelRect dataRect, TimeSpan elapsed, Plot plot)
    {
        if (plot.Benchmark.IsVisible)
        {
            plot.Benchmark.ElapsedMilliseconds = elapsed.TotalMilliseconds;
            plot.Benchmark.Render(surface, dataRect);
        }
    }
}
