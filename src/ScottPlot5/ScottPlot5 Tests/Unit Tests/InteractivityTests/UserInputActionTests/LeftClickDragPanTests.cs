using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserInputs;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class LeftClickDragPanTests
{
    const int PLOT_WIDTH = 400;
    const int PLOT_HEIGHT = 300;
    Pixel ImageCenter => new(PLOT_HEIGHT / 2, PLOT_WIDTH / 2);

    [Test]
    public void Test_LeftClickDragPan_PanButDoesNotZoom()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate left-click-drag to the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new LeftMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.WithOffset(-50, 50))); // TODO: why is this reversed????
        AxisLimits newLimits = plot.Axes.GetLimits();

        Console.WriteLine(originalLimits);
        Console.WriteLine(newLimits);

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_LeftClickDrag_ShiftLocksHorizontalAxis()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate SHIFT + left-click-drag to the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Shift));
        proc.Process(new LeftMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.WithOffset(-50, 50))); // TODO: why is this reversed????
        AxisLimits newLimits = plot.Axes.GetLimits();

        Console.WriteLine(originalLimits);
        Console.WriteLine(newLimits);

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_LeftClickDrag_CtrlLocksVerticalAxis()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.RenderInMemory(PLOT_WIDTH, PLOT_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate ALT + left-click-drag to the upper right
        UserInputProcessor proc = new(plot);
        proc.Process(new KeyDown(StandardKeys.Control));
        proc.Process(new LeftMouseDown(ImageCenter));
        proc.Process(new MouseMove(ImageCenter.WithOffset(-50, 50))); // TODO: why is this reversed????
        AxisLimits newLimits = plot.Axes.GetLimits();

        Console.WriteLine(originalLimits);
        Console.WriteLine(newLimits);

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
