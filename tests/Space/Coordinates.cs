using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Space
{
    class Coordinates
    {
        [Test]
        public void Test_Space_Coordinates()
        {
            var info = new ScottPlot.Space.FigureInfo();
            info.Resize(600, 400, 380, 310, 150, 50);
            info.SetLimits(-1, 1, -10, 10);

            // test a point 10% (38 pixels) from left edge of data area
            Assert.AreEqual(150 + 38, info.GetPixelX(-.8));

            // test a point 10% (31 pixels) from top edge of data area
            Assert.AreEqual(50 + 31, info.GetPixelY(8));
        }

        [Test]
        public void Test_Space_SecondY()
        {
            var info = new ScottPlot.Space.FigureInfo();
            info.Resize(600, 400, 380, 310, 150, 50);

            // set a different scale for the primary Y
            info.SetLimits(-123, 123, -123, 123);

            // set the standard scale for the secondary Y
            info.SetLimits(-1, 1, -10, 10, planeIndex: 1);

            // ensure the old Y1 values no longer match
            Assert.AreNotEqual(150 + 38, info.GetPixelX(-.8));
            Assert.AreNotEqual(50 + 31, info.GetPixelY(8));

            // ensure the old Y1 values match the new Y2 values
            Assert.AreEqual(150 + 38, info.GetPixelX(-.8, planeIndex: 1));
            Assert.AreEqual(50 + 31, info.GetPixelY(8, planeIndex: 1));
        }
    }
}
