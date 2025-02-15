namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateRectTests
{
    [Test]
    public void Test_CoordinateRect_Defaults()
    {
        CoordinateRect cRect = new();
        cRect.Left.Should().Be(0);
        cRect.Right.Should().Be(0);
        cRect.Bottom.Should().Be(0);
        cRect.Top.Should().Be(0);
        cRect.HasArea.Should().BeFalse();
    }

    [Test]
    public void Test_CoordinateRect_Constructor()
    {
        CoordinateRect cRect = new(-3, 7, -13, 11);
        cRect.Left.Should().Be(-3);
        cRect.Right.Should().Be(7);
        cRect.Bottom.Should().Be(-13);
        cRect.Top.Should().Be(11);
        cRect.Width.Should().Be(10);
        cRect.Height.Should().Be(24);
        cRect.HasArea.Should().BeTrue();
    }


    [Test]
    public void Test_CoordinateRect_Expanded()
    {
        Coordinates coord = new(13, 42);

        CoordinateRect cRect = new(-10, 10, -10, 10);
        cRect.Contains(coord).Should().BeFalse();

        CoordinateRect cRect2 = cRect.Expanded(coord);
        cRect2.Contains(coord).Should().BeTrue();
    }

    [TestCase(0, 0, 1, 1, 0.5, 0.5, true, TestName = "Test Contains with no inverted")]
    [TestCase(0, 0, 1, -1, 0.5, -0.5, true, TestName = "Test Contains with inverted X")]
    [TestCase(0, 0, -1, 1, -0.5, 0.5, true, TestName = "Test Contains with inverted Y")]
    [TestCase(0, 0, -1, -1, -0.5, -0.5, true, TestName = "Test Contains with inverted X and Y")]
    public void Test_CoordinateRect_Contains(double x1,
        double y1,
        double x2,
        double y2,
        double x,
        double y,
        bool expectedContains)
    {
        CoordinateRect coordinateRect = new CoordinateRect(x1, x2, y1, y2);
        Assert.That(coordinateRect.Contains(x, y), Is.True);
    }
}
