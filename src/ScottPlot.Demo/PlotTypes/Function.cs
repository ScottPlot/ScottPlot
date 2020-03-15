using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Function
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Function Plot";
            public string description { get; } = "A function (not data points) is provided to create this plot. Axes can be zoomed infinitely. " +
                "For functions with a restricted domain, you should return null to prevent errors.\n\n" +
                "e.g. new Func<double, double?>((x) => x > 0 ? Math.Log(x) : (double?)null);";

            public void Render(Plot plt)
            {
                var func = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x));
                plt.PlotFunction(func);
            }
        }
    }
}
