namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class LeftClickDragPanTests
{
    [Test]
    public void Test_LeftClickDragPan_PanButDoesNotZoom()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_LeftClickDragPan_ShiftLocksHorizontalAxis()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressShift();
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_LeftClickDragPan_CtrlLocksVerticalAxis()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_LeftClickDragPan_PreservesInvertedAxes()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        plotControl.Plot.Add.Signal(Generate.Sin());

        // setup inverted axes
        plotControl.Plot.Axes.SetLimitsY(1, -1);
        plotControl.Plot.Axes.GetLimits().YRange.Span.Should().BeNegative();

        // pan with the mouse
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));

        // axes should still be inverted
        plotControl.Plot.Axes.GetLimits().YRange.Span.Should().BeNegative();
    }
}
