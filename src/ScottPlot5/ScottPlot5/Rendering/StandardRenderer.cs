using ScottPlot.LayoutSystem;

namespace ScottPlot.Rendering;

public class StandardRenderer : IRenderer
{
    public RenderDetails Render(SKSurface surface, Plot plot)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew(); // TODO: use struct timer

        Common.ReplaceNullAxesWithDefaults(plot);
        Common.AutoAxisAnyUnsetAxes(plot);

        PixelRect figureRect = surface.GetPixelRect();

        IPanel[] panels = plot.GetAllPanels(); // includes axis and non-axis panels
        FinalLayout layout = plot.Layout.GetLayout(figureRect, panels);
        PixelRect dataRect = layout.DataRect;

        plot.XAxis.TickGenerator.Regenerate(plot.XAxis.Range, dataRect.Width);
        plot.YAxis.TickGenerator.Regenerate(plot.YAxis.Range, dataRect.Height);

        Common.RenderBackground(surface, dataRect, plot);
        Common.RenderGridsBelowPlottables(surface, dataRect, plot);
        Common.RenderPlottables(surface, dataRect, plot);
        Common.RenderGridsAbovePlottables(surface, dataRect, plot);
        Common.RenderPanels(surface, dataRect, panels, layout);
        Common.RenderZoomRectangle(surface, dataRect, plot);
        sw.Stop();

        Common.RenderBenchmark(surface, dataRect, sw.Elapsed, plot);

        return new RenderDetails(figureRect, dataRect, sw.Elapsed);
    }
}
