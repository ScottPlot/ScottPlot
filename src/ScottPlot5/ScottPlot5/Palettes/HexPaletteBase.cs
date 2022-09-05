namespace ScottPlot.Palettes;

// ScottPlot5
public abstract class HexPaletteBase : Palette
{
    internal abstract string[] HexColors { get; }

    public HexPaletteBase()
    {
        Colors = Color.FromHex(HexColors);
    }
}
