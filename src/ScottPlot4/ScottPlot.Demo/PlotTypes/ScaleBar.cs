using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class ScaleBar
    {
        public class ScaleBarQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scale Bar";
            public string description { get; } = "An L-shaped scalebar can be added in the corner of any plot";

            public void Render(Plot plt)
            {
                // plot some data
                plt.PlotSignal(DataGen.Sin(51));
                plt.PlotSignal(DataGen.Cos(51, mult: 1.5));

                // add the scalebar
                plt.PlotScaleBar(5, .25, "5 ms", "250 pA");

                // remove axis and grid lines
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
                plt.AxisAuto(0);
                plt.TightenLayout(0);
            }
        }
    }
}
