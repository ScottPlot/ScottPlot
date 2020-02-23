using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public static class Plots
    {
        public static Plot SinAndCos()
        {
            var plt = new Plot();
            int pointCount = 100;
            plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Sin(pointCount));
            plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Cos(pointCount));
            return plt;
        }
    }
}
