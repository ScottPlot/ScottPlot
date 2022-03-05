using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableUnitTests
{
    class AxisLineAndSpanTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_AxisLine_AutoAxisRespected(bool ignore)
        {
            // plot with small data in the center
            var plt = new ScottPlot.Plot();
            plt.AddPoint(-10, -10);
            plt.AddPoint(10, 10);
            plt.AxisAuto();
            var limits1 = plt.GetAxisLimits();

            // large data
            var line1 = plt.AddVerticalLine(999);
            var line2 = plt.AddHorizontalLine(999);
            line1.IgnoreAxisAuto = ignore;
            line2.IgnoreAxisAuto = ignore;
            plt.AxisAuto();
            var limits2 = plt.GetAxisLimits();

            if (ignore)
                Assert.AreEqual(limits1, limits2);
            else
                Assert.AreNotEqual(limits1, limits2);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_AxisSpan_AutoAxisRespected(bool ignore)
        {
            // plot with small data in the center
            var plt = new ScottPlot.Plot();
            plt.AddPoint(-10, -10);
            plt.AddPoint(10, 10);
            plt.AxisAuto();
            var limits1 = plt.GetAxisLimits();

            // large data
            var span1 = plt.AddVerticalSpan(-999, 999);
            var span2 = plt.AddHorizontalSpan(-999, 999);
            span1.IgnoreAxisAuto = ignore;
            span2.IgnoreAxisAuto = ignore;
            plt.AxisAuto();
            var limits2 = plt.GetAxisLimits();

            if (ignore)
                Assert.AreEqual(limits1, limits2);
            else
                Assert.AreNotEqual(limits1, limits2);
        }
    }
}
