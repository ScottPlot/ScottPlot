using ScottPlot.Renderer;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public Color Color = Colors.White;

        public void Render(IRenderer renderer, Dimensions dims, bool lowQuality)
        {
            if (Visible == false)
                return;

            renderer.Clear(Color);
        }
    }
}
