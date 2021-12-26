using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    internal class Arrow
    {

        [Test]
        public void Test_Arrow_PixelOffset()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddPoint(1, 2, size: 20);
            plt.AddPoint(3, 4, size: 20);

            // initial plot
            var arrow = plt.AddArrow(1, 2, 3, 4);
            plt.Margins(.5, .5);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // adjust something
            arrow.PixelOffsetX = 20;
            arrow.PixelOffsetY = -20;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Assert.That(after.IsDifferentThan(before));
        }
    }
}
