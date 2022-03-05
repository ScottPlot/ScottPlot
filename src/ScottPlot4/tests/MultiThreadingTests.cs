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
            plt.AddSignal(TestArray);

            Assert.DoesNotThrow(() => plt.Render());
        }

        [Test]
        public void Signal_RenderHighDensityDistributionParallel_NotThrows()
        {
            double[] TestArray = Enumerable.Range(0, 5000).Select(x => Math.Sin(x / 10)).ToArray();
            Color[] Levels = new Color[] { Color.Red, Color.Green, Color.Red };

            var plt = new ScottPlot.Plot(800, 400);
            var sig = plt.AddSignal(TestArray);
            sig.DensityColors = Levels;

            Assert.DoesNotThrow(() => plt.Render());
        }

        [Test]
        public void SignalConst_RenderHighDensityParallel_NotThrows()
        {
            double[] TestArray = Enumerable.Range(0, 5000).Select(x => Math.Sin(x / 10)).ToArray();

            var plt = new ScottPlot.Plot(800, 400);
            plt.AddSignalConst(TestArray);

            Assert.DoesNotThrow(() => plt.Render());
        }

        [Test]
        public void GetPixelX_MultipleParallelCalls_NotThrows()
        {
            var plt = new ScottPlot.Plot(800, 400);
            var settings = plt.GetSettings();

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.XAxis.Dims.GetPixel(x));

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

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.YAxis.Dims.GetPixel(x * 3));

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

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.XAxis.Dims.GetUnit(x));

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

            var pixels = Enumerable.Range(0, 5000).AsParallel().Select(x => settings.YAxis.Dims.GetUnit(x * 3));

            Assert.DoesNotThrow(() =>
            {
                var array = pixels.ToArray();
            });
        }
    }
}
