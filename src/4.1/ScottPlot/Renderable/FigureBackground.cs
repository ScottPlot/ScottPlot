using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public PlotLayer Layer => PlotLayer.BelowData;

        public void Render(Bitmap bmp, PlotInfo info)
        {
            Debug.WriteLine("Rendering figure background");
        }
    }
}
