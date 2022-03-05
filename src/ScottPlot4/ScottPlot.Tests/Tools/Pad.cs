using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Tools
{
    class Pad
    {
        [Test]
        public void Test_Pad_NoArguments()
        {
            double[] values = { 1, 2, 3 };
            double[] result = ScottPlot.Tools.Pad(values);

            Assert.AreEqual(new double[] { 0, 1, 2, 3, 0 }, result);
        }

        [Test]
        public void Test_Pad_MultipleDefined()
        {
            double[] values = { 1, 2, 3 };
            double[] result = ScottPlot.Tools.Pad(values, 3, -1, -2);

            Assert.AreEqual(new double[] { -1, -1, -1, 1, 2, 3, -2, -2, -2 }, result);
        }

        [Test]
        public void Test_Pad_CloneEdges()
        {
            double[] values = { 1, 2, 3 };
            double[] result = ScottPlot.Tools.Pad(values, 3, cloneEdges: true);

            Assert.AreEqual(new double[] { 1, 1, 1, 1, 2, 3, 3, 3, 3 }, result);
        }
    }
}
