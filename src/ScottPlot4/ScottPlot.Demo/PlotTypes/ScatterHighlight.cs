using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class ScatterHighlight
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Highlight Scatter Plot Quickstart";
            public string description { get; } = "Allows higlighting points on a scatter plot";

            public void Render(Plot plt)
            {
                int pointCount = 100;
                Random rand = new Random(0);
                double[] xs = DataGen.Consecutive(pointCount, 0.1);
                double[] ys = DataGen.NoisySin(rand, pointCount);

                // optional arguments customize highlighted point color, shape, and size
                var sph = plt.PlotScatterHighlight(xs, ys);

                // you can clear previously-highlighted points
                sph.HighlightPoint(4);
                sph.HighlightClear();

                // highlight the point nearest an X (or Y) position
                plt.PlotVLine(8.123, lineStyle: LineStyle.Dash);
                sph.HighlightPointNearestX(8.123);

                // or highlight the point nearest another point in 2D space
                plt.PlotPoint(4.43, 1.48);
                sph.HighlightPointNearest(4.43, 1.48);
            }
        }
    }
}
