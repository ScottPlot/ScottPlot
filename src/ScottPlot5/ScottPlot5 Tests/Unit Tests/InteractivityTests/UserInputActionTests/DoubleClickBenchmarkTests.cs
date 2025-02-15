namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class DoubleClickBenchmarkTests
{
    [Test]
    public void Test_DoubleClick_ShowsBenchmark()
    {
        ScottPlot.Testing.MockPlotControl plotControl = new();

        plotControl.Plot.Benchmark.IsVisible.Should().BeFalse();

        plotControl.LeftClick(plotControl.Center);
        plotControl.LeftClick(plotControl.Center);
        plotControl.Plot.Benchmark.IsVisible.Should().BeTrue();

        plotControl.LeftClick(plotControl.Center);
        plotControl.LeftClick(plotControl.Center);
        plotControl.Plot.Benchmark.IsVisible.Should().BeFalse();
    }
}
