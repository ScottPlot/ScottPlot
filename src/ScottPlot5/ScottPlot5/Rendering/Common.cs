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

        if (!plot.XAxis.Range.HasBeenSet) // may occur when there are no plottables with data
        {
            plot.SetAxisLimits(AxisLimits.Default);
        }
    }

    public static void RenderBackground(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        surface.Canvas.Clear(plot.FigureBackground.ToSKColor());

        using SKPaint paint = new() { Color = plot.DataBackground.ToSKColor() };
        surface.Canvas.DrawRect(dataRect.ToSKRect(), paint);
    }

    public static void RenderGridsBelowPlottables(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        foreach (IGrid grid in plot.Grids.Where(x => x.IsBeneathPlottables))
        {
            grid.Render(surface, dataRect);
        }
    }

    public static void RenderGridsAbovePlottables(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        foreach (IGrid grid in plot.Grids.Where(x => !x.IsBeneathPlottables))
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
            if (plottable is not IPlottableGL)
                surface.Canvas.ClipRect(dataRect.ToSKRect());
            plottable.Render(surface);
            surface.Canvas.Restore();
        }
    }

    public static void RenderLegends(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        LegendItem[] items = plot.Plottables.SelectMany(x => x.LegendItems).ToArray();

        foreach (ILegend legend in plot.Legends)
        {
            legend.Render(surface.Canvas, dataRect, items);
        }
    }

    public static void RenderPanels(SKSurface surface, PixelRect dataRect, IPanel[] panels, Layouts.Layout layout)
    {
        foreach (IPanel panel in panels)
        {
            float size = layout.PanelSizes[panel];
            float offset = layout.PanelOffsets[panel];
            panel.Render(surface, dataRect, size, offset);
        }
    }

    public static void RenderZoomRectangle(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        if (plot.ZoomRectangle.IsVisible)
        {
            plot.ZoomRectangle.Render(surface.Canvas, dataRect);
        }
    }

    public static void RenderBenchmark(SKSurface surface, PixelRect dataRect, Plot plot)
    {
        plot.Benchmark.Render(surface.Canvas, dataRect);
    }
}
