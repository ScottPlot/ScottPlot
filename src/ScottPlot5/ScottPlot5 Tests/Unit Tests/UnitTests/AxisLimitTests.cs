namespace ScottPlotTests.UnitTests;

internal class AxisLimitTests
{
    [Test]
    public void Test_AxisLimits_StandardAxes()
    {
        ScottPlot.Plot plot = new();
        plot.Add.Signal(Generate.Sin(51));

        plot.RenderInMemory();
        AxisLimits limits = plot.Axes.GetLimits();

        limits.Left.Should().BeApproximately(0, 5);
        limits.Right.Should().BeApproximately(50, 5);
        limits.Bottom.Should().BeApproximately(-1, .2);
        limits.Top.Should().BeApproximately(1, .2);
    }

    [Test]
    public void Test_AxisLimits_SecondaryAxes()
    {
        ScottPlot.Plot plot = new();
        var sig = plot.Add.Signal(Generate.Sin(51));
        sig.Axes.YAxis = plot.Axes.Right;

        plot.RenderInMemory();
        AxisLimits limits = plot.Axes.GetLimits(plot.Axes.Bottom, plot.Axes.Right);

        limits.Left.Should().BeApproximately(0, 5);
        limits.Right.Should().BeApproximately(50, 5);
        limits.Bottom.Should().BeApproximately(-1, .2);
        limits.Top.Should().BeApproximately(1, .2);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_SetLimitsY()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3615

        ScottPlot.Plot plot = new();
        var sig = plot.Add.Signal(Generate.Sin(51));

        plot.Axes.AutoScale();
        plot.Axes.SetLimitsY(-50, 100);

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().BeApproximately(0, 5);
        limits.Right.Should().BeApproximately(50, 5);
        limits.Bottom.Should().Be(-50);
        limits.Top.Should().Be(100);
    }

    [Test]
    public void Test_SetLimitsX()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3615

        ScottPlot.Plot plot = new();
        var sig = plot.Add.Signal(Generate.Sin(51));

        plot.Axes.AutoScale();
        plot.Axes.SetLimitsX(-50, 100);

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().Be(-50);
        limits.Right.Should().Be(100);
        limits.Bottom.Should().BeApproximately(-1, .2);
        limits.Top.Should().BeApproximately(1, .2);
    }
}
