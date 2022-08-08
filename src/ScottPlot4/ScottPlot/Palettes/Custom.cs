using System;

namespace ScottPlot.Palettes;

public class Custom : HexPaletteBase, IPalette
{
    internal override string[] HexColors { get; }

    public Custom(string[] hexColors)
    {
        if (hexColors is null)
            throw new ArgumentNullException("must provide at least one color");

        if (hexColors.Length == 0)
            throw new ArgumentException("must provide at least one color");

        HexColors = hexColors;
    }
}
