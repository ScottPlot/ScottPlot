using ScottPlot.Renderer;

namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        PlotLayer Layer { get; }
        bool Visible { get; set; }
        void Render(IRenderer renderer, Dimensions dims, bool lowQuality);
    }
}
