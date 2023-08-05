namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class PixelTests
{
    [Test]
    public void Test_Pixel_DefaultValues()
    {
        Pixel pixel = new();
        pixel.X.Should().Be(0);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -2)]
    [TestCase(-3, 4)]
    [TestCase(5, -6)]
    [TestCase(0, float.NaN)] // permitted
    [TestCase(0, float.PositiveInfinity)] // permitted
    public void Test_Pixel_Construct(float x, float y)
    {
        Pixel pixel = new(x, y);
        pixel.X.Should().Be(x);
        pixel.Y.Should().Be(y);
    }

    [Test]
    public void Test_Pixel_CustomToString()
    {
        new Pixel().ToString().Should().Contain("X =");
    }
}
