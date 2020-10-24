using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        void Render(Settings settings);
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);
    }
}
