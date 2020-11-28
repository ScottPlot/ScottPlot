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

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
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

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
        }

        [Test]
        public void Test_AutoAxis_CandlestickSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotCandlestick(DataGen.RandomStockPrices(rand: null, pointCount: 1));

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
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

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
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

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
        }

        [Test]
        public void Test_AutoAxis_WorksAfterClear()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.AxisAuto();
            plt.Render(); // force a render
            Assert.Greater(plt.GetAxisLimits().XMin, -5);

            plt.PlotPoint(999, 999);
            plt.PlotPoint(-999, -999);
            plt.AxisAuto();
            plt.Render(); // force a render
            Assert.Less(plt.GetAxisLimits().XMin, -800);

            plt.Clear();
            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.Render(); // force a render
            Assert.Greater(plt.GetAxisLimits().XMin, -5);
        }

        [Test]
        public void Test_AxisLine_FarAwayExpandXY()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();
            var data = DataGen.RandomWalk(rand, 100);
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
            var limitsA = plt.GetAxisLimits();

            // large area
            plt.PlotLine(-99, -99, 99, 99);
            plt.AxisAuto();
            var limitsB = plt.GetAxisLimits();

            Assert.That(limitsB.XMin < limitsA.XMin);
            Assert.That(limitsB.XMax > limitsA.XMax);
            Assert.That(limitsB.YMin < limitsA.YMin);
            Assert.That(limitsB.YMax > limitsA.YMax);
        }

        [Test]
        public void Test_AutoAxis_ShrinkWhenNeeded()
        {
            var plt = new ScottPlot.Plot();

            // small area
            plt.PlotLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = plt.GetAxisLimits();
            Console.WriteLine($"limits A: {limitsA}");

            // expand to large area
            plt.SetAxisLimits(-123, 123, -123, 123);
            var limitsB = plt.GetAxisLimits();
            Console.WriteLine($"limits B: {limitsB}");

            // shrink back to small area
            plt.AxisAuto();
            var limitsC = plt.GetAxisLimits();
            Console.WriteLine($"limits C: {limitsC}");

            Assert.That(limitsB.XMin < limitsA.XMin);
            Assert.That(limitsB.XMax > limitsA.XMax);
            Assert.That(limitsB.YMin < limitsA.YMin);
            Assert.That(limitsB.YMax > limitsA.YMax);

            Assert.That(limitsB.XMin < limitsC.XMin);
            Assert.That(limitsB.XMax > limitsC.XMax);
            Assert.That(limitsB.YMin < limitsC.YMin);
            Assert.That(limitsB.YMax > limitsC.YMax);
        }
    }
}
