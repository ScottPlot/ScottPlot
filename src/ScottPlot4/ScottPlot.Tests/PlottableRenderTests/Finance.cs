using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class Finance
    {
        [Test]
        public void Test_Finance_OhlcVsCandle()
        {
            var plt = new ScottPlot.Plot();

            var ohlcs = new OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new OHLC(273, 275, 264, 265, 1, 1),
                new OHLC(267, 276, 265, 274, 2.5, 2),
                new OHLC(277, 280, 275, 278, 4, 1),
            };

            // start with default settings
            var op = new FinancePlot(ohlcs);
            plt.Add(op);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            op.Candle = true;
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
        public void Test_Finance_ColorUp()
        {
            var plt = new ScottPlot.Plot();

            var ohlcs = new OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new OHLC(273, 275, 264, 265, 1, 1),
                new OHLC(267, 276, 265, 274, 2.5, 2),
                new OHLC(277, 280, 275, 278, 4, 1),
            };

            // start with default settings
            var op = new FinancePlot(ohlcs);
            plt.Add(op);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            op.ColorUp = System.Drawing.Color.Blue;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsMoreBlueThan(before));
        }

        [Test]
        public void Test_Finance_ColorDown()
        {
            var plt = new ScottPlot.Plot();

            var ohlcs = new OHLC[]
            {
                // open, high, low, close, time, timeSpan
                new OHLC(273, 275, 264, 265, 1, 1),
                new OHLC(267, 276, 265, 274, 2.5, 2),
                new OHLC(277, 280, 275, 278, 4, 1),
            };

            // start with default settings
            var op = new FinancePlot(ohlcs);
            plt.Add(op);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            op.ColorDown = System.Drawing.Color.Blue;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(after.IsMoreBlueThan(before));
        }
    }
}
