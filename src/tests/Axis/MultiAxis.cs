using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    internal class MultiAxis
    {
        [Test]
        public void Test_RemoveAxis_Works()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var extraAxis = plt.AddAxis(ScottPlot.Renderable.Edge.Left, axisIndex: 2);
            TestTools.SaveFig(plt, "a");

            plt.RemoveAxis(extraAxis);
            TestTools.SaveFig(plt, "b");
        }
    }
}
