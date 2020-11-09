using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Axis
{
    class AxisLimits
    {
        [Test]
        public void Test_EmptyConstructor_StartsAsNaN()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();
            Console.WriteLine(limits);

            Assert.IsNaN(limits.x1);
            Assert.IsNaN(limits.x2);
            Assert.IsNaN(limits.y1);
            Assert.IsNaN(limits.y2);
        }

        [Test]
        public void Test_Constructor_StoresValues()
        {
            var limits = new ScottPlot.Config.AxisLimits2D(11, 22, 33, 44);
            Console.WriteLine(limits);

            Assert.AreEqual(11, limits.x1);
            Assert.AreEqual(22, limits.x2);
            Assert.AreEqual(33, limits.y1);
            Assert.AreEqual(44, limits.y2);
        }

        [Test]
        public void Test_ExpandX_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.Expand(-20, 20, null, null);

            Assert.AreEqual(limits.x1, -20);
            Assert.AreEqual(limits.x2, 20);
            Assert.IsNaN(limits.y1);
            Assert.IsNaN(limits.y2);
        }

        [Test]
        public void Test_ExpandY_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.Expand(null, null, -20, 20);

            Assert.IsNaN(limits.x1);
            Assert.IsNaN(limits.x2);
            Assert.AreEqual(limits.y1, -20);
            Assert.AreEqual(limits.y2, 20);
        }

        [Test]
        public void Test_ExpandXY_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.Expand(-10, 20, -30, 40);

            Assert.AreEqual(limits.x1, -10);
            Assert.AreEqual(limits.x2, 20);
            Assert.AreEqual(limits.y1, -30);
            Assert.AreEqual(limits.y2, 40);
        }
    }
}
