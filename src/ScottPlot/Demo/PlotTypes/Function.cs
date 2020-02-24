using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Function
    {
        public static Plot Quickstart()
        {
            var plt = new Plot();
            var func = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x));
            plt.PlotFunction(func, -10, 10, -1, 1);
            return plt;
        }
    }
}
