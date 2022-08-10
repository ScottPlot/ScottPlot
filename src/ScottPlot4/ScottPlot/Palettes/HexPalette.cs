namespace ScottPlot.Palettes;

// ScottPlot4

/// <summary>
/// Create a palette from HTML colors (e.g., #003366)
/// </summary>
public class HexPalette : PaletteBase
{
    public HexPalette(string[] hexColors)
    {
        Colors = FromHexColors(hexColors);
    }
}
