using NUnit.Framework;
using System;

namespace ScottPlotTests
{
    public class PlotTests
    {
        [TestCase(1, 1)]
        [TestCase(640, 480)]
        [TestCase(7680, 4320)]
        public void Test_Plot_InstantiatedDimensionsAreRetained(int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            Assert.AreEqual(width, plt.Width);
            Assert.AreEqual(height, plt.Height);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void Test_Plot_InvalidDimensionsThrow(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => { new ScottPlot.Plot(width, height); });
        }

        [Test]
        public void Test_Plot_Render()
        {
            var plt = new ScottPlot.Plot();

            double[] xs = { 1, 2, 3 };
            double[] ys = { 1, 4, 9 };
            plt.PlotScatter(xs, ys);

            plt.Render();
        }
    }
}