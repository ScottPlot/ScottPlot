using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class ScaleBar
    {
        [Test]
        public void Test_ScaleBar_Default()
        {
            var plt = new ScottPlot.Plot();

            // start plot without scalebar
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // add a scalebar
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
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
        public void Test_ScaleBar_LineWidth()
        {
            var plt = new ScottPlot.Plot();

            // create plot with generic scalebar settings
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // customize the scalebar
            sb.LineWidth += 1;
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
        public void Test_ScaleBar_FontColor()
        {
            var plt = new ScottPlot.Plot();

            // create plot with generic scalebar settings
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // customize the scalebar
            sb.FontColor = System.Drawing.Color.Blue;
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
        public void Test_ScaleBar_LineColor()
        {
            var plt = new ScottPlot.Plot();

            // create plot with generic scalebar settings
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // customize the scalebar
            sb.LineColor = System.Drawing.Color.Blue;
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
        public void Test_ScaleBar_FontSize()
        {
            var plt = new ScottPlot.Plot();

            // create plot with generic scalebar settings
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // customize the scalebar
            sb.Font.Size *= 2;
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
        public void Test_ScaleBar_FontBold()
        {
            // bold fonts are supported on all operating systems so only test on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            var plt = new ScottPlot.Plot();

            // create plot with generic scalebar settings
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            var sb = new ScottPlot.Plottable.ScaleBar()
            {
                Width = 5,
                Height = .25,
                VerticalLabel = "5 V",
                HorizontalLabel = "250 ms"
            };
            plt.Add(sb);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // customize the scalebar
            sb.FontBold = true;
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
