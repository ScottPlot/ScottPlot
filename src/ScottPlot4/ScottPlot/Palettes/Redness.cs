/* Sourced from Color Hex:
 * https://www.color-hex.com/
 * https://www.color-hex.com/color-palette/616
 */

namespace ScottPlot.Palettes;

public class Redness : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#FF0000", "#FF4F00", "#FFA900", "#900303", "#FF8181"
    };
}

