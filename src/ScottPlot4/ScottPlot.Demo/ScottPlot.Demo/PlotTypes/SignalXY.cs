using System;
using System.Linq;

namespace ScottPlot.Demo.PlotTypes
{
    class SignalXY
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Signal with X and Y data";
            public string description { get; } = "SignalXY is a speed-optimized plot for displaying vaues (Ys) with unevenly-spaced positions (Xs) that are in ascending order. If your data is evenly-spaced, Signal and SignalConst is faster.";

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
                plt.PlotSignalXY(xs, ys);
            }
        }

        public class HasXGaps : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Big Gaps";
            public string description { get; } = "Signal with defined Xs that contain gaps";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;

                double[] sine = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 0.5);

                double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();

                double[] xs = Enumerable.Range(0, pointCount)
                    .Select(x => (double)x)
                    .Select(x => x > 500_000 ? x + 1_000_000 : x)
                    .Select(x => x > 200_000 ? x + 100_000 : x)
                    .ToArray();

                plt.PlotSignalXY(xs, ys);
            }
        }

        public class SignalWithDifferentDensity : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Different Densities";
            public string description { get; } = "Signal with mised low and high density data";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;

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
