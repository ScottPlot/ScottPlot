using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Axis
{
    class Labels
    {
        public class AxisLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Title and Axis Labels";
            public string description { get; } = "Title and axis labels can be defined and custoized using arguments.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Title("Plot Title");
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
            }
        }
    }
}
