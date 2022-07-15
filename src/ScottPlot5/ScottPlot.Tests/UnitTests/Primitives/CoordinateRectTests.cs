namespace ScottPlot.Tests.UnitTests.Primitives;

internal class CoordinateRectTests
{
    [Test]
    public void Test_CoordinateRect_Defaults()
    {
        CoordinateRect cRect = new();
        Assert.That(cRect.XMin, Is.Zero);
        Assert.That(cRect.XMax, Is.Zero);
        Assert.That(cRect.YMin, Is.Zero);
        Assert.That(cRect.YMax, Is.Zero);
        Assert.That(cRect.HasArea, Is.False);
    }

    [Test]
    public void Test_CoordinateRect_Constructor()
    {
        CoordinateRect cRect = new(-3, 7, -13, 11);
        Assert.That(cRect.XMin, Is.EqualTo(-3));
        Assert.That(cRect.XMax, Is.EqualTo(7));
        Assert.That(cRect.YMin, Is.EqualTo(-13));
        Assert.That(cRect.YMax, Is.EqualTo(11));
        Assert.That(cRect.Width, Is.EqualTo(10));
        Assert.That(cRect.Height, Is.EqualTo(24));
        Assert.That(cRect.HasArea, Is.True);
    }

    [Test]
    public void Test_Coordinate_CustomToString()
    {
        StringAssert.Contains("XMin=", new CoordinateRect().ToString());
    }
}
