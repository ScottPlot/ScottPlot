using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Benchmark
{
    class Scatter
    {
        [Test]
        public void Test_scatter_10kPoints()
        {
            double[] pointCounts = { 10, 100, 1000, 10000 };
            double[] reps = { 1000, 100, 25, 10 };
            double[] speeds = new double[pointCounts.Length];

            for (int i = 0; i < pointCounts.Length; i++)
            {
                int pointCount = (int)pointCounts[i];
                var plt = new ScottPlot.Plot();
                Random rand = new Random(0);
                double[] xs = DataGen.Random(rand, pointCount);
                double[] ys = DataGen.RandomWalk(rand, pointCount);
                plt.PlotScatter(xs, ys);
                plt.GetBitmap(renderFirst: true, lowQuality: true);

                List<double> times = new List<double>();
                for (int j = 0; j < reps[i]; j++)
                {
                    plt.GetBitmap(renderFirst: true, lowQuality: true);
                    times.Add(plt.GetSettings(false).Benchmark.msec);
                }

                var stats = new ScottPlot.Statistics.Population(times.ToArray());
                speeds[i] = stats.mean;
                Console.WriteLine($"Rendered {pointCount} points in {stats.mean} ms");
            }

            var plt2 = new ScottPlot.Plot(400, 300);
            plt2.Title("Scatter Plot Benchmark");
            plt2.YLabel("Time (ms)");
            plt2.XLabel("Number of Points");
            plt2.PlotScatter(pointCounts, speeds);
            TestTools.SaveFig(plt2);
        }
    }
}
