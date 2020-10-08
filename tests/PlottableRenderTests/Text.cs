using NUnit.Framework;
using ScottPlot;
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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello", FontColor = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.text += "world";
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello" };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));
            string hash1 = ScottPlot.Tools.BitmapHash(bmp1);

            // change the plottable
            txt.rotation = 45;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));
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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello" };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));
            string hash1 = ScottPlot.Tools.BitmapHash(bmp1);

            // change the plottable
            txt.alignment = TextAlignment.middleCenter;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));
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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello" };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.frame = true;
            txt.frameColor = System.Drawing.Color.Gray;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello", frame = true, frameColor = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.frameColor = System.Drawing.Color.Blue;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello", FontColor = System.Drawing.Color.Gray };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.FontColor = System.Drawing.Color.Blue;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello", FontSize = 12 };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.FontSize = 36;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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

            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var txt = new PlottableText() { text = "hello", FontSize = 12 };
            plt.Add(txt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            txt.FontBold = true;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
