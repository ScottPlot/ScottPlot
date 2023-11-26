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

        AxisLimits limits = plot.GetAxisLimits();
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
        plot.Margins(0, 0);
        plot.SaveTestImage();

        AxisLimits limits = plot.GetAxisLimits();
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
        plot.Margins(0, .1, 1, 2);
        plot.SaveTestImage();
    }
}
