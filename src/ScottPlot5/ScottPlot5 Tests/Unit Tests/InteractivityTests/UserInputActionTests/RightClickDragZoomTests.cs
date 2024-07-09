using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class RightClickDragZoomTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_RightClickDragZoom_ZoomsWithoutPanning()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(50).MovedUp(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert zoom occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_RightClickDragZoom_ShiftLocksHorizontalAxis()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT + right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(50).MovedUp(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert zoom occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_RightClickDragZoom_CtrlLocksVerticalAxis()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate ALT + right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedRight(50).MovedUp(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert zoom occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
