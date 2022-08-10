/* no info on where this palette originated */

namespace ScottPlot.Palettes;

public class Nero : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#013A20","#478C5C","#94C973","#BACC81","#CDD193"
    };
}
