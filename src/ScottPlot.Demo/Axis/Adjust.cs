using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Axis
{
    class Adjust
    {
        public class Zoom : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Zoom";
            public string description { get; } = "The user can easily zoom and zoom by providing a fractional zoom amount. Numbers >1 zoom in while numbers <1 zoom out.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.AxisZoom(2, 2);
            }
        }

        public class Pan : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Pan";
            public string description { get; } = "The user can easily pan by a defined amount on each axis.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.AxisPan(-10, .5);
            }
        }
    }
}
