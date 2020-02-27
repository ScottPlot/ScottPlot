using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Function
    {
        public class Quickstart : IPlotDemo
        {
            public string name { get; }
            public string description { get; }

            public void Render(Plot plt)
            {
                var func = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x));
                plt.PlotFunction(func, -10, 10, -1, 1);
            }
        }
    }
}
