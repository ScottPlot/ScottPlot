namespace ScottPlot.Tests.UnitTests.Primitives;

internal class CoordinateTests
{
    [Test]
    public void Test_Coordinate_DefaultValues()
    {
        Coordinates coord = new();
        Assert.That(coord.X, Is.Zero);
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
        Assert.That(coord.X, Is.EqualTo(x));
        Assert.That(coord.Y, Is.EqualTo(y));
    }

    [Test]
    public void Test_Coordinate_CustomToString()
    {
        StringAssert.Contains("X=", new Coordinates().ToString());
    }
}
