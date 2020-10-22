using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class ErrorBar
    {
        [Test]
        public void Test_ErrorBar_ChangingValues()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = new double[] { 1, 3, 2, 4 };
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] xErrNeg = new double[] { .2, .2, .2, .2 };
            double[] xErrPos = new double[] { .2, .2, .2, .2 };
            double[] yErrNeg = new double[] { 1, 1, 1, 1 };
            double[] yErrPos = new double[] { 1, 1, 1, 1 };
            var err = new PlottableErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg) { };

            plt.Add(err);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            yErrPos[0] += 1;
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
        public void Test_ErrorBar_CapSize()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = new double[] { 1, 3, 2, 4 };
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] xErrNeg = new double[] { .2, .2, .2, .2 };
            double[] xErrPos = new double[] { .2, .2, .2, .2 };
            double[] yErrNeg = new double[] { 1, 1, 1, 1 };
            double[] yErrPos = new double[] { 1, 1, 1, 1 };
            var err = new PlottableErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg) { };

            plt.Add(err);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            err.CapSize += 1;
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
        public void Test_ErrorBar_LineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = new double[] { 1, 3, 2, 4 };
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] xErrNeg = new double[] { .2, .2, .2, .2 };
            double[] xErrPos = new double[] { .2, .2, .2, .2 };
            double[] yErrNeg = new double[] { 1, 1, 1, 1 };
            double[] yErrPos = new double[] { 1, 1, 1, 1 };
            var err = new PlottableErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg) { };

            plt.Add(err);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            err.LineWidth += 1;
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
        public void Test_ErrorBar_Color()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = new double[] { 1, 3, 2, 4 };
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] xErrNeg = new double[] { .2, .2, .2, .2 };
            double[] xErrPos = new double[] { .2, .2, .2, .2 };
            double[] yErrNeg = new double[] { 1, 1, 1, 1 };
            double[] yErrPos = new double[] { 1, 1, 1, 1 };
            var err = new PlottableErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg) { };

            plt.Add(err);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            err.Color = System.Drawing.Color.Gray;
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
        public void Test_ErrorBar_LineStyle()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = new double[] { 1, 3, 2, 4 };
            double[] ys = new double[] { 1, 3, 2, 4 };
            double[] xErrNeg = new double[] { .2, .2, .2, .2 };
            double[] xErrPos = new double[] { .2, .2, .2, .2 };
            double[] yErrNeg = new double[] { 1, 1, 1, 1 };
            double[] yErrPos = new double[] { 1, 1, 1, 1 };
            var err = new PlottableErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg) { };

            plt.Add(err);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            err.LineStyle = LineStyle.Dot;
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
    }
}
