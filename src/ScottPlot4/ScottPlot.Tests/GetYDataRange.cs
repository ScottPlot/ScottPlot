using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    internal class GetYDataRange
    {
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, new double[] { 10, 12, 14, 8, 10 }, 1, 5, 8, 14)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, new double[] { 10, 12, 14, 8, 10 }, 1, 3, 10, 14)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, new double[] { 10, 12, 14, 8, 10 }, 3, 3, 14, 14)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, new double[] { 10, 12, 14, 8, 10 }, 2.1, 3.9, 14, 14)]
        public void Scatter(double[] xs, double[] ys, double xStart, double xEnd, double expectedMin, double expectedMax)
        {
            var plt = new ScottPlot.Plot();
            var scat = plt.AddScatter(xs, ys);
            (double yMin, double yMax) = scat.GetYDataRange(xStart, xEnd);

            Assert.AreEqual(expectedMin, yMin);
            Assert.AreEqual(expectedMax, yMax);
        }

        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 0, 4, 8, 14)]
        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 0, 2, 10, 14)]
        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 2, 2, 14, 14)]
        public void Signal(double[] ys, double xStart, double xEnd, double expectedMin, double expectedMax)
        {
            var plt = new ScottPlot.Plot();
            var sig = plt.AddSignal(ys);
            (double yMin, double yMax) = sig.GetYDataRange(xStart, xEnd);

            Assert.AreEqual(expectedMin, yMin);
            Assert.AreEqual(expectedMax, yMax);
        }

        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 0, 4, 8, 14)]
        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 0, 2, 10, 14)]
        [TestCase(new double[] { 10, 12, 14, 8, 10 }, 2, 2, 14, 14)]
        public void SignalWithXOffsetAndSampleRate(double[] ys, double xStart, double xEnd, double expectedMin, double expectedMax)
        {
            const double sampleRate = 25;
            const double xOffset = -100;

            var plt = new ScottPlot.Plot();
            var sig = plt.AddSignal(ys, sampleRate: sampleRate);
            sig.OffsetX = xOffset;
            (double yMin, double yMax) = sig.GetYDataRange(xStart * sampleRate + xOffset, xEnd * sampleRate + xOffset);

            Assert.AreEqual(expectedMin, yMin);
            Assert.AreEqual(expectedMax, yMax);
        }
    }
}
