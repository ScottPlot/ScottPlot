using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class RightClickDragZoomTests
{
    const int PLOT_WIDTH = 400;
    const int PLOT_HEIGHT = 300;
    Pixel ImageCenter => new(PLOT_HEIGHT / 2, PLOT_WIDTH / 2);

    [Test]
    public void Test_RightClickDragZoom_ZoomsWithoutPanning()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new RightMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.MovedRight(50).MovedUp(50)));
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
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT + right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new RightMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.MovedRight(50).MovedUp(50)));
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
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate ALT + right-click-drag to to the upper right (zooming in)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new RightMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.MovedRight(50).MovedUp(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert zoom occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no pan occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
