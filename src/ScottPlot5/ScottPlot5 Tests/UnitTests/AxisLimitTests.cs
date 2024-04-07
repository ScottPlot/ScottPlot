namespace ScottPlotTests.UnitTests;

internal class AxisLimitTests
{
    [Test]
    public void Test_AxisLimits_StandardAxes()
    {
        ScottPlot.Plot plot = new();
        plot.Add.Signal(Generate.Sin(51));

        AxisLimits limits = plot.Axes.GetLimits();

        limits.Left.Should().BeApproximately(0, 5);
        limits.Right.Should().BeApproximately(50, 5);
        limits.Bottom.Should().BeApproximately(-1, .2);
        limits.Top.Should().BeApproximately(1, .2);
    }
}
