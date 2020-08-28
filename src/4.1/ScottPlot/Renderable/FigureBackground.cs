using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public Color Color = Color.White;

        public void Render(Bitmap bmp, PlotInfo info)
        {
            if (Visible == false)
                return;

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color);
            }
        }
    }
}
