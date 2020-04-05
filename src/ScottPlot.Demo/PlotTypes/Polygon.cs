using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Polygon
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Polygon Quickstart";
            public string description { get; } = "Pairs of X/Y points can be used to display polygons.";

            public void Render(Plot plt)
            {
                plt.PlotPolygon(
                    xs: new double[] { 2, 8, 6, 4 },
                    ys: new double[] { 3, 4, 0.5, 1 },
                    label: "polygon A", lineWidth: 2, fillAlpha: .8,
                    lineColor: System.Drawing.Color.Black);

                plt.PlotPolygon(
                    xs: new double[] { 3, 2.5, 5 },
                    ys: new double[] { 4.5, 1.5, 2.5 },
                    label: "polygon B", lineWidth: 2, fillAlpha: .8,
                    lineColor: System.Drawing.Color.Black);
                
                plt.Title($"Polygon Demonstration");
                plt.Legend();
            }
        }
    }
}
