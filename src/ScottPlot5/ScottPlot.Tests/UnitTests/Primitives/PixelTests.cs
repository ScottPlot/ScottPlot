namespace ScottPlot.Tests.UnitTests.Primitives;

internal class PixelTests
{
    [Test]
    public void Test_Pixel_DefaultValues()
    {
        Pixel pixel = new();
        Assert.That(pixel.X, Is.Zero);
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
        Assert.That(pixel.X, Is.EqualTo(x));
        Assert.That(pixel.Y, Is.EqualTo(y));
    }

    [Test]
    public void Test_Pixel_CustomToString()
    {
        StringAssert.Contains("X=", new Pixel().ToString());
    }
}
