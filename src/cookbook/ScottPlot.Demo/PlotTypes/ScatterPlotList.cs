using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class ScatterPlotList
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "ScatterPlotList Quickstart";
            public string description { get; } = "The ScatterPlotList displays a variable number of paired X/Y data points.";

            public void Render(Plot plt)
            {
                var spl = plt.PlotScatterList();

                // add points
                spl.Add(1, 5);
                spl.Add(3, 2);
                spl.Add(Math.PI, Math.PI);

                // add arrays
                double[] xs = { 4.8, 5.1, 5.2 };
                double[] ys = { 5.2, 1.1, 2.3 };
                spl.AddRange(xs, ys);

                // fit the axis limits to the latest data
                plt.AxisAuto();
            }
        }
    }
}
