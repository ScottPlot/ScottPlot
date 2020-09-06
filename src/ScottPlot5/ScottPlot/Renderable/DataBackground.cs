using ScottPlot.Renderer;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class DataBackground : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public Color Color = new Color(230, 230, 230);

        public void Render(IRenderer renderer, Dimensions dims, bool lowQuality)
        {
            if (Visible == false)
                return;

            Point clipPoint = new Point(dims.DataOffsetX, dims.DataOffsetY);
            Size clipSize = new Size(dims.DataWidth, dims.DataHeight);
            renderer.Clip(clipPoint, clipSize);
            renderer.Clear(Color);
            renderer.ClipReset();
        }
    }
}
