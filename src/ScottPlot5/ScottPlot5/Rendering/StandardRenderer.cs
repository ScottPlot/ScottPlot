using ScottPlot.Layouts;
using System.Linq;

namespace ScottPlot.Rendering;

public class StandardRenderer : IRenderer
{
    public RenderDetails Render(SKSurface surface, Plot plot)
    {
        plot.Benchmark.Restart();
        Common.ReplaceNullAxesWithDefaults(plot);
        Common.AutoAxisAnyUnsetAxes(plot);

        PixelRect figureRect = surface.GetPixelRect();

        IPanel[] panels = plot.GetAllPanels(); // includes axis and non-axis panels
        Layout layout = plot.Layout.GetLayout(figureRect, panels);
        PixelRect dataRect = layout.DataRect;

        plot.XAxis.TickGenerator.Regenerate(plot.XAxis.Range, dataRect.Width);
        plot.YAxis.TickGenerator.Regenerate(plot.YAxis.Range, dataRect.Height);

        Common.RenderBackground(surface, dataRect, plot);
        Common.RenderGridsBelowPlottables(surface, dataRect, plot);
        Common.RenderPlottables(surface, dataRect, plot);
        Common.RenderGridsAbovePlottables(surface, dataRect, plot);
        Common.RenderLegends(surface, dataRect, plot);
        Common.RenderPanels(surface, dataRect, panels, layout);
        Common.RenderZoomRectangle(surface, dataRect, plot);
        Common.SyncGLPlottables(plot);
        plot.Benchmark.Stop();

        Common.RenderBenchmark(surface, dataRect, plot);

        return new RenderDetails(figureRect, dataRect, plot.Benchmark.Elapsed);
    }
}
