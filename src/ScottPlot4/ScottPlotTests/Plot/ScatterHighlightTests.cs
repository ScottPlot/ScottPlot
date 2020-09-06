using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ScottPlotTests.Plot
{
    public class PlottableScatterHighlightTestable : PlottableScatterHighlight
    {
        public PlottableScatterHighlightTestable(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle, MarkerShape highlightedShape, Color highlightedColor, double highlightedMarkerSize)
            : base(xs, ys, color, lineWidth, markerSize, label, errorX, errorY, errorLineWidth, errorCapSize, stepDisplay, markerShape, lineStyle, highlightedShape, highlightedColor, highlightedMarkerSize)
        {
        }

        public List<int> HighlightedIndexes
        {
            get
            {
                return Enumerable.Range(0, isHighlighted.Length).Where(x => isHighlighted[x]).ToList();
            }
            set
            {
                HighlightClear();
                foreach (var index in value)
                    isHighlighted[index] = true;
            }
        }
    }

    [TestFixture]
    public class PlottableScatterHighlightTests
    {
        private PlottableScatterHighlightTestable InitWithTestValues()
        {
            double[] xs = new double[] { 1.0, 2.0, 3.0, 4.0 };
            double[] ys = new double[] { 1.0, 2.0, 3.0, 4.0 };
            double[] error = new double[] { 0.1, 0.1, 0.1, 0.1 };
            return new PlottableScatterHighlightTestable(xs, ys, Color.Red, 1, 1, "Test", error, error, 1, 1, false, MarkerShape.openCircle, LineStyle.Solid, MarkerShape.openSquare, Color.Green, 10);
        }

        [Test]
        public void HighlightClear_CallOnNotEmpty_ClearHighlightedIndexes()
        {
            var plottable = InitWithTestValues();
            plottable.HighlightedIndexes = new List<int>() { 1, 2, 3 };

            Assert.IsNotEmpty(plottable.HighlightedIndexes);
            plottable.HighlightClear();
            Assert.IsEmpty(plottable.HighlightedIndexes);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HighlightPoint_ExistedIndexProvided_AddToHighlighted(int index)
        {
            var plottable = InitWithTestValues();
            plottable.xs = new double[] { 1, 2, 3, 4, 5 };
            plottable.ys = new double[] { 1, 2, 3, 4, 5 };

            plottable.HighlightPoint(index);
            Assert.AreEqual(plottable.HighlightedIndexes.Count, 1);
            Assert.AreEqual(index, plottable.HighlightedIndexes[0]);
        }

        [TestCase(-5)]
        [TestCase(-1)]
        [TestCase(15)]
        [TestCase(42)]
        public void HighlightPoint_NonExistedIndexProvided_Throws(int index)
        {
            var plottable = InitWithTestValues();
            plottable.xs = new double[] { 1, 2, 3, 4, 5 };
            plottable.ys = new double[] { 1, 2, 3, 4, 5 };

            Assert.Throws<ArgumentException>(() => { plottable.HighlightPoint(index); });
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
            plottable.xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            plottable.ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };

            Console.WriteLine(x);
            plottable.GetPointNearestX(x);
            plottable.HighlightPointNearestX(x);

            Assert.AreEqual(1, plottable.HighlightedIndexes.Count);
            Assert.AreEqual(expectedIndex, plottable.HighlightedIndexes[0]);
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
            plottable.xs = new double[] { 12, 3, -5, 0, 10, 0, 7 };
            plottable.ys = new double[] { -3, -2, -1, 0, 1, 3, 5 };

            plottable.HighlightPointNearestY(y);

            Assert.AreEqual(1, plottable.HighlightedIndexes.Count);
            Assert.AreEqual(expectedIndex, plottable.HighlightedIndexes[0]);
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
            plottable.xs = new double[] { 1, 0, -1, 0 };
            plottable.ys = new double[] { 0, 1, 0, -1 };

            plottable.HighlightPointNearest(x, y);

            Assert.AreEqual(1, plottable.HighlightedIndexes.Count);
            Assert.AreEqual(expectedIndex, plottable.HighlightedIndexes[0]);
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
            plottable.xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            plottable.ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };

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
            plottable.xs = new double[] { -3, -2, -1, 0, 1, 3, 5 };
            plottable.ys = new double[] { 12, 3, -5, 0, 10, 0, 7 };

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
            plottable.xs = new double[] { 1, 0, -1, 0 };
            plottable.ys = new double[] { 0, 1, 0, -1 };

            var result = plottable.GetPointNearest(x, y);

            Assert.AreEqual(expectedX, result.x, 0.01);
            Assert.AreEqual(expectedY, result.y, 0.01);
        }
    }
}
