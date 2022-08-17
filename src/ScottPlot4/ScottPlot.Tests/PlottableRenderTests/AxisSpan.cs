using NUnit.Framework;
using ScottPlot.Plottable;
using System;

namespace ScottPlotTests.PlottableRenderTests
{
    class AxisSpan
    {
        [Test]
        public void Test_AxisSpan_ChangesPosition()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axSpan = new HSpan() { X1 = 1.23, X2 = 2.34 };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.X2 += 1;
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
        public void Test_AxisSpan_Color()
        {
            var plt = new ScottPlot.Plot();

            // start with default settings
            var axSpan = new HSpan() { X1 = 1.23, X2 = 2.34, Color = System.Drawing.Color.Gray };

            plt.Add(axSpan);
            var bmp1 = TestTools.GetLowQualityBitmap(plt);

            // change the plottable
            axSpan.Color = System.Drawing.Color.Black;
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
        public void AxisHSpan_ExtremeZoomIn_FullScreenIsSpanColor()
        {
            var plt = new ScottPlot.Plot();
            var axSpan = new HSpan() { X1 = 1, X2 = 10, Color = System.Drawing.Color.Green };
            plt.Add(axSpan);

            // Initial zoom to fill full plot with span color
            plt.AxisZoom(10);

            var smallZoomBmp = TestTools.GetLowQualityBitmap(plt);
            var smallZoom = new MeanPixel(smallZoomBmp);

            // Extreme zoom to prove that full plot filled with span Color
            plt.AxisZoom(10_000_000);

            var extremeZoomBmp = TestTools.GetLowQualityBitmap(plt);
            var extremeZoom = new MeanPixel(extremeZoomBmp);

            // Compare mean pixel with delta, because different ticks
            Assert.AreEqual(smallZoom.RGB, extremeZoom.RGB, 1.0);
        }

        [Test]
        public void AxisVSpan_ExtremeZoomIn_FullScreenIsSpanColor()
        {
            var plt = new ScottPlot.Plot();
            var axSpan = new VSpan() { Y1 = 1, Y2 = 10, Color = System.Drawing.Color.Green };
            plt.Add(axSpan);

            // Initial zoom to fill full plot with span color
            plt.AxisZoom(1, 10);

            var smallZoomBmp = TestTools.GetLowQualityBitmap(plt);
            var smallZoom = new MeanPixel(smallZoomBmp);

            // Extreme zoom to prove that full plot filled with span Color
            plt.AxisZoom(1, 10_000_000);

            var extremeZoomBmp = TestTools.GetLowQualityBitmap(plt);
            var extremeZoom = new MeanPixel(extremeZoomBmp);

            // Compare mean pixel with delta, because different ticks
            // Y Ticks has more affect on mean pixel
            Assert.AreEqual(smallZoom.RGB, extremeZoom.RGB, 20);
        }

        [Test]
        public void AxisHSpan_Frameless()
        {
            var plt = new ScottPlot.Plot(200, 100);
            plt.Style(dataBackground: System.Drawing.Color.Blue);
            plt.Style(figureBackground: System.Drawing.Color.Green);
            plt.AddHorizontalSpan(10, 20, System.Drawing.Color.Magenta);

            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);

            plt.XAxis.Line(false);
            plt.XAxis2.Line(false);
            plt.YAxis.Line(false);
            plt.YAxis2.Line(false);
            TestTools.SaveFig(plt, "noline");

            plt.Frameless();
            TestTools.SaveFig(plt, "frameless");
        }
    }
}
