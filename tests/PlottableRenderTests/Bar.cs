using NUnit.Framework;
using ScottPlot;
using System;

namespace ScottPlotTests.PlottableRenderTests
{
    class Bar
    {
        [Test]
        public void Test_Bar_ChangingValues()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.ys[0] += 1;
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
        public void Test_Bar_YError()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] yErr = new double[] { 0, 0, 0, 0 };
            var bar = new PlottableBar(null, ys, yErr, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.yErr[0] = .25;
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
        public void Test_Bar_BarWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.barWidth += .1;
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
        public void Test_Bar_Fill()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.fill = false;
            var bmp2 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

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
        public void Test_Bar_FillColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { fillColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.fillColor = System.Drawing.Color.Black;
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
        public void Test_Bar_BorderLineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.borderLineWidth += 1;
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
        public void Test_Bar_BorderLineColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { borderColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.borderColor = System.Drawing.Color.Black;
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
        public void Test_Bar_ErrorLineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, ys, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.errorLineWidth += 1;
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
        public void Test_Bar_ErrorCapSize()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, ys, null) { };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.errorCapSize += 1;
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
        public void Test_Bar_ErrorColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, ys, null) { errorColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.errorColor = System.Drawing.Color.Black;
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
        public void Test_Bar_ShowValues()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, 3, 2, 4 };
            var bar = new PlottableBar(null, ys, null, null) { };

            plt.Add(bar);
            plt.Axis(-1, 4, -1, 5);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.showValues = true;
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
        public void Test_Bar_NegativeColor()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] ys = new double[] { 1, -3, 2, -4 };
            var bar = new PlottableBar(null, ys, null, null) { fillColor = System.Drawing.Color.Gray };

            plt.Add(bar);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            bar.negativeColor = System.Drawing.Color.Black;
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