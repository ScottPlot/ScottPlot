using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Plottables
{
    // This class holds plot context (e.g., plot size and data area axis limits) allowing plottables to know where to draw their data.
    public class DataArea
    {
        // TODO: mark this obsolete once a fully abstracted rendering system is added
        public System.Drawing.Graphics gfxData;

        public double pxPerUnitX, pxPerUnitY;
        public float heightPx, widthPx;

        public Config.AxisLimits2D axisLimits;

        public System.Drawing.PointF GetPixel(double xCoordinate, double yCoordinate)
        {
            // note that this is pixel location on the DATA graphic (not the FIGURE graphic)
            float xPx = (float)((xCoordinate - axisLimits.x1) * pxPerUnitX);
            float yPx = heightPx - (float)((yCoordinate - axisLimits.y1) * pxPerUnitY);
            return new System.Drawing.PointF(xPx, yPx);
        }
    }
}
