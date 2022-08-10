using System;

namespace ScottPlot.Palettes;

public class Custom : HexPaletteBase
{
    internal override string[] HexColors { get; }

    public override string Name { get; }

    public override string Description { get; }

    public Custom(string[] hexColors, string name = "", string description = "")
    {
        if (hexColors is null)
            throw new ArgumentNullException("must provide at least one color");

        if (hexColors.Length == 0)
            throw new ArgumentException("must provide at least one color");

        HexColors = hexColors;
        Name = name;
        Description = description;
    }
}
