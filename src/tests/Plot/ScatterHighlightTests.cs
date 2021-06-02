using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using ScottPlot.Plottable;

namespace ScottPlotTests.Plot
{
    [TestFixture]
    public class PlottableScatterHighlightTests
    {
        private ScatterPlot InitWithTestValues()
        {
            double[] xs = new double[] { 1.0, 2.0, 3.0, 4.0 };
            double[] ys = new double[] { 1.0, 2.0, 3.0, 4.0 };
            double[] error = new double[] { 0.1, 0.1, 0.1, 0.1 };
            return new ScatterPlot(xs, ys, error, error) { Color = Color.Green };
        }

        [TestCase(0, 3)]
        [TestCase(-500, 0)]
        [TestCase(1000, 6)]
        [TestCase(0, 3)]
        [TestCase(-0.4, 3)]
        [TestCase(-0.6, 2)]
        [TestCase(2, 4)]
        [TestCase(2.0000001, 5)]
        public void HighlightPointNearest_SomeXValues_NearestPointIndexAdded(double x, int expectedIndex)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            double[] ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearestX(x);

            Assert.AreEqual(expectedIndex, result.index);
        }

        [TestCase(0, 3)]
        [TestCase(-500, 0)]
        [TestCase(1000, 6)]
        [TestCase(0, 3)]
        [TestCase(-0.4, 3)]
        [TestCase(-0.6, 2)]
        [TestCase(2, 4)]
        [TestCase(2.0000001, 5)]
        public void HighlightPointNearest_SomeYValues_NearestPointIndexAdded(double y, int expectedIndex)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { 12, 3, -5, 0, 10, 0, 7 };
            double[] ys = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearestY(y);

            Assert.AreEqual(expectedIndex, result.index);
        }

        [TestCase(0.1, 0, 0)]
        [TestCase(0.0, 0.1, 1)]
        [TestCase(-0.1, 0.0, 2)]
        [TestCase(0.0, -0.1, 3)]
        [TestCase(1000, 1, 0)]
        [TestCase(1, 1000, 1)]
        public void HighlightPointNearest_SomeXYValues_NearestPointIndexAdded(double x, double y, int expectedIndex)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { 1, 0, -1, 0 };
            double[] ys = new double[] { 0, 1, 0, -1 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearest(x, y);

            Assert.AreEqual(expectedIndex, result.index);
        }

        [TestCase(-3, -3, 12)]
        [TestCase(-500, -3, 12)]
        [TestCase(1000, 5, 7)]
        [TestCase(0, 0, 0)]
        [TestCase(-0.4, 0, 0)]
        [TestCase(-0.6, -1, -5)]
        [TestCase(2, 1, 10)]
        [TestCase(2.0000001, 3, 0)]
        public void GetPointNearest_SomeXValues_ReturnNearestPoint(double x, double expectedX, double expectedY)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            double[] ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearestX(x);

            Assert.AreEqual(expectedX, result.x, 0.01);
            Assert.AreEqual(expectedY, result.y, 0.01);
        }

        [TestCase(-3, -1, -5)]
        [TestCase(-500, -1, -5)]
        [TestCase(1000, -3, 12)]
        [TestCase(0, 0, 0)]
        [TestCase(-0.4, 0, 0)]
        [TestCase(-0.6, 0, 0)]
        [TestCase(2, -2, 3)]
        [TestCase(2.0000001, -2, 3)]
        public void GetPointNearest_SomeYValues_ReturnNearestPoint(double y, double expectedX, double expectedY)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            double[] ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearestY(y);

            Assert.AreEqual(expectedX, result.x, 0.01);
            Assert.AreEqual(expectedY, result.y, 0.01);
        }

        [TestCase(0.1, 0, 1, 0)]
        [TestCase(0.0, 0.1, 0, 1)]
        [TestCase(-0.1, 0.0, -1, 0)]
        [TestCase(0.0, -0.1, 0, -1)]
        [TestCase(1000, 1, 1, 0)]
        [TestCase(1, 1000, 0, 1)]
        public void GetPointNearest_SomeXYValues_NearestPointIndexAdded(double x, double y, double expectedX, double expectedY)
        {
            var plottable = InitWithTestValues();
            double[] xs = new double[] { 1, 0, -1, 0 };
            double[] ys = new double[] { 0, 1, 0, -1 };
            plottable.Update(xs, ys);

            var result = plottable.GetPointNearest(x, y);

            Assert.AreEqual(expectedX, result.x, 0.01);
            Assert.AreEqual(expectedY, result.y, 0.01);
        }
    }
}
