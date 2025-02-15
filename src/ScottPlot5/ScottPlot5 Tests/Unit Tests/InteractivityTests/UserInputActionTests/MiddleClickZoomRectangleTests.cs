namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class MiddleClickZoomRectangleTests
{
    [Test]
    public void Test_MiddleClickDragZoomRectangle_Zooms()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.Plot.ZoomRectangle.IsVisible.Should().BeFalse();
        plotControl.MiddleMouseDown(plotControl.Center);
        plotControl.MoveMouse(plotControl.Center.MovedRight(100).MovedUp(100));
        plotControl.Plot.ZoomRectangle.IsVisible.Should().BeTrue();
        plotControl.MiddleMouseUp(plotControl.Center.MovedRight(100).MovedUp(100));
        plotControl.Plot.ZoomRectangle.IsVisible.Should().BeFalse();

        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftMiddleClickDragZoomRectangle_OnlyZoomsVertically()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressShift();
        plotControl.MiddleClickDrag(plotControl.Center, plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_CtrlMiddleClickDragZoomRectangle_OnlyZoomsHorizontally()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.MiddleClickDrag(plotControl.Center, plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_AltLeftClickDragZoomRectangle_Zooms()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressAlt();
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }
}
