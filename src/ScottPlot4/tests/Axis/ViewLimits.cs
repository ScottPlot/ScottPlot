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
        public void Test_View_LimitsOuter()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            plt.SetOuterViewLimits(xMin: -123, xMax: 123, yMin: -13, yMax: 13);
            TestTools.SaveFig(plt, "1");
            for (int i = 0; i < 10; i++)
                plt.AxisZoom(.5, .5);

            TestTools.SaveFig(plt, "2");
            var limits2 = plt.GetAxisLimits();

            Assert.GreaterOrEqual(limits2.XMin, -123);
            Assert.GreaterOrEqual(limits2.XMax, 123);
            Assert.GreaterOrEqual(limits2.YMin, -13);
            Assert.GreaterOrEqual(limits2.YMax, 13);
        }

        [Test]
        public void Test_View_LimitsInner()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            plt.SetInnerViewLimits(xMin: 10, xMax: 20, yMin: .1, yMax: .2);
            plt.AxisAuto();

            TestTools.SaveFig(plt, "1");
            for (int i = 0; i < 10; i++)
                plt.AxisZoom(2, 2);

            TestTools.SaveFig(plt, "2");
            var limits2 = plt.GetAxisLimits();

            Assert.LessOrEqual(limits2.XMin, 10);
            Assert.LessOrEqual(limits2.XMax, 20);
            Assert.LessOrEqual(limits2.YMin, .1);
            Assert.LessOrEqual(limits2.YMax, .2);
        }
    }
}
