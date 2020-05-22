using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Experimental
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

                PlottableScatterHighlight highlightPlot = plt.PlotScatterHighlight(x, rand);

                highlightPlot.HighlightPoint(4);
                highlightPlot.HighlightPointNearest(8);
                highlightPlot.HighlightPointNearest(5, 1);
                //highlightPlot.HighlightClear();
            }
        }
    }
}
