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
            plt.PlotScatter(xs, ys, markerSize: 0);
            plt.Benchmark(true);

            // the first render allocates memory for pointF arrays
            plt.GetBitmap(true);
            Console.WriteLine($"Render 1: {plt.GetSettings(false).benchmark}");

            // subsequent renders re-use the same arrays
            plt.GetBitmap(true);
            Console.WriteLine($"Render 2: {plt.GetSettings(false).benchmark}");

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");


            // 2019-01-26
            //   Render 1: Full render of 1 object(1,000,000 points) took 1269.239 ms(0.79 Hz)
            //   Render 2: Full render of 1 object(1,000,000 points) took 1229.764 ms(0.81 Hz) <-- 3% faster
        }

        [Test]
        public void Test_Benchmark_Step()
        {
            var plt = new ScottPlot.Plot();

            int pointCount = 200_000;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys = ScottPlot.DataGen.RandomWalk(new Random(0), pointCount);

            plt.Title($"Scatter Plot ({pointCount.ToString("N0")} points)");
            plt.PlotStep(xs, ys);
            plt.Benchmark(true);

            // the first render allocates memory for pointF arrays
            plt.GetBitmap(true);
            Console.WriteLine($"Render 1: {plt.GetSettings(false).benchmark}");

            // subsequent renders re-use the same arrays
            plt.GetBitmap(true);
            Console.WriteLine($"Render 2: {plt.GetSettings(false).benchmark}");

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");


            // 2019-01-26
            //   Render 1: Full render of 1 object (200,000 points) took 490.078 ms (2.04 Hz)
            //   Render 2: Full render of 1 object (200,000 points) took 454.589 ms (2.20 Hz) <-- 7% faster
        }
    }
}
