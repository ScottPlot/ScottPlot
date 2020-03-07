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
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            plt.PlotScatter(x, sin);
            plt.PlotScatter(x, cos);
        }
    }
}
