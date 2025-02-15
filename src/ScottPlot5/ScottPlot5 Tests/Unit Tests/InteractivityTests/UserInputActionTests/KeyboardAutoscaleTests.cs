namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class KeyboardAutoscaleTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_KeyboardAutoscale_ResetsAxisLimits()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();
        plotControl.Plot.Add.Signal(Generate.Sin());
        plotControl.Plot.Add.Signal(Generate.Cos());

        // start out autoscaled
        plotControl.Plot.Axes.AutoScale();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        // slide the plot
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        plotControl.Plot.Axes.GetLimits().Center.Should().NotBe(originalLimits.Center);

        // keyboard AutoAxis
        plotControl.PressKey(ScottPlot.Interactivity.StandardKeys.A);
        plotControl.Plot.Axes.GetLimits().Center.Should().Be(originalLimits.Center);
    }
}
