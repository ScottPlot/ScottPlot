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
    }
}
