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

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            Point clipPoint = new Point(info.DataOffsetX, info.DataOffsetY);
            Size clipSize = new Size(info.DataWidth, info.DataHeight);
            renderer.Clip(clipPoint, clipSize);
            renderer.Clear(Color);
            renderer.ClipReset();
        }
    }
}
