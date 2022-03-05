using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Axis
{
    class AutoAxis
    {
        [Test]
        public void Test_AxisAuto_Works()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddPoint(-5, -5);
            plt.AddPoint(5, 5);

            // set limits too small
            plt.SetAxisLimits(-1, 1, -1, 1);
            var limits1 = plt.GetAxisLimits();

            // autoAxis should make them bigger
            plt.AxisAuto();
            var limits2 = plt.GetAxisLimits();

            Assert.Less(limits2.XMin, limits1.XMin);
            Assert.Greater(limits2.XMax, limits1.XMax);
            Assert.Less(limits2.YMin, limits1.YMin);
            Assert.Greater(limits2.YMax, limits1.YMax);
        }

        [Test]
        public void Test_AxisAutoY_Works()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddPoint(-5, -5);
            plt.AddPoint(5, 5);

            // set limits too small
            plt.SetAxisLimits(-1, 1, -1, 1);
            var limits1 = plt.GetAxisLimits();

            // autoAxis should make them bigger just for Y values
            plt.AxisAutoY();
            var limits2 = plt.GetAxisLimits();

            Assert.AreEqual(limits1.XMin, limits2.XMin);
            Assert.AreEqual(limits1.XMax, limits2.XMax);

            Assert.Less(limits2.YMin, limits1.YMin);
            Assert.Greater(limits2.YMax, limits1.YMax);
        }

        [Test]
        public void Test_AxisAutoX_Works()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddPoint(-5, -5);
            plt.AddPoint(5, 5);

            // set limits too small
            plt.SetAxisLimits(-1, 1, -1, 1);
            var limits1 = plt.GetAxisLimits();

            // autoAxis should make them bigger just for X values
            plt.AxisAutoX();
            var limits2 = plt.GetAxisLimits();

            Assert.AreEqual(limits1.YMin, limits2.YMin);
            Assert.AreEqual(limits1.YMax, limits2.YMax);

            Assert.Less(limits2.XMin, limits1.XMin);
            Assert.Greater(limits2.XMax, limits1.XMax);
        }

        [Test]
        public void Test_AxisAuto_SignalWithMinMaxIndexSet()
        {
            var plt = new ScottPlot.Plot(400, 300);
            var sig = plt.AddSignal(ScottPlot.DataGen.Sin(1000), sampleRate: 10);
            sig.MinRenderIndex = 450;
            sig.MaxRenderIndex = 550;
            plt.AxisAuto();

            var limits = plt.GetAxisLimits();
            Console.WriteLine($"AutoAxis Limits: {limits}");

            Assert.Less(limits.XMin, limits.XMax);
            Assert.Less(limits.YMin, limits.YMax);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisAuto_AdjustsAllAxes()
        {
            var plt = new ScottPlot.Plot(400, 300);

            var sig1 = plt.AddSignal(ScottPlot.DataGen.Sin(51));
            sig1.XAxisIndex = 0;
            sig1.YAxisIndex = 0;

            var sig2 = plt.AddSignal(ScottPlot.DataGen.Cos(51));
            sig2.XAxisIndex = 1;
            sig2.YAxisIndex = 1;

            // on startup all axes are reset with AxisAuto()
            plt.Render();
            plt.AxisAuto();
            var originalLimitsPrimary = plt.GetAxisLimits(0);
            var originalLimitsSecondary = plt.GetAxisLimits(1);

            // zoom out on all axes
            plt.AxisZoom(.1, .1, xAxisIndex: 0, yAxisIndex: 0);
            plt.AxisZoom(.1, .1, xAxisIndex: 1, yAxisIndex: 1);

            var zoomedOutLimitsPrimary = plt.GetAxisLimits(0);
            Assert.Greater(zoomedOutLimitsPrimary.XSpan, originalLimitsPrimary.XSpan);
            Assert.Greater(zoomedOutLimitsPrimary.YSpan, originalLimitsPrimary.YSpan);

            var zoomedOutLimitsSecondary = plt.GetAxisLimits(1);
            Assert.Greater(zoomedOutLimitsSecondary.XSpan, originalLimitsSecondary.XSpan);
            Assert.Greater(zoomedOutLimitsSecondary.YSpan, originalLimitsSecondary.YSpan);

            // call AxisAuto() which is now expected to act on all axes
            plt.AxisAuto();

            var resetLimitsPrimary = plt.GetAxisLimits(0);
            var resetLimitsSecondary = plt.GetAxisLimits(1);
            Assert.AreEqual(resetLimitsPrimary.XSpan, originalLimitsPrimary.XSpan);
            Assert.AreEqual(resetLimitsPrimary.YSpan, originalLimitsPrimary.YSpan);
            Assert.AreEqual(resetLimitsSecondary.XSpan, originalLimitsSecondary.XSpan);
            Assert.AreEqual(resetLimitsSecondary.YSpan, originalLimitsSecondary.YSpan);

            TestTools.SaveFig(plt);

            //var limits = plt.GetAxisLimits();
            //Console.WriteLine(limits);
        }

        [Test]
        public void Test_GetDataLimits()
        {
            var plt = new ScottPlot.Plot();

            Random rand = new(0);
            double[] xs = ScottPlot.DataGen.Random(rand, 100);
            double[] ys = ScottPlot.DataGen.Random(rand, 100);
            plt.AddScatter(xs, ys);
            plt.Render();

            // default axis limits contain padding and should be larger than data
            var axisLimitsDefault = plt.GetAxisLimits();
            Console.WriteLine(axisLimitsDefault);
            Assert.Less(axisLimitsDefault.XMin, xs.Min());
            Assert.Greater(axisLimitsDefault.XMax, xs.Max());
            Assert.Less(axisLimitsDefault.YMin, ys.Min());
            Assert.Greater(axisLimitsDefault.YMax, ys.Max());

            // axis limits after tight margins should make axis limts equal data limits
            plt.Margins(0, 0);
            var axisLimitsTight = plt.GetAxisLimits();
            Console.WriteLine(axisLimitsTight);
            Assert.AreEqual(axisLimitsTight.XMin, xs.Min(), 1e-8);
            Assert.AreEqual(axisLimitsTight.XMax, xs.Max(), 1e-8);
            Assert.AreEqual(axisLimitsTight.YMin, ys.Min(), 1e-8);
            Assert.AreEqual(axisLimitsTight.YMax, ys.Max(), 1e-8);

            // data limits should be the same numbers without modifying the axes
            var dataLimits = plt.GetDataLimits();
            Console.WriteLine(dataLimits);
            Assert.AreEqual(dataLimits.XMin, xs.Min());
            Assert.AreEqual(dataLimits.XMax, xs.Max());
            Assert.AreEqual(dataLimits.YMin, ys.Min());
            Assert.AreEqual(dataLimits.YMax, ys.Max());
        }
    }
}
