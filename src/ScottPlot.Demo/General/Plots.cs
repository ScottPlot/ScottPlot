using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot;

namespace ScottPlot.Demo.General
{
    public class Plots
    {
        // TODO: pull these from their source pages rather than duplicating them here

        public class SinAndCos : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Sin and Cos (Scatter)";
            public string description { get; } = "Simple scatter plot with a legend.";

            public void Render(Plot plt)
            {
                int pointCount = 100;
                plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Sin(pointCount));
                plt.PlotScatter(DataGen.Consecutive(pointCount), DataGen.Cos(pointCount));
                plt.Legend();
            }
        }

        public class RandomWalk_fiveThousandPoints_Signal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5k points (Signal)";
            public string description { get; } = "Signal plots are intended for evenly-spaced data and much faster than Scatter plots.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
            }
        }

        public class RandomWalk_5millionPoints_Signal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5M points (Signal)";
            public string description { get; } = "Signal plots with millions of points can be interacted with in real time.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
            }
        }

        public class RandomWalk_5millionPoints_SignalConst : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5M points (SignalConst)";
            public string description { get; } = "SignalConst plots pre-processes data to render much faster than Signal plots. Pre-processing takes a little time up-front and requires 4x the memory of Signal.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignalConst(DataGen.RandomWalk(rand, pointCount));
            }
        }
    }
}
