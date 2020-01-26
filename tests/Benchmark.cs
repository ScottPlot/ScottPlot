using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    class Benchmark
    {
        [Test]
        public void Test_Benchmark_Scatter()
        {
            var plt = new ScottPlot.Plot();

            int pointCount = 1_000_000;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys = ScottPlot.DataGen.RandomWalk(new Random(0), pointCount);

            plt.Title($"Scatter Plot ({pointCount.ToString("N0")} points)");
            plt.PlotScatter(xs, ys, markerShape: ScottPlot.MarkerShape.none);
            plt.Benchmark(true);

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
            Console.WriteLine(plt.GetSettings(false).benchmark);

            // 2019-01-26: Full render of 1 object (1,000,000 points) took 1275.760 ms (0.78 Hz)
        }
    }
}
