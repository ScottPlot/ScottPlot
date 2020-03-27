using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Bar
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar Plot Quickstart";
            public string description { get; } = "Bar graph series can be created by supply Xs and Ys. Optionally apply errorbars as a third array using an argument.";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 10;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] ys = DataGen.RandomNormal(rand, pointCount, 20, 5);
                double[] yError = DataGen.RandomNormal(rand, pointCount, 5, 2);

                // make the bar plot
                plt.PlotBar(xs, ys, yError);

                // customize the plot to make it look nicer
                plt.Axis(y1: 0);
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                plt.XTicks(xs, labels);
            }
        }

        public class BarWithError : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar Plot with Errorbars";
            public string description { get; } = "Error can be supplied as a command line argument.";

            public void Render(Plot plt)
            {
                int pointCount = 20;
                double[] xs = new double[pointCount];
                double[] ys = new double[pointCount];
                double[] yErr = new double[pointCount];
                Random rand = new Random(0);
                for (int i = 0; i < pointCount; i++)
                {
                    xs[i] = i;
                    ys[i] = .5 + rand.NextDouble();
                    yErr[i] = rand.NextDouble() * .3 + .05;
                }

                plt.Title("Bar Plot With Error Bars");
                plt.PlotBar(xs, ys, barWidth: .5, errorY: yErr, errorCapSize: 2);
                plt.Grid(enableVertical: false);
                plt.Grid(lineStyle: LineStyle.Dot);
                plt.Axis(null, null, 0, null);
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
                plt.Grid(lineStyle: LineStyle.Dot);
                plt.Axis(null, null, 0, null);
                plt.Legend();

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                plt.XTicks(Xs, labels);
            }
        }

        public class Horizontal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Horizontal Bar Graph";
            public string description { get; } = "Bar graphs can be displayed horizontally.";

            public void Render(Plot plt)
            {
                // generate random data to plot
                Random rand = new Random(0);
                int pointCount = 5;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] ys = DataGen.RandomNormal(rand, pointCount, 20, 5);
                double[] yError = DataGen.RandomNormal(rand, pointCount, 3, 2);

                // make the bar plot
                plt.PlotBar(xs, ys, yError, horizontal: true);

                // customize the plot to make it look nicer
                plt.Axis(x1: 0);
                plt.Grid(enableHorizontal: false, lineStyle: LineStyle.Dot);

                // apply custom axis tick labels
                string[] labels = { "one", "two", "three", "four", "five"};
                plt.YTicks(xs, labels);
            }
        }
    }
}