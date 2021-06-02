using NUnit.Framework;
using ScottPlot;
using ScottPlot.Plottable;
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

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new FunctionPlot(func) { };
            plt.SetAxisLimits(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            double? func2(double x) => Math.Pow(x, 2);
            funcPlot.Function = func2;
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
        public void Test_function_LineWidth()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new FunctionPlot(func) { };
            plt.SetAxisLimits(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            funcPlot.LineWidth += 1;
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
        public void Test_function_LineStyle()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new FunctionPlot(func) { };
            plt.SetAxisLimits(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            funcPlot.LineStyle = LineStyle.Dash;
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
        public void Test_function_Color()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            double? func(double x) => Math.Sqrt(x);
            var funcPlot = new FunctionPlot(func) { };
            plt.SetAxisLimits(-1, 1, -.5, 1.5);

            plt.Add(funcPlot);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            funcPlot.Color = Color.Gray;
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
    }
}
