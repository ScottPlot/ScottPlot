using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScottPlotTests
{
    public class PlotTests
    {
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void Test_Plot_InvalidDimensionsThrow(int width, int height)
        {
            var plt = new ScottPlot.Plot();
            Assert.Throws<ArgumentException>(() => { plt.Render(width, height); });
        }

        [Test]
        public void Test_Plot_Render()
        {
            var plt = new ScottPlot.Plot();

            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };
            plt.PlotScatter(xs, ys);

            TestTools.SaveFig(plt);
        }

        [TestCase(1, 1)]
        [TestCase(640, 480)]
        [TestCase(7680, 4320)]
        public void Test_InstantiatedDimensions_AreRemembered(int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            Assert.AreEqual(width, plt.Width);
            Assert.AreEqual(height, plt.Height);
        }

        [TestCase(1, 1)]
        [TestCase(640, 480)]
        [TestCase(7680, 4320)]
        public void Test_InstantiatedDimensions_AreDefaultRenderDimensions(int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            Bitmap bmp = plt.Render();
            Assert.AreEqual(width, bmp.Width);
            Assert.AreEqual(height, bmp.Height);
        }

        [TestCase(1, 1)]
        [TestCase(640, 480)]
        [TestCase(7680, 4320)]
        public void Test_InstantiatedDimensions_AreOverriddenByRenderDimensions(int width, int height)
        {
            var plt = new ScottPlot.Plot(333, 222);

            Bitmap bmp = plt.Render(width, height);
            Assert.AreEqual(width, bmp.Width);
            Assert.AreEqual(height, bmp.Height);

            Assert.AreEqual(width, plt.Width);
            Assert.AreEqual(height, plt.Height);
        }
    }
}