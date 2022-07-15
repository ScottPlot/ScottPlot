using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// This class contains all logic needed draw a plot onto a surface
/// </summary>
internal static class Render
{
    internal static RenderInformation OnSurface(SKSurface surface, Plot plot)
    {
        RenderInformation renderInfo = new();

        InitializeAllPlottableAxes(plot);
        AutoAxisAnyUnsetAxes(plot);

        renderInfo.FigureRect = PixelRect.FromSKRect(surface.Canvas.LocalClipBounds);
        renderInfo.DataRect = CalculateLayout(renderInfo, plot);
        plot.LeftAxisView.RegenerateTicks(renderInfo.DataRect);
        plot.BottomAxisView.RegenerateTicks(renderInfo.DataRect);

        RenderBackground(surface, renderInfo.DataRect, plot);
        RenderPlottables(surface, renderInfo.DataRect, plot);
        RenderAxes(surface, renderInfo.DataRect, plot);
        renderInfo.Finished();

        RenderZoomRectangle(surface, renderInfo.DataRect, plot);
        RenderDebugInfo(surface, renderInfo.DataRect, renderInfo.Elapsed.TotalMilliseconds, plot);
        return renderInfo;
    }

    /// <summary>
    /// Assign the default axis to any plottable that was not specifically assigned an axis
    /// </summary>
    private static void InitializeAllPlottableAxes(Plot plot)
    {
        foreach (var plottable in plot.GetPlottables())
        {
            if (plottable.XAxis is null)
                plottable.XAxis = plot.XAxis;
            if (plottable.YAxis is null)
                plottable.YAxis = plot.YAxis;
        }
    }

    private static void AutoAxisAnyUnsetAxes(Plot plot)
    {
        if (!plot.XAxis.HasBeenSet || !plot.YAxis.HasBeenSet)
        {
            plot.AutoScale();
        }
    }

    private static PixelRect CalculateLayout(RenderInformation renderInfo, Plot plot)
    {
        return plot.GetDataAreaRect(renderInfo.FigureRect);
    }

    private static void RenderBackground(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        surface.Canvas.Clear(SKColors.White);
        plot.Grid.Render(surface, dataRect, plot.BottomAxisView);
        plot.Grid.Render(surface, dataRect, plot.LeftAxisView);
    }

    private static void RenderPlottables(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        foreach (var plottable in plot.GetPlottables().Where(x => x.IsVisible))
        {
            surface.Canvas.Save();
            plottable.Render(surface, dataRect);
            surface.Canvas.Restore();
        }
    }

    private static void RenderAxes(SKSurface surface, PixelRect dataRect, Plot plot)
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

    private static void RenderZoomRectangle(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        if (plot.ZoomRectangle.IsVisible)
        {
            plot.ZoomRectangle.Render(surface, dataRect);
        }
    }

    private static void RenderDebugInfo(SKSurface surface, PixelRect dataRect, double elapsedMilliseconds, Plot plot)
    {
        if (plot.Benchmark.IsVisible)
        {
            plot.Benchmark.ElapsedMilliseconds = elapsedMilliseconds;
            plot.Benchmark.Render(surface, dataRect);
        }
    }
}
