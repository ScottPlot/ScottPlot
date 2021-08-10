using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    class ViewLimits
    {
        [Test]
        public void Test_View_Limits()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            plt.SetViewLimits(yMin: -1);
            TestTools.SaveFig(plt, "1");
            var limits1 = plt.GetAxisLimits();

            plt.AxisPan(dy: -.5);

            TestTools.SaveFig(plt, "2");
            var limits2 = plt.GetAxisLimits();

            Assert.AreEqual(limits1.YSpan, limits2.YSpan);
        }
    }
}
