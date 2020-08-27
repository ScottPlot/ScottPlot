using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    public class AutoAxis
    {
        [Test]
        public void Test_AutoAxis_ScatterDiagonalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AutoAxis_ScatterSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_CandlestickSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotCandlestick(ScottPlot.DataGen.RandomStockPrices(rand: null, pointCount: 1));
            plt.GetBitmap(); // force a render

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_ScatterHorizontalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 1 }
                );
            plt.AxisAuto();

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_ScatterVerticalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 1 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_WorksAfterClear()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.AxisAuto();
            plt.GetBitmap(); // force a render
            Assert.Greater(plt.Axis()[0], -5);

            plt.PlotPoint(999, 999);
            plt.PlotPoint(-999, -999);
            plt.AxisAuto();
            plt.GetBitmap(); // force a render
            Assert.Less(plt.Axis()[0], -800);

            plt.Clear();
            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.GetBitmap(); // force a render
            Assert.Greater(plt.Axis()[0], -5);
        }

        [Test]
        public void Test_AxisLine_FarAwayExpandXY()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();
            var data = ScottPlot.DataGen.RandomWalk(rand, 100);
            plt.PlotSignal(data, xOffset: 100, yOffset: 100, label: "scatter");
            plt.PlotVLine(-100, label: "vertical");
            plt.PlotHLine(-100, label: "horizontal");
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisLine_FarAwayOnlyExpandX()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();
            var data = ScottPlot.DataGen.RandomWalk(rand, 100);
            plt.PlotSignal(data, xOffset: 100, yOffset: 100, label: "scatter");
            plt.PlotVLine(-100, label: "vertical");
            plt.PlotHLine(-100, label: "horizontal");
            plt.Legend();

            plt.AxisAuto(xExpandOnly: true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisLine_FarAwayOnlyExpandY()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();
            var data = ScottPlot.DataGen.RandomWalk(rand, 100);
            plt.PlotSignal(data, xOffset: 100, yOffset: 100, label: "scatter");
            plt.PlotVLine(-100, label: "vertical");
            plt.PlotHLine(-100, label: "horizontal");
            plt.Legend();

            plt.AxisAuto(yExpandOnly: true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AutoAxis_ExpandXY()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = new ScottPlot.Config.AxisLimits2D(plt.Axis());

            // large area
            plt.PlotLine(-99, -99, 99, 99);
            plt.AxisAuto();
            var limitsB = new ScottPlot.Config.AxisLimits2D(plt.Axis());

            Assert.That(limitsA.xSpan < limitsB.xSpan);
            Assert.That(limitsA.ySpan < limitsB.ySpan);
        }

        [Test]
        public void Test_AutoAxis_ExpandYonly()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = new ScottPlot.Config.AxisLimits2D(plt.Axis());

            // large area
            plt.PlotLine(-99, -99, 99, 99);
            plt.AxisAuto(yExpandOnly: true);
            var limitsB = new ScottPlot.Config.AxisLimits2D(plt.Axis());

            Assert.That(limitsA.xSpan == limitsB.xSpan);
            Assert.That(limitsA.ySpan < limitsB.ySpan);
        }

        [Test]
        public void Test_AutoAxis_ExpandXonly()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = new ScottPlot.Config.AxisLimits2D(plt.Axis());
            Console.WriteLine($"limits A: {limitsA}");

            // large area
            plt.PlotLine(-99, -99, 99, 99);
            plt.AxisAuto(xExpandOnly: true);
            var limitsB = new ScottPlot.Config.AxisLimits2D(plt.Axis());
            Console.WriteLine($"limits B: {limitsB}");

            Assert.That(limitsA.xSpan < limitsB.xSpan);
            Assert.That(limitsA.ySpan == limitsB.ySpan);
        }

        [Test]
        public void Test_AutoAxis_ShrinkWhenNeeded()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = new ScottPlot.Config.AxisLimits2D(plt.Axis());
            Console.WriteLine($"limits A: {limitsA}");

            // expand to large area
            plt.Axis(-123, 123, -123, 123);
            var limitsB = new ScottPlot.Config.AxisLimits2D(plt.Axis());
            Console.WriteLine($"limits B: {limitsB}");

            // shrink back to small area
            plt.AxisAuto();
            var limitsC = new ScottPlot.Config.AxisLimits2D(plt.Axis());
            Console.WriteLine($"limits C: {limitsC}");

            Assert.That(limitsB.xSpan > limitsC.xSpan);
            Assert.That(limitsB.ySpan > limitsC.ySpan);

            Assert.That(limitsA.xSpan == limitsC.xSpan);
            Assert.That(limitsA.ySpan == limitsC.ySpan);
        }
    }
}
