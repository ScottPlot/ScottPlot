namespace ScottPlot.Palettes;

// ScottPlot4
public abstract class HexPaletteBase : PaletteBase
{
    internal abstract string[] HexColors { get; }

    public HexPaletteBase()
    {
        Colors = FromHexColors(HexColors);
    }
}
