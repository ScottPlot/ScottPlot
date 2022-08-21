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
        foreach (var plottable in plot.Plottables)
        {
            if (plottable.Axes.XAxis is null)
                plottable.Axes.XAxis = plot.XAxis;
            if (plottable.Axes.YAxis is null)
                plottable.Axes.YAxis = plot.YAxis;
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
        return plot.Layout.GetDataAreaRect(figureRect, plot.XAxes, plot.YAxes);
    }

    public static void RenderBackground(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        surface.Canvas.Clear(SKColors.White);
    }

    public static void RenderGrids(SKSurface surface, PixelRect dataRect, Plot plot, bool beneathPlottables)
    {
        foreach (IGrid grid in plot.Grids.Where(x => x.IsBeneathPlottables == beneathPlottables))
        {
            grid.Render(surface, dataRect);
        }
    }

    public static void RenderPlottables(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        foreach (var plottable in plot.Plottables.Where(x => x.IsVisible))
        {
            plottable.Axes.DataRect = dataRect;
            surface.Canvas.Save();
            surface.Canvas.ClipRect(dataRect.ToSKRect());
            plottable.Render(surface);
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
        plot.XAxis.Render(surface, dataRect);
        plot.YAxis.Render(surface, dataRect);
    }

    public static void RenderZoomRectangle(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        if (plot.ZoomRectangle.IsVisible)
        {
            plot.ZoomRectangle.Axes.DataRect = dataRect;
            plot.ZoomRectangle.Render(surface);
        }
    }

    public static void RenderBenchmark(SKSurface surface, PixelRect dataRect, TimeSpan elapsed, Plot plot)
    {
        if (plot.Benchmark.IsVisible)
        {
            plot.Benchmark.Axes.DataRect = dataRect;
            plot.Benchmark.ElapsedMilliseconds = elapsed.TotalMilliseconds;
            plot.Benchmark.Render(surface);
        }
    }
}
