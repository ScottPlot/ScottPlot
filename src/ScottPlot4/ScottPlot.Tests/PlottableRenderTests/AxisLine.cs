using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Plottable;

namespace ScottPlotTests.PlottableRenderTests
{
    class AxisLine
    {
        [Test]
        public void Test_AxisLine_ChangesPosition()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axLine = new HLine() { Y = 1.23 };

            plt.Add(axLine);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axLine.Y += 1;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            Assert.AreNotEqual(ScottPlot.Tools.BitmapHash(bmp1), ScottPlot.Tools.BitmapHash(bmp2));
        }

        [Test]
        public void Test_AxisLine_LineStyle()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axLine = new HLine() { Y = 1.23 };

            plt.Add(axLine);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axLine.LineStyle = LineStyle.Dash;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsLighterThan(before));
        }

        [Test]
        public void Test_AxisLine_LineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axLine = new HLine() { Y = 1.23 };

            plt.Add(axLine);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axLine.LineWidth += 1;
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
        public void Test_AxisLine_Color()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axLine = new HLine() { Y = 1.23, Color = System.Drawing.Color.Gray };

            plt.Add(axLine);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axLine.Color = System.Drawing.Color.Black;
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
