using ScottPlot.LayoutSystem;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        foreach (IPlottable plottable in plot.Plottables)
        {
            if (!plottable.Axes.XAxis.Range.HasBeenSet || !plottable.Axes.YAxis.Range.HasBeenSet)
            {
                plot.AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis);
            }
        }
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

    public static void RenderPanels(SKSurface surface, PixelRect dataRect, IPanel[] panels, FinalLayout layout)
    {
        foreach (IPanel panel in panels)
        {
            panel.Render(surface, dataRect, layout.PanelSizes[panel], layout.PanelOffsets[panel]);
        }
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
