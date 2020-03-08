using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Quickstart
{
    class Quickstart
    {
        public class Scatter : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scatter Plot Quickstart";
            public string description { get; } = "Scatter plots are best for small numbers of paired X/Y data points. For evenly-spaced data points Signal is much faster.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(xs, sin, label: "sin");
                plt.PlotScatter(xs, cos, label: "cos");
                plt.Legend();

                plt.Title("Scatter Plot Quickstart");
                plt.YLabel("Vertical Units");
                plt.XLabel("Horizontal Units");
            }
        }

        public class Signal_5MillionPoints : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5 Million Points";
            public string description { get; } = "The Signal plot type is ideal for displaying evenly-spaced data. Plots with millions of data points can be interacted with in real time. If the underlying data does not change, SignalConst() may be an even more performant way to display it.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = (int)1e6;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));

                plt.Title("Signal Plot Quickstart (5 million points)");
                plt.YLabel("Vertical Units");
                plt.XLabel("Horizontal Units");
            }
        }
    }
}
