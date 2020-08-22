using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Renderable
{
    class Benchmark
    {
        [Test]
        public void Test_Benchmark_Visible()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            var meanDefault = TestTools.MeanPixel(plt.GetBitmap());
            plt.Benchmark(show: true);
            var meanBenchOn = TestTools.MeanPixel(plt.GetBitmap());
            plt.Benchmark(show: false);
            var meanBenchOff = TestTools.MeanPixel(plt.GetBitmap());

            // appearance of the benchmark will lessen mean pixel intensity
            Assert.AreEqual(meanDefault.R, meanBenchOff.R);
            Assert.Less(meanBenchOn.R, meanDefault.R);
        }

        [Test]
        public void Test_Benchmark_Toggle()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            var meanDefault = TestTools.MeanPixel(plt.GetBitmap());
            plt.Benchmark(toggle: true);
            var meanBenchOn = TestTools.MeanPixel(plt.GetBitmap());
            plt.Benchmark(toggle: true);
            var meanBenchOff = TestTools.MeanPixel(plt.GetBitmap());

            // appearance of the benchmark will lessen mean pixel intensity
            Assert.AreEqual(meanDefault.R, meanBenchOff.R);
            Assert.Less(meanBenchOn.R, meanDefault.R);
        }
    }
}
