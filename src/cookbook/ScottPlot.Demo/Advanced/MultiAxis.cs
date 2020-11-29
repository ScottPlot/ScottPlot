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
                // generate sample data to plot
                Random rand = new Random(0);
                int pointCount = 100;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] rainfall = DataGen.RandomWalk(rand, pointCount, 1, 10); // small values
                double[] organisms = DataGen.RandomWalk(rand, pointCount, 1000, 20000); // large values

                // customize the primary X axis
                plt.XAxis.Configure(label: "Days After Volcano Eruption");

                // add a scatter plot and customize the primary Y axis
                var scatter1 = plt.PlotScatter(xs, rainfall);
                plt.YAxis.Configure(label: "Daily Rainfall (cm)", color: scatter1.color);

                // add another scatter plot but give it a custom vertical axis index
                var scatter2 = plt.PlotScatter(xs, organisms);
                scatter2.VerticalAxisIndex = 3;

                // add a new vertical axis using same axis index as the second scatter plot
                var secondLeftAxis = plt.AddAxis(Renderable.Edge.Left, axisIndex: 3);
                secondLeftAxis.Configure(label: "Number of Organisms", color: scatter2.color);
            }
        }
    }
}
