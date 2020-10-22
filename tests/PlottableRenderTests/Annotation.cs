using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class Annotation
    {
        [Test]
        public void Test_Annotation_ChangingText()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var pa = new PlottableAnnotation() { label = "Hello", xPixel = 10, yPixel = 10, FontSize = 36 };
            plt.Add(pa);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            pa.label += "World";
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
        public void Test_Annotation_BackgroundColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var pa = new PlottableAnnotation() { label = "Hello", xPixel = 10, yPixel = 10 };
            plt.Add(pa);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            pa.BackgroundColor = System.Drawing.Color.Gray;
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
        public void Test_Annotation_ShadowColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var pa = new PlottableAnnotation() { label = "Hello", xPixel = 10, yPixel = 10 };
            plt.Add(pa);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            pa.ShadowColor = System.Drawing.Color.Black;
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
        public void Test_Annotation_BorderColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var pa = new PlottableAnnotation() { label = "Hello", xPixel = 10, yPixel = 10, BorderColor = System.Drawing.Color.Gray };
            plt.Add(pa);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            pa.BorderColor = System.Drawing.Color.Black;
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
        public void Test_Annotation_BorderWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            var pa = new PlottableAnnotation() { label = "Hello", xPixel = 10, yPixel = 10 };
            plt.Add(pa);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            pa.BorderWidth += 1;
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
