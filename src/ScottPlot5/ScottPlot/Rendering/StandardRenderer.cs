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
        PixelRect dataRect = Common.CalculateLayout(figureRect, plot);
        plot.XAxis.TickGenerator.Regenerate(plot.XAxis.Range, dataRect.Width);
        plot.YAxis.TickGenerator.Regenerate(plot.YAxis.Range, dataRect.Height);

        Common.RenderBackground(surface, dataRect, plot);
        Common.RenderGrids(surface, dataRect, plot, beneathPlottables: true);
        Common.RenderPlottables(surface, dataRect, plot);
        Common.RenderGrids(surface, dataRect, plot, beneathPlottables: false);
        Common.RenderAxes(surface, dataRect, plot);
        Common.RenderZoomRectangle(surface, dataRect, plot);
        sw.Stop();

        Common.RenderBenchmark(surface, dataRect, sw.Elapsed, plot);

        return new RenderDetails(figureRect, dataRect, sw.Elapsed);
    }
}
