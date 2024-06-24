namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class PaletteTests
{
    [Test]
    public void Test_GetPalette_ReturnsPalettes()
    {
        IPalette[] palettes = ScottPlot.Palette.GetPalettes();
        palettes.Should().NotBeNullOrEmpty();
        Console.WriteLine("Palettes: " + string.Join(", ", palettes.Select(x => x.ToString())));
    }

    [Test]
    public void Test_Custom_Palette()
    {
        string[] customColors = { "#019d9f", "#7d3091", "#57e075", "#e5b5fa", "#009118" };
        var pal = ScottPlot.Palette.FromColors(customColors);
        pal.Colors.Length.Should().Be(customColors.Length);
    }
}
