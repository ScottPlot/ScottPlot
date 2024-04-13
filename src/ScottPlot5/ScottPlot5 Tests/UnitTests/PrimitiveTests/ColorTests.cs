namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class ColorTests
{
    [Test]
    public void Test_Color_Constructor()
    {
        Color color = new(13, 17, 23);
        color.Red.Should().Be(13);
        color.Green.Should().Be(17);
        color.Blue.Should().Be(23);
        color.Alpha.Should().Be(255);
    }

    [Test]
    public void Test_Color_ConstructorWithAlpha()
    {
        Color color = new(13, 17, 23, 27);
        color.Red.Should().Be(13);
        color.Green.Should().Be(17);
        color.Blue.Should().Be(23);
        color.Alpha.Should().Be(27);
    }

    [Test]
    public void Test_Color_ToARGB()
    {
        // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
        Color color = new(199, 21, 133);
        color.ARGB.Should().Be(0xFFC71585);
    }

    [Test]
    public void Test_Color_FromARGB()
    {
        // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
        Color color = Color.FromARGB(0xFFC71585);
        color.Red.Should().Be(199);
        color.Green.Should().Be(21);
        color.Blue.Should().Be(133);
        color.Alpha.Should().Be(255);
    }

    [Test]
    public void Test_Color_WithRed()
    {
        Color color = new Color(12, 34, 56, 78).WithRed(99);
        color.Red.Should().Be(99);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void Test_Color_WithGreen()
    {
        Color color = new Color(12, 34, 56, 78).WithGreen(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(99);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void Test_Color_WithBlue()
    {
        Color color = new Color(12, 34, 56, 78).WithBlue(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(99);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void Test_Color_WithAlpha()
    {
        Color color = new Color(12, 34, 56, 78).WithAlpha(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(99);
    }

    [Test]
    public void Test_Color_ToSKColor()
    {
        SKColor color = new Color(12, 34, 56, 78).ToSKColor();
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void Test_Color_ToHex()
    {
        Color color = new(12, 34, 56);
        color.ToStringRGB().Should().Be("#0C2238");
    }

    [Test]
    public void Test_Colors_ColorValues()
    {
        Colors.Orange.ToStringRGB().Should().Be("#FFA500");
        Colors.Chocolate.ToStringRGB().Should().Be("#D2691E");
        Colors.GoldenRod.ToStringRGB().Should().Be("#DAA520");

        ScottPlot.NamedColors.XkcdColors.Orange.ToStringRGB().Should().Be("#F97306");
        ScottPlot.NamedColors.XkcdColors.Darkblue.ToStringRGB().Should().Be("#030764");
        ScottPlot.NamedColors.XkcdColors.BabyPoopGreen.ToStringRGB().Should().Be("#8F9805");
    }

    [Test]
    public void Test_Colors_WebColors_HasColors()
    {
        new ScottPlot.NamedColors.WebColors().GetAllColors().Should().NotBeEmpty();
    }

    [Test]
    public void Test_Colors_WebColors_ColorValues()
    {
        ScottPlot.NamedColors.WebColors.Orange.ToStringRGB().Should().Be("#FFA500");
        ScottPlot.NamedColors.WebColors.Chocolate.ToStringRGB().Should().Be("#D2691E");
        ScottPlot.NamedColors.WebColors.GoldenRod.ToStringRGB().Should().Be("#DAA520");
    }

    [Test]
    public void Test_Colors_XKCD_HasColors()
    {
        new ScottPlot.NamedColors.XkcdColors().GetAllColors().Should().NotBeEmpty();
    }

    [Test]
    public void Test_Colors_XKCD_ColorValues()
    {
        ScottPlot.NamedColors.XkcdColors.Orange.ToStringRGB().Should().Be("#F97306");
        ScottPlot.NamedColors.XkcdColors.Darkblue.ToStringRGB().Should().Be("#030764");
        ScottPlot.NamedColors.XkcdColors.BabyPoopGreen.ToStringRGB().Should().Be("#8F9805");
    }

    [Test]
    public void Test_Colors_RandomHue()
    {
        var colors = Colors.RandomHue(10);

        var reds = colors.Select(x => (int)x.R).ToArray();
        reds.Average().Should().BeGreaterThan(0);
        reds.Average().Should().BeLessThan(255);

        var greens = colors.Select(x => (int)x.G).ToArray();
        greens.Average().Should().BeGreaterThan(0);
        greens.Average().Should().BeLessThan(255);

        var blues = colors.Select(x => (int)x.B).ToArray();
        blues.Average().Should().BeGreaterThan(0);
        blues.Average().Should().BeLessThan(255);

        var alphas = colors.Select(x => (int)x.A).ToArray();
        alphas.Average().Should().Be(255);
    }

    [Test]
    public void Test_Colors_Rainbow()
    {
        Color[] colors = Colors.RandomHue(10);
        colors.Select(x => x.ToHex()).Should().OnlyHaveUniqueItems();
    }
}
