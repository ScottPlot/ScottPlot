using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot;

namespace ScottPlot.Demo.General
{
    public class Plots
    {
        // TODO: pull these from their source pages rather than duplicating them here

        public class SinAndCos : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Sin and Cos";
            public string description { get; } = "The scatter plot is a simple way to display paired X/Y data.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] xs = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(xs, sin, label: "sin");
                plt.PlotScatter(xs, cos, label: "cos");
                plt.Legend();

                plt.Title("ScottPlot Quickstart");
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
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
            }
        }

        public class Clear : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Clearing data";
            public string description { get; } = "Plots can be cleared using the Clear() method. Arguments let the user customize which types of plot objects to clear.";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(-5, 5, 0.1);
                double[] sin = DataGen.Sin(xs);
                double[] cos = DataGen.Cos(xs);

                plt.PlotScatter(xs, sin);
                plt.PlotScatter(xs, cos);
                plt.Clear();
                plt.PlotScatter(sin, xs, color: Color.Magenta, lineStyle: LineStyle.Dot, markerSize: 0);
            }
        }
    }
}
