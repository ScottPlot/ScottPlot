using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class DataGenTests
    {
        [Test]
        public void Test_Range_MatchesExpectedValues()
        {
            // simple range
            Assert.AreEqual(new double[] { 0, 1, 2 },
                            ScottPlot.DataGen.Range(3));

            // range of values
            Assert.AreEqual(new double[] { 3, 4, 5, 6 },
                            ScottPlot.DataGen.Range(3, 7));

            Assert.AreEqual(new double[] { 7, 6, 5, 4 },
                            ScottPlot.DataGen.Range(7, 3));

            // same values with a defined step size of 1
            Assert.AreEqual(new double[] { 3, 4, 5, 6 },
                            ScottPlot.DataGen.Range(3, 7, 1));

            Assert.AreEqual(new double[] { 7, 6, 5, 4 },
                            ScottPlot.DataGen.Range(7, 3, 1));

            // fractional step size
            double[] expected = new double[] { 0, .1, .2, .3, .4, .5, .6, .7, .8, .9 };
            double[] actual = ScottPlot.DataGen.Range(0, 1, .1);

            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], 1e-10);
        }
    }
}
