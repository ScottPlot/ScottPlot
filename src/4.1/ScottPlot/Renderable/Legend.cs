using ScottPlot.Renderer;
using System;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class Legend : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.AboveData;

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            Debug.WriteLine("Rendering Legend");
        }
    }
}
