using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Point
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot points";
            public string description { get; } = "Points are essentially scatter plots with a single point.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.PlotPoint(25, 0.8);
                plt.PlotPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            }
        }
    }
}
