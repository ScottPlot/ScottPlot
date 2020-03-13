using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Statistics;

namespace ScottPlotTests
{
    [TestFixture]
    public class RegressionTests
    {
        // Issue #290 Test
        [Test]
        public void GetCoefficients_FirstXNotZero_ResultEqualtoTI_84_Plus()
        {
            double[] x = new double[] { 1, 2, 3, 4, 5, 6, 7 };
            double[] y = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };
            var reg = new LinearRegressionLine(x, y);
            Assert.AreEqual(0.4, reg.slope, 0.0001);
            Assert.AreEqual(1.5428, reg.offset, 0.0001);
        }
    }
}
