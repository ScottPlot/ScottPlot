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
        public void Test_AxisAutoY_Repeated()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(xs, ys);
            plt.SetAxisLimits(-5, 10, -15, 40);

            for (int i = 0; i < 10; i++)
                plt.AxisAutoY();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisAutoX_Repeated()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(xs, ys);
            plt.SetAxisLimits(-5, 10, -15, 40);

            for (int i = 0; i < 10; i++)
                plt.AxisAutoX();

            TestTools.SaveFig(plt);
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
