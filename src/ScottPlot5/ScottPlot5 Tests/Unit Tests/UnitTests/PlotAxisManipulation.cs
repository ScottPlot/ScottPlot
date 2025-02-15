namespace ScottPlotTests.UnitTests;

internal class PlotAxisManipulation
{
    [Test]
    public void Test_Plot_Pan()
    {
        Plot plt = new();
        plt.Axes.SetLimits(-7, 42, -13, 69);
        plt.Axes.Pan(new CoordinateOffset(1, 2));

        AxisLimits expectedLimits = new(-6, 43, -11, 71);
        plt.Axes.GetLimits().Should().Be(expectedLimits);
    }

    [Test]
    public void Test_Plot_Pan_Pixels()
    {
        Plot plt = new();

        AxisLimits initialLimits = new(-7, 42, -13, 69);
        plt.Axes.SetLimits(initialLimits);

        Pixel px1 = Pixel.Zero;
        Pixel px2 = px1.MovedLeft(20).MovedDown(10);

        plt.Should().RenderInMemoryWithoutThrowing();

        plt.Axes.Pan(px1, px2);
        AxisLimits newLimits = plt.Axes.GetLimits();
        newLimits.Left.Should().BeGreaterThan(initialLimits.Left);
        newLimits.Right.Should().BeGreaterThan(initialLimits.Right);
        newLimits.Bottom.Should().BeGreaterThan(initialLimits.Bottom);
        newLimits.Top.Should().BeGreaterThan(initialLimits.Top);
    }

    [Test]
    public void Test_Plot_Zoom()
    {
        Plot plt = new();

        plt.Axes.SetLimits(-7, 42, -13, 69);
        plt.Axes.GetLimits().Left.Should().Be(-7);
        plt.Axes.GetLimits().Right.Should().Be(42);
        plt.Axes.GetLimits().Bottom.Should().Be(-13);
        plt.Axes.GetLimits().Top.Should().Be(69);

        plt.Axes.Zoom(2, 1);
        plt.Axes.GetLimits().Left.Should().BeGreaterThan(-7);
        plt.Axes.GetLimits().Right.Should().BeLessThan(42);
        plt.Axes.GetLimits().Bottom.Should().Be(-13);
        plt.Axes.GetLimits().Top.Should().Be(69);

        plt.Axes.Zoom(1, 2);
        plt.Axes.GetLimits().Left.Should().BeGreaterThan(-7);
        plt.Axes.GetLimits().Right.Should().BeLessThan(42);
        plt.Axes.GetLimits().Bottom.Should().BeGreaterThan(-13);
        plt.Axes.GetLimits().Top.Should().BeLessThan(69);

        plt.Axes.Zoom();
    }
}
