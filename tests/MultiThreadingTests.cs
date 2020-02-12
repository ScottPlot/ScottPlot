using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlotTests
{
    [TestFixture]
    public class MultiThreadingTests
    {
        [Test]
        public void Signal_RenderHighDensityParallel_NotThrows()
        {
            double[] TestArray = Enumerable.Range(0, 5000).Select(x => Math.Sin(x / 10)).ToArray();

            var plt = new ScottPlot.Plot(800, 400);
            plt.GetSettings().misc.useParallel = true;
            plt.PlotSignal(TestArray);

            Assert.DoesNotThrow(() => plt.GetBitmap());
        }

        [Test]
        public void Signal_RenderHighDensityDistributionParallel_NotThrows()
        {
            double[] TestArray = Enumerable.Range(0, 5000).Select(x => Math.Sin(x / 10)).ToArray();
            Color[] Levels = new Color[] { Color.Red, Color.Green, Color.Red };

            var plt = new ScottPlot.Plot(800, 400);
            plt.GetSettings().misc.useParallel = true;
            plt.PlotSignal(TestArray, colorByDensity: Levels);

            Assert.DoesNotThrow(() => plt.GetBitmap());
        }

        [Test]
        public void SignalConst_RenderHighDensityParallel_NotThrows()
        {
            double[] TestArray = Enumerable.Range(0, 5000).Select(x => Math.Sin(x / 10)).ToArray();

            var plt = new ScottPlot.Plot(800, 400);
            plt.GetSettings().misc.useParallel = true;
            plt.PlotSignalConst(TestArray);

            Assert.DoesNotThrow(() => plt.GetBitmap());
        }

        [Test]
        public void GetPixel_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetPixel(x, x * 3));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }

        [Test]
        public void GetPixelX_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetPixelX(x));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }

        [Test]
        public void GetPixelY_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetPixelY(x * 3));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }

        [Test]
        public void GetLocation_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetLocation(x, x * 3));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }

        [Test]
        public void GetLocationX_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetLocationX(x));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }

        [Test]
        public void GetLocationY_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.GetLocationY(x * 3));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }
    }
}
