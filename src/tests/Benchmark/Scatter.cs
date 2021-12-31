using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Benchmark
{
    class Scatter
    {
        [Test]
        public void Test_scatter_benchmark()
        {
            int[] pointCounts = { 1000, 100, 10 };
            const int REPS = 10;

            Random rand = new(0);
            for (int i = 0; i < pointCounts.Length; i++)
            {
                int pointCount = pointCounts[i];
                var plt = new ScottPlot.Plot();
                double[] xs = DataGen.Random(rand, pointCount);
                double[] ys = DataGen.RandomWalk(rand, pointCount);
                plt.AddScatter(xs, ys);
                plt.Render(lowQuality: true);

                for (int j = 0; j < REPS; j++)
                {
                    plt.Render(lowQuality: true);
                }

                double[] renderTimes = plt.BenchmarkTimes();
                Assert.AreEqual(REPS + 1, renderTimes.Length);

                double meanTime = renderTimes.Sum() / renderTimes.Length;
                Console.WriteLine($"Rendered {pointCount} points in {meanTime:N2} ms (n={renderTimes.Length})");
            }
        }
    }
}
