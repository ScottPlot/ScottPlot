using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class MiddleClickZoomRectangleTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_MiddleClickDragZoomRectangle_Zooms()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate middle-click-drag zoom rectangle toward the upper right
        UserInputProcessor proc = new(plot);
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        proc.Process(new MiddleMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeTrue();
        proc.Process(new MiddleMouseUp(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftMiddleClickDragZoomRectangle_OnlyZoomsHorizontally()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT + middle-click-drag zoom rectangle toward the upper right
        UserInputProcessor proc = new(plot);
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new MiddleMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeTrue();
        proc.Process(new MiddleMouseUp(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_CtrlMiddleClickDragZoomRectangle_OnlyZoomsHorizontally()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate CTRL + middle-click-drag zoom rectangle toward the upper right
        UserInputProcessor proc = new(plot);
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new MiddleMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeTrue();
        proc.Process(new MiddleMouseUp(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_AltLeftClickDragZoomRectangle_Zooms()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate middle-click-drag zoom rectangle toward the upper right
        UserInputProcessor proc = new(plot);
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        proc.Process(new KeyDown(StandardKeys.Alt));
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeTrue();
        proc.Process(new LeftMouseUp(FIGURE_CENTER.MovedRight(100).MovedUp(100)));
        plot.ZoomRectangle.IsVisible.Should().BeFalse();
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }
}
