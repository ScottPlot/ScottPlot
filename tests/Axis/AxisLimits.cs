using NUnit.Framework;
using ScottPlot;
using System;

namespace ScottPlotTests.Axis
{
    class AxisLimits
    {
        [Test]
        public void Test_EmptyConstructor_StartsAsNaN()
        {
            var limits = new AxisLimits2D();
            Console.WriteLine(limits);

            Assert.IsNaN(limits.XMin);
            Assert.IsNaN(limits.XMax);
            Assert.IsNaN(limits.YMin);
            Assert.IsNaN(limits.YMax);
        }

        [Test]
        public void Test_Constructor_StoresValues()
        {
            var limits = new AxisLimits2D(11, 22, 33, 44);
            Console.WriteLine(limits);

            Assert.AreEqual(11, limits.XMin);
            Assert.AreEqual(22, limits.XMax);
            Assert.AreEqual(33, limits.YMin);
            Assert.AreEqual(44, limits.YMax);
        }

        [Test]
        public void Test_ExpandX_ExpectedOutput()
        {
            var limits = new AxisLimits2D();

            limits.Expand(-20, 20, null, null);

            Assert.AreEqual(limits.XMin, -20);
            Assert.AreEqual(limits.XMax, 20);
            Assert.IsNaN(limits.YMin);
            Assert.IsNaN(limits.YMax);
        }

        [Test]
        public void Test_ExpandY_ExpectedOutput()
        {
            var limits = new AxisLimits2D();

            limits.Expand(null, null, -20, 20);

            Assert.IsNaN(limits.XMin);
            Assert.IsNaN(limits.XMax);
            Assert.AreEqual(limits.YMin, -20);
            Assert.AreEqual(limits.YMax, 20);
        }

        [Test]
        public void Test_ExpandXY_ExpectedOutput()
        {
            var limits = new AxisLimits2D();

            limits.Expand(-10, 20, -30, 40);

            Assert.AreEqual(limits.XMin, -10);
            Assert.AreEqual(limits.XMax, 20);
            Assert.AreEqual(limits.YMin, -30);
            Assert.AreEqual(limits.YMax, 40);
        }
    }
}
