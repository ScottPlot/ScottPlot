using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
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

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new ScatterPlot(xs, ys) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.Ys[0] += 1;
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

        [Test]
        public void Test_Scatter_ChangeErrorData()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.XError[0] += .1;
            splt.YError[0] += .1;
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
        public void Test_Scatter_ChangeOnlyYErrorData()
        {
            var plt = new ScottPlot.Plot();

            // set errorY but NOT errorX
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, errorY: yErr) { };

            plt.Add(splt);
            var bmp = TestTools.GetLowQualityBitmap(plt);
            Console.WriteLine(new MeanPixel(bmp));

            Assert.That(bmp != null);
        }

        [Test]
        public void Test_Scatter_LineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.LineWidth += 1;
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
        public void Test_Scatter_ErrorLineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.ErrorLineWidth += 1;
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
        public void Test_Scatter_ErrorCapSize()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.ErrorCapSize += 1;
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
        public void Test_Scatter_MarkerSize()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            double[] xErr = { .15, .15, .5, .5 };
            double[] yErr = { .5, .5, 1, 1 };
            var splt = new ScatterPlot(xs, ys, xErr, yErr) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.MarkerSize += 1;
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
        public void Test_Scatter_MarkerShape()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new ScatterPlot(xs, ys)
            {
                MarkerSize = 20,
                MarkerShape = MarkerShape.filledCircle
            };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.MarkerShape = MarkerShape.openCircle;
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

        [Test]
        public void Test_Scatter_Step()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new ScatterPlot(xs, ys) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.StepDisplay = true;
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
        public void Test_Scatter_Color()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new ScatterPlot(xs, ys) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.Color = System.Drawing.Color.Gray;
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

        [Test]
        public void Test_Scatter_LineStyle()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };
            var splt = new ScatterPlot(xs, ys) { };

            plt.Add(splt);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            splt.LineStyle = LineStyle.Dash;
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

        [Test]
        public void Test_Scatter_ZoomWorksWithTwoPoints()
        {
            // Tests a bug where plots with a single point (axis span 0) can't zoom
            // https://github.com/ScottPlot/ScottPlot/issues/768

            double[] dataX = { 42, 3 };
            double[] dataY = { 303, 5 };

            // create a scatter plot with a single point
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(dataX, dataY);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // zoom in
            plt.AxisZoom(2, 2);
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // ensure the bitmap changed
            Assert.AreNotEqual(ScottPlot.Tools.BitmapHash(bmp1), ScottPlot.Tools.BitmapHash(bmp2));
        }

        [Test]
        public void Test_Scatter_ZoomWorksWithOnePoint()
        {
            // Tests a bug where plots with a single point (axis span 0) can't zoom
            // https://github.com/ScottPlot/ScottPlot/issues/768

            double[] dataX = { 42 };
            double[] dataY = { 303 };

            // create a scatter plot with a single point
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(dataX, dataY);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // zoom in
            plt.AxisZoom(2, 2);
            var bmp2 = TestTools.GetLowQualityBitmap(plt);

            // ensure the bitmap changed
            Assert.AreNotEqual(ScottPlot.Tools.BitmapHash(bmp1), ScottPlot.Tools.BitmapHash(bmp2));
        }
    }
}
