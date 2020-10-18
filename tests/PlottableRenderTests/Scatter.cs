using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class Scatter
    {
        [Test]
        public void Test_Scatter_ChangeData()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.ys[0] += 1;
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
        public void Test_Scatter_ChangeErrorData()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new PlottableScatter(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.errorX[0] += .1;
            splt.errorY[0] += .1;
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
        public void Test_Scatter_LineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new PlottableScatter(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.lineWidth += 1;
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
        public void Test_Scatter_ErrorLineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new PlottableScatter(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.errorLineWidth += 1;
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
        public void Test_Scatter_ErrorCapSize()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new PlottableScatter(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.errorCapSize += 1;
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
        public void Test_Scatter_MarkerSize()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new PlottableScatter(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.markerSize += 1;
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
        public void Test_Scatter_MarkerShape()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.markerShape = MarkerShape.openCircle;
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
        public void Test_Scatter_Step()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.stepDisplay = true;
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
        public void Test_Scatter_Arrow()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.ArrowheadLength = 5;
            splt.ArrowheadWidth = 5;
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
        public void Test_Scatter_Color()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.color = System.Drawing.Color.Gray;
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
        public void Test_Scatter_LineStyle()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatter(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.lineStyle = LineStyle.Dash;
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
        public void Test_Scatter_Highlight()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new PlottableScatterHighlight(xs, ys) { };

            plt.Add(splt);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            splt.HighlightPointNearest(2.1, 4.1);
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
