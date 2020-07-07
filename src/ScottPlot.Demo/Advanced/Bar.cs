using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Advanced
{
    class Bar
    {
        public class MultipleBars : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Multiple Bar Graphs";
            public string description { get; } = "Multiple bar graphs can be displayed together by tweaking the widths and offsets of two separate bar graphs. " +
                "However in most cases this is not necessary because the PlotBar() and PlotPopulation() tools are so robust (see those examples).";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 10;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] ys1 = DataGen.RandomNormal(rand, pointCount, 20, 5);
                double[] ys2 = DataGen.RandomNormal(rand, pointCount, 20, 5);
                double[] err1 = DataGen.RandomNormal(rand, pointCount, 5, 2);
                double[] err2 = DataGen.RandomNormal(rand, pointCount, 5, 2);

                // add both bar plots with a careful widths and offsets
                plt.PlotBar(xs, ys1, err1, "data A", barWidth: .3, xOffset: -.2);
                plt.PlotBar(xs, ys2, err2, "data B", barWidth: .3, xOffset: .2);

                // customize the plot to make it look nicer
                plt.Axis(y1: 0);
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
                plt.Axis(y1: 0);
                plt.Legend(location: legendLocation.upperRight);

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                plt.XTicks(xs, labels);
            }
        }
    }
}
