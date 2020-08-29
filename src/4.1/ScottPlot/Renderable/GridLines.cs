using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Renderable
{
    public class GridLines : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            Debug.WriteLine("Rendering grid lines");
        }
    }
}
