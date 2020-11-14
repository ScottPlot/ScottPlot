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
            plt.XLabel("Primary Bottom Axis", color: sig1.color);
            plt.YLabel("Primary Left Axis", color: sig1.color);
            plt.XLabel2("Primary Top Axis", color: sig2.color, bold: false);
            plt.YLabel2("Primary Right Axis", color: sig2.color);

            // create a new axis for each side

            var bottom = new ScottPlot.Renderable.AdditionalBottomAxis(xAxisIndex: 2);
            bottom.Title.Label = "Secondary Bottom Axis";
            bottom.Configure(color: sig3.color);
            plt.GetSettings(false).Axes.Add(bottom);

            var left = new ScottPlot.Renderable.AdditionalLeftAxis(yAxisIndex: 2);
            left.Title.Label = "Secondary Left Axis";
            left.Configure(color: sig3.color);
            plt.GetSettings(false).Axes.Add(left);

            var top = new ScottPlot.Renderable.AdditionalTopAxis(xAxisIndex: 3);
            top.Title.Label = "Secondary Top Axis";
            top.Configure(color: sig4.color);
            plt.GetSettings(false).Axes.Add(top);

            var right = new ScottPlot.Renderable.AdditionalRightAxis(yAxisIndex: 3);
            right.Title.Label = "Secondary Right Axis";
            right.Configure(color: sig4.color);
            plt.GetSettings(false).Axes.Add(right);

            TestTools.SaveFig(plt);
        }
    }
}
