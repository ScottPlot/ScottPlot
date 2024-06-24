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

    [Test]
    public void Test_AutoScale_DefinedPlottables()
    {
        Plot plot = new();

        var sig1 = plot.Add.Signal(Generate.Sin());
        sig1.Data.XOffset = 100;
        sig1.Data.YOffset = 100;

        var sig2 = plot.Add.Signal(Generate.Sin());
        sig2.Data.XOffset = 200;
        sig2.Data.YOffset = 200;

        var sig3 = plot.Add.Signal(Generate.Sin());
        sig3.Data.XOffset = 300;
        sig3.Data.YOffset = 300;

        plot.Axes.AutoScale();
        plot.Axes.GetLimits().Left.Should().BeGreaterThan(0);
        plot.Axes.GetLimits().Left.Should().BeLessThan(100);
        plot.Axes.GetLimits().Right.Should().BeGreaterThan(350);
        plot.Axes.GetLimits().Right.Should().BeLessThan(400);
        plot.Axes.GetLimits().Bottom.Should().BeGreaterThan(50);
        plot.Axes.GetLimits().Bottom.Should().BeLessThan(100);
        plot.Axes.GetLimits().Top.Should().BeGreaterThan(310);
        plot.Axes.GetLimits().Top.Should().BeLessThan(350);

        IPlottable[] plottablesToScale = [sig2];
        plot.Axes.AutoScale(plottablesToScale);
        plot.Axes.GetLimits().Left.Should().Be(200);
        plot.Axes.GetLimits().Right.Should().Be(250);
        plot.Axes.GetLimits().Bottom.Should().BeApproximately(199, .1);
        plot.Axes.GetLimits().Top.Should().BeApproximately(201, .1);
    }

    [Test]
    public void Test_Autoscale_ExtremelyLarge()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3930

        Plot plot = new();
        plot.Add.Marker(1e100, 1e100);
        plot.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Autoscale_ExtremelySmall()
    {
        Plot plot = new();
        plot.Add.Marker(1e-100, 1e-100);
        plot.Should().RenderInMemoryWithoutThrowing();
    }
}
