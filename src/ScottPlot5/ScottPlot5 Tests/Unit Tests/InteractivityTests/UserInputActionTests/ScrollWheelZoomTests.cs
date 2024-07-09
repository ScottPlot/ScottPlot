using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class ScrollWheelZoomTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_UPPER_RIGHT => new(FIGURE_WIDTH * .75, FIGURE_HEIGHT * .25);

    [Test]
    public void Test_ScrollWheel_UpZoomsIn()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate mouse scroll wheel zoom in near the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new MouseWheelUp(FIGURE_UPPER_RIGHT));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ScrollWheel_DownZoomsOut()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate mouse scroll wheel zoom in near the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new MouseWheelDown(FIGURE_UPPER_RIGHT));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeLessThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeLessThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeGreaterThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeGreaterThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftScrollWheel_UpZoomsInVertically()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT +  mouse scroll wheel zoom in near the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new MouseWheelUp(FIGURE_UPPER_RIGHT));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftScrollWheel_UpZoomsInHorizontally()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT +  mouse scroll wheel zoom in near the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new MouseWheelUp(FIGURE_UPPER_RIGHT));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom-in occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
