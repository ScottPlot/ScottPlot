namespace ScottPlot.Tests.UnitTests.Primitives;

internal class PixelRectTests
{
    [Test]
    public void Test_PixelRect_Defaults()
    {
        PixelRect pxRect = new();
        Assert.That(pxRect.Left, Is.Zero);
        Assert.That(pxRect.Right, Is.Zero);
        Assert.That(pxRect.Bottom, Is.Zero);
        Assert.That(pxRect.Top, Is.Zero);
        Assert.That(pxRect.HasArea, Is.False);
    }

    [Test]
    public void Test_PixelRect_Constructor()
    {
        PixelRect pxRect = new(-3, 7, 123, 11);
        Assert.That(pxRect.Left, Is.EqualTo(-3));
        Assert.That(pxRect.Right, Is.EqualTo(7));
        Assert.That(pxRect.Bottom, Is.EqualTo(123));
        Assert.That(pxRect.Top, Is.EqualTo(11));
        Assert.That(pxRect.Width, Is.EqualTo(10));
        Assert.That(pxRect.Height, Is.EqualTo(123 - 11));
        Assert.That(pxRect.HasArea, Is.True);
    }

    [Test]
    public void Test_Pixel_CustomToString()
    {
        StringAssert.Contains("Left=", new PixelRect().ToString());
    }
}
