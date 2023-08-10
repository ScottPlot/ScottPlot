namespace ScottPlotTests.UnitTests;

internal class PlotAxisManipulation
{
    [Test]
    public void Test_Plot_Pan()
    {
        Plot plt = new();
        plt.SetAxisLimits(-7, 42, -13, 69);
        plt.Pan(new CoordinateSize(1, 2));

        AxisLimits expectedLimits = new(-6, 43, -11, 71);
        plt.GetAxisLimits().Should().Be(expectedLimits);
    }

    [Test]
    public void Test_Plot_Pan_Pixels()
    {
        Plot plt = new();

        AxisLimits initialLimits = new(-7, 42, -13, 69);
        plt.SetAxisLimits(initialLimits);

        PixelSize panDistance = new(20, 10);

        Action panBeforeRender = () => plt.Pan(panDistance);
        panBeforeRender.Should().Throw<InvalidOperationException>();

        plt.Render();
        plt.Pan(panDistance);
        AxisLimits newLimits = plt.GetAxisLimits();
        newLimits.Left.Should().BeGreaterThan(initialLimits.Left);
        newLimits.Right.Should().BeGreaterThan(initialLimits.Right);
        newLimits.Bottom.Should().BeGreaterThan(initialLimits.Bottom);
        newLimits.Top.Should().BeGreaterThan(initialLimits.Top);
    }

    [Test]
    public void Test_Plot_Zoom()
    {
        Plot plt = new();

        plt.SetAxisLimits(-7, 42, -13, 69);
        plt.GetAxisLimits().Left.Should().Be(-7);
        plt.GetAxisLimits().Right.Should().Be(42);
        plt.GetAxisLimits().Bottom.Should().Be(-13);
        plt.GetAxisLimits().Top.Should().Be(69);

        plt.Zoom(2, 1);
        plt.GetAxisLimits().Left.Should().BeGreaterThan(-7);
        plt.GetAxisLimits().Right.Should().BeLessThan(42);
        plt.GetAxisLimits().Bottom.Should().Be(-13);
        plt.GetAxisLimits().Top.Should().Be(69);

        plt.Zoom(1, 2);
        plt.GetAxisLimits().Left.Should().BeGreaterThan(-7);
        plt.GetAxisLimits().Right.Should().BeLessThan(42);
        plt.GetAxisLimits().Bottom.Should().BeGreaterThan(-13);
        plt.GetAxisLimits().Top.Should().BeLessThan(69);

        plt.Zoom();
    }
}
