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
            var sig2 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 10, phase: .2), sampleRate: .1);
            var sig3 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 100, phase: .4), sampleRate: .01);
            var sig4 = plt.PlotSignal(ScottPlot.DataGen.Sin(51, mult: 1000, phase: .6), sampleRate: .001);

            sig1.VerticalAxisIndex = 0;
            sig2.VerticalAxisIndex = 1;
            sig3.VerticalAxisIndex = 2;
            sig4.VerticalAxisIndex = 3;

            sig1.HorizontalAxisIndex = 0;
            sig2.HorizontalAxisIndex = 1;
            sig3.HorizontalAxisIndex = 2;
            sig4.HorizontalAxisIndex = 3;

            // by default ther are already 4 axes, so customize them all

            plt.GetSettings(false).XAxis.Title.Label = "Primary Bottom Axis";
            plt.GetSettings(false).XAxis.Title.IsVisible = true;
            plt.GetSettings(false).XAxis.Configure(color: sig1.color);

            plt.GetSettings(false).YAxis.Title.Label = "Primary Left Axis";
            plt.GetSettings(false).YAxis.Title.IsVisible = true;
            plt.GetSettings(false).YAxis.Configure(color: sig1.color);
            
            plt.GetSettings(false).XAxis2.Title.Label = "Primary Top Axis";
            plt.GetSettings(false).XAxis2.Title.Font.Bold = false;
            plt.GetSettings(false).XAxis2.IsVisible = true;
            plt.GetSettings(false).XAxis2.Title.IsVisible = true;
            plt.GetSettings(false).XAxis2.Ticks.MajorTickEnable = true;
            plt.GetSettings(false).XAxis2.Ticks.MajorLabelEnable = true;
            plt.GetSettings(false).XAxis2.Ticks.MinorTickEnable = true;
            plt.GetSettings(false).XAxis2.Configure(color: sig2.color);

            plt.GetSettings(false).YAxis2.Title.Label = "Primary Right Axis";
            plt.GetSettings(false).YAxis2.IsVisible = true;
            plt.GetSettings(false).YAxis2.Title.IsVisible = true;
            plt.GetSettings(false).YAxis2.Ticks.MajorTickEnable = true;
            plt.GetSettings(false).YAxis2.Ticks.MajorLabelEnable = true;
            plt.GetSettings(false).YAxis2.Ticks.MinorTickEnable = true;
            plt.GetSettings(false).YAxis2.Configure(color: sig2.color);

            // create a new axis for each side

            var bottom = new ScottPlot.Renderable.AdditionalBottomAxis(3, true);
            bottom.Title.Label = "Secondary Bottom Axis";
            bottom.Configure(color: sig3.color);
            plt.GetSettings(false).Axes.Add(bottom);

            var left = new ScottPlot.Renderable.AdditionalLeftAxis(2, true);
            left.Title.Label = "Secondary Left Axis";
            left.Configure(color: sig3.color);
            plt.GetSettings(false).Axes.Add(left);

            var top = new ScottPlot.Renderable.AdditionalTopAxis(2, true);
            top.Title.Label = "Secondary Top Axis";
            top.Configure(color: sig4.color);
            plt.GetSettings(false).Axes.Add(top);

            var right = new ScottPlot.Renderable.AdditionalRightAxis(3, true);
            right.Title.Label = "Secondary Right Axis";
            right.Configure(color: sig4.color);
            plt.GetSettings(false).Axes.Add(right);

            TestTools.SaveFig(plt);
        }
    }
}
