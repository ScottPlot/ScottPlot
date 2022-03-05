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

            var sig1 = plt.AddSignal(ScottPlot.DataGen.Sin(51, mult: 1, phase: 0), sampleRate: 1);
            var sig2 = plt.AddSignal(ScottPlot.DataGen.Sin(51, mult: 10, phase: -.1), sampleRate: .1);
            var sig3 = plt.AddSignal(ScottPlot.DataGen.Sin(51, mult: 100, phase: -.2), sampleRate: .01);
            var sig4 = plt.AddSignal(ScottPlot.DataGen.Sin(51, mult: 1000, phase: -.3), sampleRate: .001);

            sig1.YAxisIndex = 0;
            sig2.YAxisIndex = 1;
            sig3.YAxisIndex = 2;
            sig4.YAxisIndex = 3;

            sig1.XAxisIndex = 0;
            sig2.XAxisIndex = 1;
            sig3.XAxisIndex = 2;
            sig4.XAxisIndex = 3;

            // by default ther are already 4 axes, so customize them all
            plt.XAxis.Label("Primary Bottom Axis", color: sig1.Color);
            plt.YAxis.Label("Primary Left Axis", color: sig1.Color);
            plt.XAxis2.Label("Primary Top Axis", color: sig2.Color);
            plt.YAxis2.Label("Primary Right Axis", color: sig2.Color);

            // create a new axis for each side
            plt.AddAxis(ScottPlot.Renderable.Edge.Bottom, 2, "Secondary Bottom Axis", sig3.Color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Left, 2, "Secondary Left Axis", sig3.Color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Top, 3, "Secondary Top Axis", sig4.Color);
            plt.AddAxis(ScottPlot.Renderable.Edge.Right, 3, "Secondary Right Axis", sig4.Color);

            TestTools.SaveFig(plt);
        }
    }
}
