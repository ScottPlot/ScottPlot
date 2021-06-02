using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
    }
}
