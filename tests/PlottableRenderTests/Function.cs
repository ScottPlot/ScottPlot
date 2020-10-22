using NUnit.Framework;
using ScottPlot;
using System;
using System.Drawing;

namespace ScottPlotTests.PlottableRenderTests
{
    class Function
    {
        [Test]
        public void Test_function_ChangingData()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new PlottableFunction(func) { };
            plt.Axis(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            double? func2(double x) => Math.Pow(x, 2);
            funcPlot.function = func2;
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
        public void Test_function_LineWidth()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new PlottableFunction(func) { };
            plt.Axis(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            funcPlot.lineWidth += 1;
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
        public void Test_function_LineStyle()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new PlottableFunction(func) { };
            plt.Axis(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            funcPlot.lineStyle = LineStyle.Dash;
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
        public void Test_function_Color()
        {
            var plt = new ScottPlot.Plot();
            plt.AntiAlias(false, false, false);

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new PlottableFunction(func) { };
            plt.Axis(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = new System.Drawing.Bitmap(plt.GetBitmap(renderFirst: true));

            // change the plottable
            funcPlot.color = Color.Gray;
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
