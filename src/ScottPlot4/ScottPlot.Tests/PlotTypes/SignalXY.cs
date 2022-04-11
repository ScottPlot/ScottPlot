using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            plt.AddSignalXY(xs, ys);
            plt.SetAxisLimits(20530, 20560, -61, -57);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_NotAllXsAscend_ValidateDataThrowsException()
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
            var signalXY = plt.AddSignalXY(xs, ys);
            Assert.Throws<InvalidOperationException>(() => { signalXY.ValidateData(deep: true); });
        }

        [Test]
        public void Test_OutsideOfRenderLimitsNotAllXsAscend_ValidateDataDoesNotThrowException()
        {
            int pointCount = 1000;
            int minRenderIndex = 100;
            int maxRenderIndex = 200;
            double[] ys = new double[pointCount];
            double[] xs = new double[pointCount];
            for (int i = 0; i < minRenderIndex; i++)
            {
                ys[i] = double.MaxValue;
                xs[i] = double.MaxValue;
            }
            for (int i = minRenderIndex; i <= maxRenderIndex; i++)
            {
                ys[i] = i;
                xs[i] = i;
            }

            var plt = new ScottPlot.Plot(500, 350);
            var signalXY = plt.AddSignalXY(xs, ys);
            signalXY.MinRenderIndex = minRenderIndex;
            signalXY.MaxRenderIndex = maxRenderIndex;
            Assert.DoesNotThrow(() => { signalXY.ValidateData(deep: true); });
        }

        [Test]
        public void Test_SignalXY_RenderLimits()
        {
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Consecutive(100_000);
            double[] ys = ScottPlot.DataGen.RandomWalk(rand, 100_000);
            var plt = new ScottPlot.Plot(500, 350);
            var sig = plt.AddSignalXY(xs, ys);
            sig.MinRenderIndex = 4_000;
            sig.MaxRenderIndex = 5_000;
            plt.SetAxisLimits(yMin: -200, yMax: 200);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_SignalXY_FillBelow_ZoomOut()
        {
            // addresses issue #1476 where zooming far out causes the width of the
            // fill to be zero and a hard crash
            // https://github.com/ScottPlot/ScottPlot/issues/1476

            var plt = new ScottPlot.Plot(400, 300);

            var line = plt.AddSignalXY(
                xs: ScottPlot.DataGen.Consecutive(100),
                ys: ScottPlot.DataGen.RandomWalk(100));

            line.FillBelow();

            for (int i = 0; i < 10; i++)
            {
                plt.AxisZoom(.1, .1);
                plt.Render();
            }
        }
    }
}
