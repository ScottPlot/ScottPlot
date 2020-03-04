using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    public static class Function
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Function Plot";
            public string description { get; } = "A function (not data points) is provided to create this plot. Axes can be zoomed infinitely.";

            public void Render(Plot plt)
            {
                var func = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x));
                plt.PlotFunction(func, -10, 10, -1, 1);
            }
        }
    }
}
