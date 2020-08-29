using ScottPlot.Renderer;

namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        PlotLayer Layer { get; }
        bool Visible { get; set; }
        bool AntiAlias { get; set; }
        void Render(IRenderer renderer, PlotInfo info);
    }
}
