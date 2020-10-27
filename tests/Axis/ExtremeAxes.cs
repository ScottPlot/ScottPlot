using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Axis
{
    class ExtremeAxes
    {
        [Test]
        public void Test_Axis_VeryBigNumbers()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            plt.Axis(y1: -10e50, y2: 10e50);
            plt.TightenLayout();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Axis_ModeratelySmallNumbers()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.Title("Moderately Small (e-8)");
            double[] xs = { 1e-8, 2e-8, 3e-8, 4e-8 };
            double[] ys = { 1e-8, 4e-8, 3e-8, 6e-8 };
            plt.PlotScatter(xs, ys);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Axis_ExtremelySmallNumbers()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.Title("Extremely Small (e-20)");
            double[] xs = { 1e-20, 2e-20, 3e-20, 4e-20 };
            double[] ys = { 1e-20, 4e-20, 3e-20, 6e-20 };
            plt.PlotScatter(xs, ys);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Axis_ExtremelySmallSpan()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotLine(0, 1, 1, 1.0000000000000001);
            plt.GetBitmap();
        }
    }
}
