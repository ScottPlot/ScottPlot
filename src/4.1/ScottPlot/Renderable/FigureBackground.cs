using System.Drawing;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;

        public void Render(PlotInfo info)
        {
        }
    }
}
