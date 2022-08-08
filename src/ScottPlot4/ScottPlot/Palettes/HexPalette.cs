namespace ScottPlot.Palettes;

/// <summary>
/// Create a palette from HTML colors (e.g., #003366)
/// </summary>
public class HexPalette : HexPaletteBase
{
    internal override string[] HexColors { get; }

    public HexPalette(string[] hexColors)
    {
        HexColors = HexColors;
    }
}
