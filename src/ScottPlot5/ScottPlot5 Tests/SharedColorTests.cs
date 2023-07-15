namespace SharedTests;

internal class SharedColorTests
{
    [Test]
    public void Test_RgbColor_RgbConstructor()
    {
        ScottPlot.SharedColor color = new(33, 66, 99);
        color.R.Should().Be(33);
        color.G.Should().Be(66);
        color.B.Should().Be(99);
        color.A.Should().Be(255);
    }

    [Test]
    public void Test_RgbColor_HexConstructor()
    {
        ScottPlot.SharedColor color = ScottPlot.SharedColor.FromHex("#336699");
        color.R.Should().Be(51);
        color.G.Should().Be(102);
        color.B.Should().Be(153);
        color.A.Should().Be(255);
    }

    [Test]
    public void Test_RgbColor_HexConstructorWithAlpha()
    {
        ScottPlot.SharedColor color = ScottPlot.SharedColor.FromHex("#336699AA");
        color.R.Should().Be(51);
        color.G.Should().Be(102);
        color.B.Should().Be(153);
        color.A.Should().Be(170);
    }
}
