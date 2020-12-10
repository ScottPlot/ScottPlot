using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class MultiAxis
    {
        [Test]
        public void Test_MutiAxis_AllSides()
        {
            // plot data using 4 different vertical axis indexes
            var plt = new ScottPlot.Plot();

            var sig1 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 1, phase: 0), sampleRate: 1);
            var sig2 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 10, phase: -.1), sampleRate: .1);
            var sig3 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 100, phase: -.2), sampleRate: .01);
            var sig4 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 1000, phase: -.3), sampleRate: .001);

            sig1.VerticalAxisIndex = 0;
            sig2.VerticalAxisIndex = 1;
            sig3.VerticalAxisIndex = 2;
            sig4.VerticalAxisIndex = 3;

            sig1.HorizontalAxisIndex = 0;
            sig2.HorizontalAxisIndex = 1;
            sig3.HorizontalAxisIndex = 2;
            sig4.HorizontalAxisIndex = 3;

            // by default ther are already 4 axes, so customize them all
            plt.XAxis.ConfigureAxisLabel("Primary Bottom Axis", color: sig1.color);
            plt.YAxis.ConfigureAxisLabel("Primary Left Axis", color: sig1.color);
            plt.XAxis2.ConfigureAxisLabel("Primary Top Axis", color: sig2.color);
            plt.YAxis2.ConfigureAxisLabel("Primary Right Axis", color: sig2.color);

            // create a new axis for each side
            plt.AddAxis(ScottPlot.Renderable.Edge.Bottom, 2, "Secondary Bottom Axis", sig3.color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Left, 2, "Secondary Left Axis", sig3.color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Top, 3, "Secondary Top Axis", sig4.color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Right, 3, "Secondary Right Axis", sig4.color);

            TestTools.SaveFig(plt);
        }
    }
}
