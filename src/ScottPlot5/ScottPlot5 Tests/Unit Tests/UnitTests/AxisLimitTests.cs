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

    [Test]
    public void Test_AxisLimits_WithZoom()
    {
        AxisLimits limits1 = new(5, 10, 25, 50);
        AxisLimits limits2 = limits1.WithZoom(0.5, 0.25);
        limits2.Left.Should().Be(2.5);
        limits2.Right.Should().Be(12.5);
        limits2.Bottom.Should().Be(-12.5);
        limits2.Top.Should().Be(87.5);
    }

    [Test]
    public void Test_AxisLimits_WithZoomTo()
    {
        AxisLimits limits1 = new(-20, 20, -40, 40);
        AxisLimits limits2 = limits1.WithZoom(0.5, 0.5, 10, 20);
        limits2.Left.Should().Be(-50);
        limits2.Right.Should().Be(30);
        limits2.Bottom.Should().Be(-100);
        limits2.Top.Should().Be(60);
    }
}
