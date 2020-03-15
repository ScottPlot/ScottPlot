using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Arrow
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot arrows";
            public string description { get; } = "arrows can be added which point at specific points on the plot";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.PlotArrow(25, 0, 27, .2, label: "default");
                plt.PlotArrow(27, -.25, 23, -.5, label: "big", lineWidth: 10);
                plt.PlotArrow(12, 1, 12, 0, label: "skinny", arrowheadLength: 10);
                plt.PlotArrow(20, .6, 20, 1, label: "fat", arrowheadWidth: 10);
                plt.Legend(fixedLineWidth: false);
            }
        }
    }
}
