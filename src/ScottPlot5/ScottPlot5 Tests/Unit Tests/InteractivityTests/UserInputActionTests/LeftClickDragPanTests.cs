using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class LeftClickDragPanTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_LeftClickDragPan_PanButDoesNotZoom()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate left-click-drag to to the lower left (panning to the upper right)
        UserInputProcessor proc = new(plot);
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedLeft(50).MovedDown(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

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
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT + left-click-drag to to the lower left (panning to the upper right)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedLeft(50).MovedDown(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

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
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate ALT + left-click-drag to to the lower left (panning to the upper right)
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedLeft(50).MovedDown(50)));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
