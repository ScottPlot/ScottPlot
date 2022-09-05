namespace ScottPlot_Tests.UnitTests;

internal class CoordinateRectTests
{
    [Test]
    public void Test_CoordinateRect_Defaults()
    {
        CoordinateRect cRect = new();
        cRect.XMin.Should().Be(0);
        cRect.XMax.Should().Be(0);
        cRect.YMin.Should().Be(0);
        cRect.YMax.Should().Be(0);
        cRect.HasArea.Should().BeFalse();
    }

    [Test]
    public void Test_CoordinateRect_Constructor()
    {
        CoordinateRect cRect = new(-3, 7, -13, 11);
        cRect.XMin.Should().Be(-3);
        cRect.XMax.Should().Be(7);
        cRect.YMin.Should().Be(-13);
        cRect.YMax.Should().Be(11);
        cRect.Width.Should().Be(10);
        cRect.Height.Should().Be(24);
        cRect.HasArea.Should().BeTrue();
    }

    [Test]
    public void Test_Coordinate_CustomToString()
    {
        new CoordinateRect().ToString().Should().Contain("XMin=");
    }
}
