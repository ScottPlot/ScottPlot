namespace ScottPlotTests.UnitTests;

public class ColorTests
{
    [Test]
    public void Test_Color_ValuesMatchKnown()
    {
        Color color = Color.FromHex("#2e5b6d");

        // https://www.rapidtables.com/convert/color/hex-to-rgb.html
        color.Red.Should().Be(46);
        color.Green.Should().Be(91);
        color.Blue.Should().Be(109);
        color.Alpha.Should().Be(255);

        color.ARGB.Should().Be(0xff2e5b6d);
    }

    [Test]
    public void Test_Color_Transparent()
    {
        // If requesting a semitransparent black, make it slightly non-black
        // to prevent SVG export from rendering the color as opaque.
        // https://github.com/ScottPlot/ScottPlot/issues/3063

        Color color = Colors.Transparent;
        color.Red.Should().Be(1);
        color.Green.Should().Be(1);
        color.Blue.Should().Be(1);
        color.Alpha.Should().Be(0);
        color.ARGB.Should().Be(0x00010101);
        color.ARGB.Should().Be(65793);
    }

    [Test]
    public void Test_Color_AlphaARGB()
    {
        Color color = Color.FromHex("#2e5b6d").WithAlpha(123);
        color.Red.Should().Be(46);
        color.Green.Should().Be(91);
        color.Blue.Should().Be(109);
        color.Alpha.Should().Be(123);
        color.ARGB.Should().Be(0x7B2E5B6D);
    }
}
