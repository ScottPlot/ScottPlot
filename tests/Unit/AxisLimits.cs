using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Unit
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
        public void Test_ExpandX_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.ExpandX(-20, 20);

            Assert.AreEqual(limits.x1, -20);
            Assert.AreEqual(limits.x2, 20);
            Assert.IsNaN(limits.y1);
            Assert.IsNaN(limits.y2);
        }

        [Test]
        public void Test_ExpandY_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.ExpandY(-20, 20);

            Assert.IsNaN(limits.x1);
            Assert.IsNaN(limits.x2);
            Assert.AreEqual(limits.y1, -20);
            Assert.AreEqual(limits.y2, 20);
        }

        [Test]
        public void Test_ExpandXY_ExpectedOutput()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();

            limits.ExpandXY(-10, 20, -30, 40);

            Assert.AreEqual(limits.x1, -10);
            Assert.AreEqual(limits.x2, 20);
            Assert.AreEqual(limits.y1, -30);
            Assert.AreEqual(limits.y2, 40);
        }

        [Test]
        public void Test_MakeRational_WhenAllAreNaN()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();
            limits.MakeRational();

            Assert.That(limits.x1 < 0);
            Assert.That(limits.x1 > -10);

            Assert.That(limits.x2 > 0);
            Assert.That(limits.x2 < 10);

            Assert.That(limits.y1 < 0);
            Assert.That(limits.y1 > -10);

            Assert.That(limits.y2 > 0);
            Assert.That(limits.y2 < 10);
        }

        [Test]
        public void Test_MakeRational_WhenXsContainNaN()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();
            limits.ExpandY(-30, 40);
            limits.MakeRational();

            Console.WriteLine(limits);

            Assert.That(limits.x1 < 0);
            Assert.That(limits.x1 > -10);

            Assert.That(limits.x2 > 0);
            Assert.That(limits.x2 < 10);

            Assert.That(limits.y1 == -30);
            Assert.That(limits.y2 == 40);
        }

        [Test]
        public void Test_MakeRational_WhenYsContainNaN()
        {
            var limits = new ScottPlot.Config.AxisLimits2D();
            limits.ExpandX(-10, 20);
            limits.MakeRational();

            Console.WriteLine(limits);

            Assert.That(limits.x1 == -10);
            Assert.That(limits.x2 == 20);

            Assert.That(limits.y1 < 0);
            Assert.That(limits.y1 > -10);

            Assert.That(limits.y2 > 0);
            Assert.That(limits.y2 < 10);
        }
    }
}
