using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        public PlotLayer Layer => PlotLayer.BelowData;

        public void Render(Bitmap bmp, PlotInfo info)
        {
            Debug.WriteLine("Rendering axis ticks");
        }
    }
}
