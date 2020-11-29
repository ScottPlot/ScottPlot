using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Advanced
{
    class MultiAxis
    {
        public class MultiAxisQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Additional Y Axis";
            public string description { get; } =
                "This example demonstrates how to plot data datasets using the same X axis but different Y axes. " +
                "Every plottable has X and Y axis indexes defined in a public field. Axis indexes 0 and 1 are reserved " +
                "for the left and right axes (or bottom and top), and additional axes use higher index numbers. " +
                "Here we add a plottable and an axis and ensure both use an alternate Y axis index.";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 10;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] rainfall = DataGen.Random(rand, pointCount, 1, 10); // small values
                double[] organisms = DataGen.Random(rand, pointCount, 1000, 20000); // large values

                // add data to the plot the traditional way and customize the primary Y axis label
                plt.PlotScatter(xs, rainfall);
                plt.YLabel("Rainfall (cm)");

                // create a new plottable, customize its vertical axis index, then add a vertical axis with the same index
                var scatter2 = plt.PlotScatter(xs, organisms);
                scatter2.VerticalAxisIndex = 3;
                plt.AddAxis(Renderable.Edge.Left, axisIndex: 3, "Organisms", color: scatter2.color);
            }
        }
    }
}
