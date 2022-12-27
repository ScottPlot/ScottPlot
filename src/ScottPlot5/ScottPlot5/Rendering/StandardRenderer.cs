using ScottPlot.LayoutSystem;
using SkiaSharp;

namespace ScottPlot.Rendering;

public class StandardRenderer : IRenderer
{
    public RenderDetails Render(SKSurface surface, Plot plot)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Common.ReplaceNullAxesWithDefaults(plot);
        Common.AutoAxisAnyUnsetAxes(plot);

        PixelRect figureRect = PixelRect.FromSKRect(surface.Canvas.LocalClipBounds);

        IPanel[] panels = plot.GetAllPanels();
        FinalLayout layout = plot.Layout.GetLayout(figureRect, panels);
        PixelRect area = layout.Area;

        plot.XAxis.TickGenerator.Regenerate(plot.XAxis.Range, area.Width);
        plot.YAxis.TickGenerator.Regenerate(plot.YAxis.Range, area.Height);

        Common.RenderBackground(surface, area, plot);
        Common.RenderGrids(surface, area, plot, beneathPlottables: true);
        Common.RenderPlottables(surface, area, plot);
        Common.RenderGrids(surface, area, plot, beneathPlottables: false);
        Common.RenderPanels(surface, area, layout.Panels);
        Common.RenderZoomRectangle(surface, area, plot);
        sw.Stop();

        Common.RenderBenchmark(surface, area, sw.Elapsed, plot);

        return new RenderDetails(figureRect, area, sw.Elapsed);
    }
}
