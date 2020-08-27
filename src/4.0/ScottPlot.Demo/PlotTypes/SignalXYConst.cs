using System;
using System.Linq;

namespace ScottPlot.Demo.PlotTypes
{
    class SignalXYConst
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "SignalConst with X and Y data";
            // TODO describe Const benefits in description
            public string description { get; } = "SignalXYConst is a speed-optimized plot for displaying vaues (Ys) with unevenly-spaced positions (Xs) that are in ascending order. If your data is evenly-spaced, Signal and SignalConst is faster.";

            public void Render(Plot plt)
            {
                // generate random, unevenly-spaced data
                Random rand = new Random(0);
                int pointCount = 100_000;
                double[] ys = new double[pointCount];
                double[] xs = new double[pointCount];
                for (int i = 1; i < ys.Length; i++)
                {
                    ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                    xs[i] = xs[i - 1] + rand.NextDouble();
                }

                plt.Title($"SignalXY Plot ({pointCount:N0} points)");
                plt.PlotSignalXYConst(xs, ys);
            }
        }

        public class MixedDataTypes : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Different data types for xs and ys";
            // TODO make detailed description
            public string description { get; } = "SignalXYConst with (int)Xs and (float)Ys arrays";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;

                double[] sine = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 0.5);

                float[] ys = sine.Zip(noise, (s, n) => s + n).Select(x => (float)x).ToArray();

                int[] xs = Enumerable.Range(0, pointCount)
                    .Select(x => (int)x)
                    .Select(x => x > 500_000 ? x + 1_000_000 : x)
                    .Select(x => x > 200_000 ? x + 100_000 : x)
                    .ToArray();

                plt.PlotSignalXYConst(xs, ys);
            }
        }

        public class HeavyLoadSignalXYConst : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Lots of points SignalXYConst";
            public string description { get; } = "SignalXYConst with 100_000_000 points";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100_000_000;

                double[] sine = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 0.5);

                double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();

                double[] xs = new double[pointCount];
                double currentX = 0;
                for (int i = 0; i < pointCount; i++)
                {
                    if ((i % 100000) < 10)
                        currentX += 10;
                    else
                        currentX += 0.0001;
                    xs[i] = currentX;
                }
                plt.PlotSignalXYConst(xs, ys);
            }
        }
        public class HeavyLoadSignalXY : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Lots of points SignalXY (Slow)";
            public string description { get; } = "SignalXY with 100_000_000 points";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100_000_000;

                double[] sine = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 0.5);

                double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();

                double[] xs = new double[pointCount];
                double currentX = 0;
                for (int i = 0; i < pointCount; i++)
                {
                    if ((i % 100000) < 10)
                        currentX += 10;
                    else
                        currentX += 0.0001;
                    xs[i] = currentX;
                }
                plt.PlotSignalXY(xs, ys);
            }
        }
    }
}
