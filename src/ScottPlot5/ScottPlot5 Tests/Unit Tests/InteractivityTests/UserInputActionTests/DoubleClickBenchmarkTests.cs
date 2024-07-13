using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class DoubleClickBenchmarkTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_DoubleClick_ShowsBenchmark()
    {
        Plot plot = new();
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        UserInputProcessor proc = new(plot);

        plot.Benchmark.IsVisible.Should().BeFalse();

        // click one
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new LeftMouseUp(FIGURE_CENTER));

        // click two
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new LeftMouseUp(FIGURE_CENTER));

        plot.Benchmark.IsVisible.Should().BeTrue();

        // click one
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new LeftMouseUp(FIGURE_CENTER));

        // click two
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new LeftMouseUp(FIGURE_CENTER));

        plot.Benchmark.IsVisible.Should().BeFalse();
    }
}
