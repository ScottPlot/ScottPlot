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
        newLimits.XMin.Should().BeGreaterThan(initialLimits.XMin);
        newLimits.XMax.Should().BeGreaterThan(initialLimits.XMax);
        newLimits.YMin.Should().BeGreaterThan(initialLimits.YMin);
        newLimits.YMax.Should().BeGreaterThan(initialLimits.YMax);
    }

    [Test]
    public void Test_Plot_Zoom()
    {
        Plot plt = new();

        plt.SetAxisLimits(-7, 42, -13, 69);
        plt.GetAxisLimits().XMin.Should().Be(-7);
        plt.GetAxisLimits().XMax.Should().Be(42);
        plt.GetAxisLimits().YMin.Should().Be(-13);
        plt.GetAxisLimits().YMax.Should().Be(69);

        plt.Zoom(2, 1);
        plt.GetAxisLimits().XMin.Should().BeGreaterThan(-7);
        plt.GetAxisLimits().XMax.Should().BeLessThan(42);
        plt.GetAxisLimits().YMin.Should().Be(-13);
        plt.GetAxisLimits().YMax.Should().Be(69);

        plt.Zoom(1, 2);
        plt.GetAxisLimits().XMin.Should().BeGreaterThan(-7);
        plt.GetAxisLimits().XMax.Should().BeLessThan(42);
        plt.GetAxisLimits().YMin.Should().BeGreaterThan(-13);
        plt.GetAxisLimits().YMax.Should().BeLessThan(69);

        plt.Zoom();
    }
}
