namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateTests
{
    [Test]
    public void Test_Coordinate_DefaultValues()
    {
        Coordinates coord = new();
        coord.X.Should().Be(0);
        coord.Y.Should().Be(0);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -2)]
    [TestCase(-3, 4)]
    [TestCase(5, -6)]
    [TestCase(0, double.NaN)] // permitted
    [TestCase(0, double.PositiveInfinity)] // permitted
    public void Test_Coordinate_Construct(double x, double y)
    {
        Coordinates coord = new(x, y);
        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
    }

    [Test]
    public void Test_Coordinate_CustomToString()
    {
        new Coordinates().ToString().Should().Contain("X =");
    }
}
