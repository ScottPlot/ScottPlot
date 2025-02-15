namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class RightClickDragZoomTests
{
    [Test]
    public void Test_RightClickDragZoom_ZoomsWithoutPanning()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.RightClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert zoom
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_RightClickDragZoom_ShiftLocksHorizontalAxis()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressShift();
        plotControl.RightClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert zoom
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_RightClickDragZoom_CtrlLocksVerticalAxis()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.RightClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert zoom
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
