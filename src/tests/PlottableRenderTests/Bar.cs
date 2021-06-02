using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
using System;

namespace ScottPlotTests.PlottableRenderTests
{
    class Bar
    {
        [Test]
        public void Test_Bar_ChangingValues()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.Values[0] += 1;
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
        public void Test_Bar_YError()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] yErr = new double[] { 0, 0, 0, 0 };
            var bar = new BarPlot(null, ys, yErr, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.ValueErrors[0] = .25;
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
        public void Test_Bar_BarWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.BarWidth += .1;
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
        public void Test_Bar_FillColor()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { FillColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.FillColor = System.Drawing.Color.Black;
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
        public void Test_Bar_BorderLineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.BorderLineWidth += 1;
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
        public void Test_Bar_BorderLineColor()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { BorderColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.BorderColor = System.Drawing.Color.Black;
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
        public void Test_Bar_ErrorLineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, ys, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.ErrorLineWidth += 1;
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
        public void Test_Bar_ErrorCapSize()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, ys, null) { };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.ErrorCapSize += 1;
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
        public void Test_Bar_ErrorColor()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, ys, null) { ErrorColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.ErrorColor = System.Drawing.Color.Black;
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
        public void Test_Bar_ShowValues()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new BarPlot(null, ys, null, null) { };

            plt.Add(bar);
            plt.SetAxisLimits(-1, 4, -1, 5);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.ShowValuesAboveBars = true;
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
        public void Test_Bar_NegativeColor()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] ys = new double[] { 1, -3, 2, -4 };
            var bar = new BarPlot(null, ys, null, null) { FillColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            bar.FillColorNegative = System.Drawing.Color.Black;
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
