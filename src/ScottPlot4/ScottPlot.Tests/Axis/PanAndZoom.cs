using FluentAssertions;
using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.Axis;

internal class PanAndZoom
{
    [Test]
    public void Test_SecondaryAxisPan_ShouldNotChangePrimaryAxisLimits()
    {
        // demonstrates issue described by:
        // https://github.com/ScottPlot/ScottPlot/issues/2483

        ScottPlot.Plot plt = new();

        plt.SetAxisLimits(-5, 5, -25, 25);
        AxisLimits primaryLimits = plt.GetAxisLimits();

        // set secondary limits
        plt.SetAxisLimits(95, 105, 75, 125, xAxisIndex: 1, yAxisIndex: 1);
        plt.GetAxisLimits().Should().Be(primaryLimits);

        // pan primary without actually moving
        plt.AxisPanCenter(0, 0, xAxisIndex: 0, yAxisIndex: 0);
        plt.GetAxisLimits().Should().Be(primaryLimits);

        // pan secondary without actually moving
        plt.AxisPanCenter(0, 0, xAxisIndex: 1, yAxisIndex: 1);
        plt.GetAxisLimits().Should().Be(primaryLimits);
    }
}