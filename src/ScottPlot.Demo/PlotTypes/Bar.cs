using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Bar
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar Graph Quickstart";
            public string description { get; } = "Bar graph series can be created by supply Xs and Ys. Optionally apply errorbars as a third array using an argument.";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 10;
                double[] Xs = new double[pointCount];
                double[] dataA = new double[pointCount];
                double[] errorA = new double[pointCount];
                for (int i = 0; i < pointCount; i++)
                {
                    Xs[i] = i * 10;
                    dataA[i] = rand.NextDouble() * 100;
                    errorA[i] = rand.NextDouble() * 10;
                }

                // make the bar plot
                plt.PlotBar(Xs, dataA, errorY: errorA);

                // customize the plot to make it look nicer
                plt.Axis(null, null, 0, null);
                plt.Grid(false);

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                plt.XTicks(Xs, labels);
            }
        }

        public class MultipleBars : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Multiple Bar Graphs";
            public string description { get; } = "Multiple bar graphs can be displayed together by tweaking the widths and offsets of two separate bar graphs.";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 10;
                double[] Xs = new double[pointCount];
                double[] dataA = new double[pointCount];
                double[] errorA = new double[pointCount];
                double[] dataB = new double[pointCount];
                double[] errorB = new double[pointCount];
                for (int i = 0; i < pointCount; i++)
                {
                    Xs[i] = i * 10;
                    dataA[i] = rand.NextDouble() * 100;
                    dataB[i] = rand.NextDouble() * 100;
                    errorA[i] = rand.NextDouble() * 10;
                    errorB[i] = rand.NextDouble() * 10;
                }

                // add both bar plots with a careful widths and offsets
                plt.PlotBar(Xs, dataA, errorY: errorA, label: "data A", barWidth: 3.2, xOffset: -2);
                plt.PlotBar(Xs, dataB, errorY: errorB, label: "data B", barWidth: 3.2, xOffset: 2);

                // customize the plot to make it look nicer
                plt.Grid(false);
                plt.Axis(null, null, 0, null);
                plt.Legend();

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                plt.XTicks(Xs, labels);
            }
        }
    }
}
