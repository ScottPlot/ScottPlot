using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Ticks
{
    class Visibility
    {
        public class DateAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Hide Tick Labels";
            public string description { get; } = "Tick label visibility can be controlled with arguments to the Ticks() method";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Ticks(displayTicksX: false);
            }
        }
    }
}
