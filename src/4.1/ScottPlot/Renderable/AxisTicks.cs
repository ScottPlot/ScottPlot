using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public void Render(Bitmap bmp, PlotInfo info)
        {
            if (Visible == false)
                return;

            Debug.WriteLine("Rendering axis ticks");
        }
    }
}
