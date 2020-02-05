using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Plottables
{
    // This class holds the minimum amount of information needed to pass into Plottable.Render()

    // This class was created because Settings is HUGE and contains lots of things the renderer doesn't need to know.
    // Doing this is a step toward having a framework-independent renderer.

    // While this is being developed the settings module is inside the Context, but it can be removed once all the
    // useful data is properly extracted.

    public class Context
    {
        //[Obsolete("dont access graphics objects directly", true)]
        public System.Drawing.Graphics gfxData;

        // local storage
        public double xAxisScale, yAxisScale;
        public float dataSizeHeight, dataSizeWidth;
        public Config.AxisLimits2D axisLimits;

        public System.Drawing.PointF GetPixel(double locationX, double locationY)
        {
            // Return the pixel location on the data bitmap corresponding to an X/Y location.
            // This is useful when drawing graphics on the data bitmap.
            float xPx = (float)((locationX - axisLimits.x1) * xAxisScale);
            float yPx = dataSizeHeight - (float)((locationY - axisLimits.y1) * yAxisScale);
            return new System.Drawing.PointF(xPx, yPx);
        }
    }
}
