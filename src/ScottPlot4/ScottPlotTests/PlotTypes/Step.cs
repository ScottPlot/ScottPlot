using NUnit.Framework;
using ScottPlot;
using System;

namespace ScottPlotTests.PlotTypes
{
    class Step
    {
        [Test]
        public void Test_Step_RandomData()
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(20);
            double[] ys = DataGen.RandomWalk(rand, xs.Length);

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotStep(xs, ys);
            plt.PlotScatter(xs, ys, lineWidth: 0);
            TestTools.SaveFig(plt);
        }
    }
}
