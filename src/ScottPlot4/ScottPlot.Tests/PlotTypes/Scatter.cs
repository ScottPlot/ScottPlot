using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace ScottPlotTests.PlotTypes
{
    class Scatter
    {
        [Test]
        public void Test_Scatter_SinglePoint()
        {
            // https://github.com/ScottPlot/ScottPlot/issues/948
            var plt = new ScottPlot.Plot();
            plt.AddScatter(
                xs: new double[1] { 0 },
                ys: new double[1] { 2 },
                markerSize: 1,
                lineWidth: 1);
            plt.Render();
        }

        [Test]
        public void Test_Scatter_Offset()
        {
            Random rand = new(0);
            double[] xs = DataGen.Random(rand, 20);
            double[] ys = DataGen.Random(rand, 20);

            var plt = new ScottPlot.Plot(400, 300);
            var scatter = plt.AddScatter(xs, ys);
            scatter.XError = DataGen.Random(rand, 20, .1);
            scatter.YError = DataGen.Random(rand, 20, .1);

            plt.AxisAuto();
            var limits1 = plt.GetAxisLimits();
            //TestTools.SaveFig(plt, "beforeOffset");
            Assert.Less(limits1.XMax, 10);
            Assert.Less(limits1.YMax, 20);

            scatter.OffsetX = 10;
            scatter.OffsetY = 20;

            plt.AxisAuto();
            var limits2 = plt.GetAxisLimits();
            //TestTools.SaveFig(plt, "afterOffset");
            Assert.Greater(limits2.XMax, 10);
            Assert.Greater(limits2.YMax, 20);
        }

        [Test]
        public void Test_Scatter_Smooth()
        {
            double[] xs = { 18.5, 20.6, 22.3, 24.5, 26.6, 15, 15 };
            double[] ys = { 1.43, 1.48, 1.6, 1.59, 1.53, 1.52, 1.6 };
            var plt = new ScottPlot.Plot(600, 400);

            var sp1 = plt.AddScatter(xs, ys, label: "DrawLines()");
            sp1.Smooth = false;

            var sp2 = plt.AddScatter(xs, ys, label: "DrawCurve()");
            sp2.Smooth = true;

            plt.Legend();
            plt.SetAxisLimits(0, 30, 1.42, 1.62);
            TestTools.SaveFig(plt);
        }
    }
}
