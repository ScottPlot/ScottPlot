﻿using NUnit.Framework;
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
            var axSpan = new HSpan() { position1 = 1.23, position2 = 2.34 };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.position2 += 1;
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
            var axSpan = new HSpan() { position1 = 1.23, position2 = 2.34, color = System.Drawing.Color.Gray };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.color = System.Drawing.Color.Black;
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
        public void Test_AxisSpan_Alpha()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axSpan = new HSpan() { position1 = 1.23, position2 = 2.34 };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.alpha /= 2;
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
    }
}
