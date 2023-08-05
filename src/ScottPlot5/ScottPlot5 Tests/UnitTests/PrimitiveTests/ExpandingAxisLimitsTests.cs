namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class ExpandingAxisLimitsTests
{
    [Test]
    public void Test_Default_IsNotSet()
    {
        ExpandingAxisLimits limits = new();

        limits.XMin.Should().Be(double.NaN);
        limits.XMax.Should().Be(double.NaN);
        limits.YMin.Should().Be(double.NaN);
        limits.YMax.Should().Be(double.NaN);

        limits.AxisLimits.Should().Be(AxisLimits.NoLimits);
    }

    [Test]
    public void Test_Init_AxisLimits()
    {
        AxisLimits initialLimits = new(-13, 17, -42, 69);
        ExpandingAxisLimits limits = new(initialLimits);
        limits.AxisLimits.Should().Be(initialLimits);
    }

    [Test]
    public void Test_Expand_XY()
    {
        ExpandingAxisLimits limits = new();

        limits.Expand(-7, 13);
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(-7);
        limits.YMin.Should().Be(13);
        limits.YMax.Should().Be(13);

        limits.Expand(42, -69);
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }

    [Test]
    public void Test_Expand_X()
    {
        ExpandingAxisLimits limits = new();

        limits.ExpandX(-7);
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(-7);

        limits.ExpandX(42);
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
    }

    [Test]
    public void Test_Expand_Y()
    {
        ExpandingAxisLimits limits = new();

        limits.ExpandY(13);
        limits.YMin.Should().Be(13);
        limits.YMax.Should().Be(13);

        limits.ExpandY(-69);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }

    [Test]
    public void Test_Expand_Coordinates()
    {
        ExpandingAxisLimits limits = new();

        limits.Expand(new Coordinates(-7, 13));
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(-7);
        limits.YMin.Should().Be(13);
        limits.YMax.Should().Be(13);

        limits.Expand(new Coordinates(42, -69));
        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }

    [Test]
    public void Test_Expand_CoordinateList()
    {
        ExpandingAxisLimits limits = new();

        List<Coordinates> coordinates = new()
        {
            new Coordinates(-7, 13),
            new Coordinates(42, -69)
        };

        limits.Expand(coordinates);

        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }

    [Test]
    public void Test_Expand_CoordinateRect()
    {
        ExpandingAxisLimits limits = new();

        CoordinateRect rect = new(-7, 42, -69, 13);

        limits.Expand(rect);

        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }

    [Test]
    public void Test_Expand_AxisLimits()
    {
        ExpandingAxisLimits limits = new();

        AxisLimits axisLimits = new(-7, 42, -69, 13);

        limits.Expand(axisLimits);

        limits.XMin.Should().Be(-7);
        limits.XMax.Should().Be(42);
        limits.YMin.Should().Be(-69);
        limits.YMax.Should().Be(13);
    }
}
