namespace ScottPlot.Palettes;

public abstract class HexPaletteBase : PaletteBase, IPalette
{
    internal abstract string[] HexColors { get; }

    public override Color[] Colors { get; }

    public HexPaletteBase()
    {
        Colors = Color.FromHex(HexColors);
    }
}
