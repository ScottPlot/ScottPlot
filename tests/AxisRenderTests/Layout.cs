using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class Layout
    {
        [Test]
        public void Test_Layout_DoesntChangeOnSuccessiveRenders()
        {
            var plt = new ScottPlot.Plot(400, 300);
            var before = new MeanPixel(plt);
            var after = new MeanPixel(plt);
            Assert.That(after.IsEqualTo(before));
        }
    }
}
