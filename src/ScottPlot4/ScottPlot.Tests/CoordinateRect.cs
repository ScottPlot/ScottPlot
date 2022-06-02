using System;
using NUnit.Framework;

namespace ScottPlotTests
{
    internal class CoordinateRect
    {
        [Test]
        public void Test_CoordinateRect_Construction()
        {
            ScottPlot.CoordinateRect rect = new(-10, 10, -20, 20);
            Console.WriteLine(rect);

            Assert.That(rect.XMin, Is.EqualTo(-10));
            Assert.That(rect.XMax, Is.EqualTo(+10));
            Assert.That(rect.YMin, Is.EqualTo(-20));
            Assert.That(rect.YMax, Is.EqualTo(+20));
        }

        [Test]
        public void Test_CoordinateRect_Construction_InvertedXsAndYs()
        {
            ScottPlot.CoordinateRect rect = new(10, -10, 20, -20);
            Console.WriteLine(rect);

            Assert.That(rect.XMin, Is.EqualTo(-10));
            Assert.That(rect.XMax, Is.EqualTo(+10));
            Assert.That(rect.YMin, Is.EqualTo(-20));
            Assert.That(rect.YMax, Is.EqualTo(+20));
        }

        [Test]
        public void Test_CoordinateRect_ZeroArea()
        {
            ScottPlot.CoordinateRect rect = new(10, 10, 10, 10);
            Console.WriteLine(rect);

            Assert.That(rect.Area, Is.Zero);
            Assert.That(rect.HasArea, Is.False);
        }

        [Test]
        public void Test_CoordinateRect_DoesNotAllowNan()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ScottPlot.CoordinateRect(10, 10, double.NaN, 10));
        }

        [Test]
        public void Test_CoordinateRect_DoesNotAllowInfinity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ScottPlot.CoordinateRect(10, 10, double.PositiveInfinity, 10));
        }

        [Test]
        public void Test_CoordinateRect_Contains()
        {
            ScottPlot.CoordinateRect rect = new(10, -10, 20, -20);
            Console.WriteLine(rect);

            Assert.That(rect.Contains(1, 2), Is.True);
            Assert.That(rect.Contains(-1, -2), Is.True);

            Assert.That(rect.Contains(123, 456), Is.False);
            Assert.That(rect.Contains(-123, -123), Is.False);

            Assert.That(rect.Contains(1, 456), Is.False);
            Assert.That(rect.Contains(123, 1), Is.False);
        }

        [Test]
        public void Test_CoordinateRect_BoundingBox()
        {
            ScottPlot.Coordinate[] coordinates =
            {
                new ScottPlot.Coordinate(0,0),
                new ScottPlot.Coordinate(-5, -4),
                new ScottPlot.Coordinate(7, 8),
                new ScottPlot.Coordinate(1,2),
            };

            ScottPlot.CoordinateRect rect = ScottPlot.CoordinateRect.BoundingBox(coordinates);
            Assert.That(rect.XMin, Is.EqualTo(-5));
            Assert.That(rect.XMax, Is.EqualTo(7));
            Assert.That(rect.YMin, Is.EqualTo(-4));
            Assert.That(rect.YMax, Is.EqualTo(8));
        }
    }
}
