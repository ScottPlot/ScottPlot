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
            public string description { get; } = "A function (not data points) is provided to create this plot. Axes can be zoomed infinitely.";

            public void Render(Plot plt)
            {
                var func1 = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(x / 2));
                plt.PlotFunction(func1, lineWidth: 2, label: "sin(x) * sin(x/2)");

                var func2 = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(x / 3));
                plt.PlotFunction(func2, lineWidth: 2, label: "sin(x) * sin(x/3)", lineStyle: LineStyle.Dot);

                var func3 = new Func<double, double?>((x) => Math.Cos(x) * Math.Sin(x / 5));
                plt.PlotFunction(func3, lineWidth: 2, label: "cos(x) * cos(x/5)", lineStyle: LineStyle.Dash);

                plt.Title("Plot Mathematical Functions");
                plt.Legend();
            }
        }
    }
}
