using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class ArrowKeyPanAndZoomTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;

    [Test]
    public void Test_ArrowKey_Pan()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate panning to the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Right));
        proc.Process(new KeyDown(StandardKeys.Up));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_CtrlArrowKey_Zoom()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate zooming in
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new KeyDown(StandardKeys.Right));
        proc.Process(new KeyDown(StandardKeys.Up));
        AxisLimits newLimits = plot.Axes.GetLimits();

        // assert no pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom occurred
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }
}
