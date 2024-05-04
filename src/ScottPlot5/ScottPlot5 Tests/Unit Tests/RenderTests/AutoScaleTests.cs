namespace ScottPlotTests.RenderTests;

internal class AutoScaleTests
{
    [Test]
    public void Test_Autoscale_Default()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().BeLessThan(0);
        limits.Right.Should().BeGreaterThan(50);
        limits.Bottom.Should().BeLessThan(-1);
        limits.Top.Should().BeGreaterThan(1);
    }

    [Test]
    public void Test_Autoscale_Tight()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.Axes.Margins(0, 0);
        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().Be(-1);
        limits.Top.Should().Be(1);
    }

    [Test]
    public void Test_Autoscale_Custom()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.Axes.Margins(0, .1, 1, 2);
        plot.SaveTestImage();
    }

    [Test]
    public void Test_ManualLimits()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.Axes.SetLimits(-1, 2, -3, 4);
        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().Be(-1);
        limits.Right.Should().Be(2);
        limits.Bottom.Should().Be(-3);
        limits.Top.Should().Be(4);
    }

    [Test]
    public void Test_ManualLimits_X()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.Axes.SetLimits(left: 2, right: 5);
        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().Be(2);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().BeLessThan(-1);
        limits.Top.Should().BeGreaterThan(1);
    }

    [Test]
    public void Test_ManualLimits_Y()
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        plot.Axes.SetLimits(bottom: -2, top: 5);
        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().BeLessThan(0);
        limits.Right.Should().BeGreaterThan(50);
        limits.Bottom.Should().Be(-2);
        limits.Top.Should().Be(5);
    }

    [Test]
    public void Test_AutoScale_NoVisiblePlots()
    {
        Plot plot = new();

        var sig1 = plot.Add.Signal(Generate.Sin());
        sig1.IsVisible = false;

        var sig2 = plot.Add.Signal(Generate.Cos());
        sig2.IsVisible = false;

        plot.SaveTestImage();

        AxisLimits limits = plot.Axes.GetLimits();
        limits.Left.Should().Be(-10);
        limits.Right.Should().Be(10);
        limits.Bottom.Should().Be(-10);
        limits.Top.Should().Be(10);
    }
}
