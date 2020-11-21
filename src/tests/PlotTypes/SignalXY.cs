using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    class SignalXY
    {
        [Test]
        public void Test_AscendingUnevenlySpacedXs_ShouldRenderWell()
        {
            // generate random, ascending, unevenly-spaced data
            Random rand = new Random(0);
            int pointCount = 100_000;
            double[] ys = new double[pointCount];
            double[] xs = new double[pointCount];
            for (int i = 1; i < ys.Length; i++)
            {
                ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                xs[i] = xs[i - 1] + rand.NextDouble();
            }

            var plt = new ScottPlot.Plot(500, 350);
            plt.PlotSignalXY(xs, ys);
            plt.Axis(20530, 20560, -61, -57);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_NotAllXsAscend_ThrowsException()
        {
            // generate random, NOT-ALL-ascending, unevenly-spaced data
            Random rand = new Random(0);
            int pointCount = 100_000;
            double[] ys = new double[pointCount];
            double[] xs = new double[pointCount];
            for (int i = 1; i < ys.Length; i++)
            {
                ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                xs[i] = xs[i - 1] + rand.NextDouble() - .25; // may be <0
            }

            var plt = new ScottPlot.Plot(500, 350);
            Assert.Throws<ArgumentException>(() => { plt.PlotSignalXY(xs, ys); });
        }

        [Test]
        public void Test_SignalXY_RenderLimits()
        {
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Consecutive(100_000);
            double[] ys = ScottPlot.DataGen.RandomWalk(rand, 100_000);
            var plt = new ScottPlot.Plot(500, 350);
            plt.PlotSignalXY(xs, ys, minRenderIndex: 4000, maxRenderIndex: 5000);
            plt.Axis(y1: -200, y2: 200);
            TestTools.SaveFig(plt);
        }
    }
}
