namespace ScottPlot.Palettes;

public abstract class HexPaletteBase : PaletteBase, IPalette
{
    internal abstract string[] HexColors { get; }

    public HexPaletteBase()
    {
        if (HexColors is null)
            throw new System.NullReferenceException($"{nameof(HexColors)} must be populated before the constructor is called");

        Colors = FromHexColors(HexColors);
    }
}
