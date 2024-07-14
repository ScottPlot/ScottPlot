using ScottPlot.Interactivity;
using ScottPlot.Interactivity.PlotResponses;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class KeyboardAutoscaleTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_KeyboardAutoscale_ResetsAxisLimits()
    {
        // create a plot and force a render to allow pixel-based interactions
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.RenderInMemory(FIGURE_WIDTH, FIGURE_HEIGHT);
        AxisLimits originalLimits = plot.Axes.GetLimits();

        // simulate left-click-drag to to the lower left (panning to the upper right)
        UserInputProcessor proc = new(plot);
        proc.Process(new LeftMouseDown(FIGURE_CENTER));
        proc.Process(new MouseMove(FIGURE_CENTER.MovedLeft(50).MovedDown(50)));
        AxisLimits changedLimits = plot.Axes.GetLimits();
        changedLimits.Center.Should().NotBe(originalLimits.Center);

        // keyboard AutoAxis
        proc.Process(new KeyDown(StandardKeys.A));
        AxisLimits resetLimits = plot.Axes.GetLimits();
        resetLimits.Center.Should().Be(originalLimits.Center);
    }
}
