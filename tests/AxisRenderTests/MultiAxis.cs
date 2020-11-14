using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class MultiAxis
    {
        [Test]
        public void Test_MutiAxis_Basic()
        {
            var plt = new ScottPlot.Plot();

            var YAxis3 = new ScottPlot.Renderable.AdditionalRightAxis(2, true);
            YAxis3.Title.Label = "Tertiary Vertical Axis";
            plt.GetSettings(false).Axes.Add(YAxis3);

            var sig1 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 1, phase: 0));
            var sig2 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 10, phase: .2));
            var sig3 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 100, phase: .4));

            sig1.VerticalAxisIndex = 0;
            sig2.VerticalAxisIndex = 1;
            sig3.VerticalAxisIndex = 2;

            TestTools.SaveFig(plt);
        }
    }
}
