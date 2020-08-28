using System.Drawing;

namespace ScottPlot.Renderable
{
    public class DataBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;

        public void Render(PlotInfo info)
        {
        }
    }
}
