using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using ScottPlot;
using ScottPlot.Drawing;
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

            var (xMin, xMax, yMin, yMax) = plt.AxisLimits();
            Assert.Greater(xMax, xMin);
            Assert.Greater(yMax, yMin);
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

            var (xMin, xMax, yMin, yMax) = plt.AxisLimits();
            Assert.Greater(xMax, xMin);
            Assert.Greater(yMax, yMin);
        }

        [Test]
        public void Test_AutoAxis_CandlestickSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotCandlestick(DataGen.RandomStockPrices(rand: null, pointCount: 1));

            var (xMin, xMax, yMin, yMax) = plt.AxisLimits();
            Assert.Greater(xMax, xMin);
            Assert.Greater(yMax, yMin);
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

            var (xMin, xMax, yMin, yMax) = plt.AxisLimits();
            Assert.Greater(xMax, xMin);
            Assert.Greater(yMax, yMin);
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

            var (xMin, xMax, yMin, yMax) = plt.AxisLimits();
            Assert.Greater(xMax, xMin);
            Assert.Greater(yMax, yMin);
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
        public void Test_AutoAxis_ExpandXY()
        {
            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = plt.AxisLimits();

            // large area
            plt.PlotLine(-99, -99, 99, 99);
            plt.AxisAuto();
            var limitsB = plt.AxisLimits();

            Assert.That(limitsB.xMin < limitsA.xMin);
            Assert.That(limitsB.xMax > limitsA.xMax);
            Assert.That(limitsB.yMin < limitsA.yMin);
            Assert.That(limitsB.yMax > limitsA.yMax);
        }

        [Test]
        public void Test_AutoAxis_ShrinkWhenNeeded()
        {
            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = plt.AxisLimits();
            Console.WriteLine($"limits A: {limitsA}");

            // expand to large area
            plt.Axis(-123, 123, -123, 123);
            var limitsB = plt.AxisLimits();
            Console.WriteLine($"limits B: {limitsB}");

            // shrink back to small area
            plt.AxisAuto();
            var limitsC = plt.AxisLimits();
            Console.WriteLine($"limits C: {limitsC}");

            Assert.That(limitsB.xMin < limitsA.xMin);
            Assert.That(limitsB.xMax > limitsA.xMax);
            Assert.That(limitsB.yMin < limitsA.yMin);
            Assert.That(limitsB.yMax > limitsA.yMax);

            Assert.That(limitsB.xMin < limitsC.xMin);
            Assert.That(limitsB.xMax > limitsC.xMax);
            Assert.That(limitsB.yMin < limitsC.yMin);
            Assert.That(limitsB.yMax > limitsC.yMax);
        }
    }
}
