/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class PolarNight : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#2E3440", "#3B4252", "#434C5E", "#4C566A",
    };
}
