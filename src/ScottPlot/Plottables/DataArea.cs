using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Plottables
{
    // This class holds plot context (e.g., plot size and data area axis limits) designed to be
    // passed into the Render() function of plottables. Data contained here lets plottables know
    // where to render their data.

    public class DataArea
    {
        // TODO: mark this obsolete once an abstracted renderer is created.
        public System.Drawing.Graphics gfxData;

        public Drawing.Size sizePx;
        public Drawing.Scale pxPerUnit;
        public Config.AxisLimits2D axisLimits;

        public System.Drawing.PointF GetPixel(double x, double y)
        {
            double xPx = ((x - axisLimits.x1) * pxPerUnit.x);
            double yPx = sizePx.height - ((y - axisLimits.y1) * pxPerUnit.y);
            return new System.Drawing.PointF((float)xPx, (float)yPx);
        }
    }
}
