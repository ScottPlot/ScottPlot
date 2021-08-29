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
            plt.AddScatter(
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
            plt.AddScatter(
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
            plt.AddCandlesticks(DataGen.RandomStockPrices(rand: null, pointCount: 1));

            var limits = plt.GetAxisLimits();
            Assert.Greater(limits.XSpan, 0);
            Assert.Greater(limits.YSpan, 0);
        }

        [Test]
        public void Test_AutoAxis_ScatterHorizontalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.AddScatter(
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
            plt.AddScatter(
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

            plt.AddPoint(0.1, 0.1);
            plt.AddPoint(-0.1, -0.1);
            plt.AxisAuto();
            plt.Render(); // force a render
            Assert.Greater(plt.GetAxisLimits().XMin, -5);

            plt.AddPoint(999, 999);
            plt.AddPoint(-999, -999);
            plt.AxisAuto();
            plt.Render(); // force a render
            Assert.Less(plt.GetAxisLimits().XMin, -800);

            plt.Clear();
            plt.AddPoint(0.1, 0.1);
            plt.AddPoint(-0.1, -0.1);
            plt.Render(); // force a render
            Assert.Greater(plt.GetAxisLimits().XMin, -5);
        }

        [Test]
        public void Test_AxisLine_FarAwayExpandXY()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();
            var data = DataGen.RandomWalk(rand, 100);

            var sig = plt.AddSignal(data);
            sig.OffsetX = 100;
            sig.OffsetY = 100;
            sig.Label = "scatter";

            plt.AddVerticalLine(-100, label: "vertical");
            plt.AddHorizontalLine(-100, label: "horizontal");
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AutoAxis_ExpandXY()
        {
            var plt = new ScottPlot.Plot();

            // small area
            plt.AddLine(-5, -5, 5, 5);
            plt.AxisAuto();
            var limitsA = plt.GetAxisLimits();

            // large area
            plt.AddLine(-99, -99, 99, 99);
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
            plt.AddLine(-5, -5, 5, 5);
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

        [Test]
        public void Test_MultiAxis_AutoAxis()
        {
            double[] xs = { 1, 2, 3 };
            double[] ys = { 4, 5, 6 };

            var plt = new ScottPlot.Plot(400, 300);
            var sp = plt.AddScatter(xs, ys);
            sp.YAxisIndex = 1;
            sp.XAxisIndex = 1;
            plt.Render();
            var limitsA = plt.GetAxisLimits(1, 1);

            xs[0] = 999;
            ys[0] = 999;
            plt.AxisAuto(0.05, .01, 1, 1);
            plt.Render();
            var limitsB = plt.GetAxisLimits(1, 1);
            Assert.Greater(limitsB.XMax, limitsA.XMax);
            Assert.Greater(limitsB.YMax, limitsA.YMax);
        }
    }
}
