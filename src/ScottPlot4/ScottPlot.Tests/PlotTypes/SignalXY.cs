using NUnit.Framework;
using System;
using System.Linq;

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
            plt.AddSignalXY(xs, ys);
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });
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
            Assert.DoesNotThrow(() => { plt.Render(); });
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

        /// <summary>
        /// This test is for debugging purposes only - it always passes, but is useful for debugging.
        /// It was created to recreate the issue described in https://github.com/ScottPlot/ScottPlot/issues/1803
        /// Before the fix it threw an OverflowException which was silently handled but resulted in no data drawn.
        /// The core issue was that interpolation may result in an infinity point that GDI cannot render.
        /// </summary>
        [Test]
        public void Test_SignalXY_ExtrapolateIdenticalPoints()
        {
            double[] ys = new double[10000];
            double[] xs = ys.Select(x => double.PositiveInfinity).ToArray();
            double r = 0.0;
            for (int i = 0; i < ys.Length; i++)
            {
                r += 0.005;
                xs[i] = r;
                ys[i] = Math.Sin(r);
            }

            var plt = new ScottPlot.Plot(1200, 800);
            plt.AddSignalXY(xs, ys);
            plt.AxisAutoY();
            plt.XAxis.Dims.SetAxis(min: r - 10.0, r);
            plt.Validate(deep: true);

            plt.GetSettings(false).IgnoreOverflowExceptionsDuringRender = false;

            // before the fix this used to throw:
            //Assert.Throws<OverflowException>(() => plt.Render());

            // after ths fix there is no more exception:
            Assert.DoesNotThrow(() => plt.Render());
        }
    }
}
