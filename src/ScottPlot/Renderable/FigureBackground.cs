using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;
        public bool IsVisible { get; set; } = true;

        public void Render(PlotDimensions dims, Graphics gfx)
        {
            gfx.ClipToDataArea(dims, false, true);
            gfx.Clear(Color);
        }
    }
}
