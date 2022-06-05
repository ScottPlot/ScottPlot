using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlottableRenderTests
{
    internal class Image
    {
        [Test]
        public void Test_Image_Size()
        {
            var plt = new ScottPlot.Plot(400, 300);

            var img = plt.AddImage(ScottPlot.DataGen.SampleImage(), 1, 9);
            img.X = 2;
            img.Y = 9;
            img.WidthInAxisUnits = 1;
            img.HeightInAxisUnits = 4;
            img.Rotation = 15;

            plt.SetAxisLimits(0, 10, 0, 10);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            img.Scale = 2;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            img.WidthInAxisUnits = 2;
            var bmp3 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            //TestTools.SaveFig(bmp3, "3");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            var afterafter = new MeanPixel(bmp3);
            Assert.That(after.IsDarkerThan(before));
            Assert.That(afterafter.IsDarkerThan(after));
        }
    }
}
