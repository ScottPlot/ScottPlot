namespace ScottPlot.Rendering;

public class StandardRenderer : IRenderer
{
    public RenderDetails Render(RenderPack rp)
    {
        Common.ReplaceNullAxesWithDefaults(rp);
        Common.AutoAxisAnyUnsetAxes(rp);
        Common.EnsureAxesHaveArea(rp);
        Common.RecalculateDataRect(rp);
        Common.RegnerateTicks(rp);
        Common.RenderBackground(rp);
        Common.RenderGridsBelowPlottables(rp);
        Common.RenderPlottables(rp);
        Common.RenderGridsAbovePlottables(rp);
        Common.RenderLegends(rp);
        Common.RenderPanels(rp);
        Common.RenderZoomRectangle(rp);
        Common.SyncGLPlottables(rp);
        Common.RenderBenchmark(rp);
        return new RenderDetails(rp);
    }
}
