﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Plot
{
    [TestFixture]
    public class PlotTests
    {
        private (double xUnitsPerPixel, double yUnitsPerPixel) getUnitsPerPixel(ScottPlot.Plot plt)
        {
            var settings = plt.GetSettings(false);
            var limits = new ScottPlot.Config.AxisLimits2D(settings.axes.ToArray());

            var xUnitsPerPixel = limits.xSpan / settings.dataSize.Width;
            var yUnitsPerPixel = limits.ySpan / settings.dataSize.Height;
            return (xUnitsPerPixel, yUnitsPerPixel);
        }

        [Test]
        public void AutoAxis_EqualAxis_UnitsPerPixelEqual()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotLine(0, 0, 5, 1);
            plt.EqualAxis = true;
            plt.AxisAuto();

            var (xUnitsPerPixel, yUnitsPerPixel) = getUnitsPerPixel(plt);
            Assert.AreEqual(xUnitsPerPixel, yUnitsPerPixel, xUnitsPerPixel * 0.000001);
        }

        [Test]
        public void AutoAxis_EqualAxisOnScatter_UnitsPerPixelEqual()
        {
            double[] xs = new double[] { 1, 5, 7, 19, 42 };
            double[] ys = new double[] { 51, -5, 6, 12, 3 };
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(xs, ys);
            plt.EqualAxis = true;
            plt.AxisAuto();

            var (xUnitsPerPixel, yUnitsPerPixel) = getUnitsPerPixel(plt);
            Assert.AreEqual(xUnitsPerPixel, yUnitsPerPixel, xUnitsPerPixel * 0.000001);
        }

        [TestCase(640, 300)]
        [TestCase(2000, 300)]
        [TestCase(1000, 1000)]
        [TestCase(400, 1500)]
        public void AutoAxis_EqualAxisOnScatterDifferentResolutions_UnitsPerPixelEqual(int width, int height)
        {
            double[] xs = new double[] { 1, 5, 7, 19, 42 };
            double[] ys = new double[] { 51, -5, 6, 12, 3 };
            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(xs, ys);
            plt.EqualAxis = true;
            plt.AxisAuto();

            var (xUnitsPerPixel, yUnitsPerPixel) = getUnitsPerPixel(plt);
            Assert.AreEqual(xUnitsPerPixel, yUnitsPerPixel, xUnitsPerPixel * 0.000001);
        }

        [TestCase(912, 542)]
        [TestCase(1920, 1080)]
        [TestCase(1000, 1000)]
        [TestCase(700, 600)]
        public void Resize_EqualAxisOnScatter_UnitsPerPixelEqual(int width, int height)
        {
            double[] xs = new double[] { 1, 5, 7, 19, 42 };
            double[] ys = new double[] { 51, -5, 6, 12, 3 };
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(xs, ys);
            plt.EqualAxis = true;
            plt.AxisAuto();

            plt.Resize(width, height);

            var (xUnitsPerPixel, yUnitsPerPixel) = getUnitsPerPixel(plt);
            Assert.AreEqual(xUnitsPerPixel, yUnitsPerPixel, xUnitsPerPixel * 0.000001);
        }

        [TestCase(0, 5)]
        [TestCase(12, 5)]
        [TestCase(18, 0)]
        [TestCase(-6, 5)]
        [TestCase(-6, 6)]
        public void Zoom_EqualAxisOnScatter_UnitsPerPixelEqual(int dx, int dy)
        {
            double[] xs = new double[] { 1, 5, 7, 19, 42 };
            double[] ys = new double[] { 51, -5, 6, 12, 3 };
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(xs, ys);
            plt.EqualAxis = true;
            plt.AxisAuto();

            plt.GetSettings(false).AxesZoomPx(dx, dy);

            var (xUnitsPerPixel, yUnitsPerPixel) = getUnitsPerPixel(plt);
            Assert.AreEqual(xUnitsPerPixel, yUnitsPerPixel, xUnitsPerPixel * 0.000001);
        }
    }
}
