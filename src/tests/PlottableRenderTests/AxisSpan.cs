using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class AxisSpan
    {
        [Test]
        public void Test_AxisSpan_ChangesPosition()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axSpan = new HSpan() { X1 = 1.23, X2 = 2.34 };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.X2 += 1;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsDarkerThan(before));
        }

        [Test]
        public void Test_AxisSpan_Color()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axSpan = new HSpan() { X1 = 1.23, X2 = 2.34, Color = System.Drawing.Color.Gray };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.Color = System.Drawing.Color.Black;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsDarkerThan(before));
        }
    }
}
