namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class PixelRectTests
{
    [Test]
    public void Test_PixelRect_Defaults()
    {
        PixelRect pxRect = new();
        pxRect.Left.Should().Be(0);
        pxRect.Right.Should().Be(0);
        pxRect.Bottom.Should().Be(0);
        pxRect.Top.Should().Be(0);
        pxRect.HasArea.Should().BeFalse();
    }

    [Test]
    public void Test_PixelRect_Constructor()
    {
        PixelRect pxRect = new(-3, 7, 123, 11);
        pxRect.Left.Should().Be(-3);
        pxRect.Right.Should().Be(7);
        pxRect.Bottom.Should().Be(123);
        pxRect.Top.Should().Be(11);
        pxRect.Width.Should().Be(10);
        pxRect.Height.Should().Be(123 - 11);

        pxRect.HasArea.Should().BeTrue();
    }

    [Test]
    public void Test_Pixel_CustomToString()
    {
        new PixelRect().ToString().Should().Contain("Left=");
    }
}
