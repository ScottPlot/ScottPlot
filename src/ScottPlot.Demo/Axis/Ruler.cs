using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Axis
{
    class Ruler
    {
        public class RulerMode : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ruler Mode";
            public string description { get; } = "Ruler mode is an alternative way to display axis tick labels";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Ticks(rulerModeX: true, rulerModeY: true);
            }
        }
    }
}
