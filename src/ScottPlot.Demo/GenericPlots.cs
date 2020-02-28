using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
   
    public static class GenericPlots
    {
        public static void SinAndCos(Plot plt)
        {
            int pointCount = 51;
            plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Sin(pointCount));
            plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Cos(pointCount));
            plt.Legend();

            plt.Title("Example title");
            plt.YLabel("Vertical Axis");
            plt.XLabel("Horizontal Axis");
        }
    }
}
