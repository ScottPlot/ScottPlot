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
                double[] x = DataGen.Consecutive(pointCount, 0.1);
                double[] rand = DataGen.Random(new Random(), pointCount);

                //Unless otherwise specified, shape and colour are the same as unhighlighted points
                //Default marker size is double the size of unhighlighted points
                PlottableScatterHighlight highlightPlot = plt.PlotScatterHighlight(x, rand, highlightedShape: MarkerShape.filledSquare, highlightedColor: Color.Red, highlightedMarkerSize: 8);

                highlightPlot.HighlightPoint(4);
                highlightPlot.HighlightPointNearestX(8);
                highlightPlot.HighlightPointNearest(5, 1);
                //highlightPlot.HighlightClear();
            }
        }
    }
}
