using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.DataGenTests
{
    public class RenderTests
    {
        [Test]
        public void Test_DataGen_SinSweep()
        {
            int pointCount = 5_000;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys = ScottPlot.DataGen.SinSweep(pointCount, density: 20);
            var plt = new ScottPlot.Plot(600, 300);
            plt.AddScatterLines(xs, ys);
            plt.AxisAutoX(margin: 0);
            TestTools.SaveFig(plt);
        }
    }
}
