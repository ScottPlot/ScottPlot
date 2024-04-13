using FluentAssertions;
using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.Axis;

internal class PanAndZoom
{
    [Test]
    public void Test_PanCenter_PrimaryAxes()
    {
        ScottPlot.Plot plt = new();

        plt.SetAxisLimits(-5, 5, -25, 25);
        var limits1 = plt.GetAxisLimits();
        limits1.XMin.Should().Be(-5);
        limits1.XMax.Should().Be(5);
        limits1.XCenter.Should().Be(0);
        limits1.YMin.Should().Be(-25);
        limits1.YMax.Should().Be(25);
        limits1.YCenter.Should().Be(0);

        plt.AxisPanCenter(1, 1);
        var limits2 = plt.GetAxisLimits();
        limits2.XMin.Should().Be(-5 + 1);
        limits2.XMax.Should().Be(5 + 1);
        limits2.XCenter.Should().Be(0 + 1);
        limits2.YMin.Should().Be(-25 + 1);
        limits2.YMax.Should().Be(25 + 1);
        limits2.YCenter.Should().Be(0 + 1);
    }

    [Test]
    public void Test_PanCenter_SecondaryAxes()
    {
        ScottPlot.Plot plt = new();

        plt.SetAxisLimits(-1, 1, -1, 1);
        var originalPrimaryLimits = plt.GetAxisLimits();

        plt.SetAxisLimits(-5, 5, -25, 25, xAxisIndex: 1, yAxisIndex: 1);

        var limits1 = plt.GetAxisLimits(xAxisIndex: 1, yAxisIndex: 1);
        limits1.XMin.Should().Be(-5);
        limits1.XMax.Should().Be(5);
        limits1.XCenter.Should().Be(0);
        limits1.YMin.Should().Be(-25);
        limits1.YMax.Should().Be(25);
        limits1.YCenter.Should().Be(0);

        plt.AxisPanCenter(1, 1, xAxisIndex: 1, yAxisIndex: 1);
        var limits2 = plt.GetAxisLimits(xAxisIndex: 1, yAxisIndex: 1);
        limits2.XMin.Should().Be(-5 + 1);
        limits2.XMax.Should().Be(5 + 1);
        limits2.XCenter.Should().Be(0 + 1);
        limits2.YMin.Should().Be(-25 + 1);
        limits2.YMax.Should().Be(25 + 1);
        limits2.YCenter.Should().Be(0 + 1);

        plt.GetAxisLimits().Should().Be(originalPrimaryLimits);
    }

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

        // pan secondary
        plt.AxisPanCenter(1, 1, xAxisIndex: 1, yAxisIndex: 1);
        plt.GetAxisLimits().Should().Be(primaryLimits);
    }

    [Test]
    public void Test_PanCenter_MultiAxis()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2483

        ScottPlot.Plot plt = new(400, 300);

        // All 6 plots use a common Primary Xaxis
        var mark1 = plt.AddMarker(1, 1);
        var mark2 = plt.AddMarker(2, 2);
        var mark3 = plt.AddMarker(3, 3);
        var mark4 = plt.AddMarker(4, 4);
        var mark5 = plt.AddMarker(5, 5);
        var mark6 = plt.AddMarker(6, 6);

        // 3 plots using Primary YAxis
        mark1.YAxisIndex = 0;
        mark2.YAxisIndex = 0;
        mark3.YAxisIndex = 0;

        // 3 plots using Secondary Yaxis
        mark4.YAxisIndex = 1;
        mark5.YAxisIndex = 1;
        mark6.YAxisIndex = 1;
        plt.YAxis2.Ticks(true);

        // show the original axis limits
        plt.SetAxisLimits(0, 10, 0, 10, yAxisIndex: 0);
        plt.SetAxisLimits(0, 10, 0, 10, yAxisIndex: 1);
        TestTools.SaveFig(plt, "1");

        // put a mark at a new position and center on it
        plt.AddMarker(2.5, 2.5, ScottPlot.MarkerShape.asterisk);
        plt.AxisPanCenter(2.5, 2.5, yAxisIndex: 0);
        TestTools.SaveFig(plt, "2");

        // trying to center on a non-primary axis demonstrates the issue
        plt.AxisPanCenter(2.5, 2.5, yAxisIndex: 1);
        TestTools.SaveFig(plt, "3");
    }
}
