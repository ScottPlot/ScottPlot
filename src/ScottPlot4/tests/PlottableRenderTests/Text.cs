using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    public class Text
    {
        [Test]
        public void Test_Text_ChangingText()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello", Color = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.Label += "world";
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(before.IsGray());
            Assert.That(after.IsGray());
            Assert.That(after.IsDarkerThan(before));
        }

        [Test]
        public void Test_Text_Rotation()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello" };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);
            string hash1 = ScottPlot.Tools.BitmapHash(bmp1);

            // change the plottable
            txt.Rotation = 45;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);
            string hash2 = ScottPlot.Tools.BitmapHash(bmp2);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            Console.WriteLine($"Before: {hash1}");
            Console.WriteLine($"After: {hash2}");

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Test_Text_Alignment()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello" };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);
            string hash1 = ScottPlot.Tools.BitmapHash(bmp1);

            // change the plottable
            txt.Alignment = Alignment.MiddleCenter;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);
            string hash2 = ScottPlot.Tools.BitmapHash(bmp2);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            Console.WriteLine($"Before: {hash1}");
            Console.WriteLine($"After: {hash2}");

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Test_Text_Frame()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello" };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.BackgroundFill = true;
            txt.BackgroundColor = System.Drawing.Color.Gray;
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
        public void Test_Text_FrameColor()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello", BackgroundFill = true, BackgroundColor = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.BackgroundColor = System.Drawing.Color.Blue;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(before.IsGray());
            Assert.That(after.IsNotGray());
            Assert.That(after.IsMoreBlueThan(before));
        }

        [Test]
        public void Test_Text_FontColor()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello", Color = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.Color = System.Drawing.Color.Blue;
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // measure what changed
            //TestTools.SaveFig(bmp1, "1");
            //TestTools.SaveFig(bmp2, "2");
            var before = new MeanPixel(bmp1);
            var after = new MeanPixel(bmp2);
            Console.WriteLine($"Before: {before}");
            Console.WriteLine($"After: {after}");

            Assert.That(before.IsGray());
            Assert.That(after.IsNotGray());
            Assert.That(after.IsMoreBlueThan(before));
        }

        [Test]
        public void Test_Text_FontSize()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello", FontSize = 12 };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.FontSize = 36;
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
        public void Test_Text_FontBold()
        {
            // bold fonts are supported on all operating systems so only test on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var plt = new ScottPlot.Plot(400, 300);

            // start with default settings
            var txt = new ScottPlot.Plottable.Text() { Label = "hello", FontSize = 12 };
            plt.Add(txt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            txt.FontBold = true;
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
        public void Test_Text_PixelOffset()
        {
            var plt = new ScottPlot.Plot(400, 300);

            plt.AddPoint(30, .5, System.Drawing.Color.Black, 10);

            var txt = plt.AddText("TEST", 30, .5);
            txt.FontSize = 20;
            txt.BackgroundColor = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Black);
            txt.BackgroundFill = true;

            var bmp1 = TestTools.GetLowQualityBitmap(plt);
            txt.PixelOffsetX = 10;
            txt.PixelOffsetY = -10;

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
