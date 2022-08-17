/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class PolarNight : HexPaletteBase, IPalette
{
    public override string Name => "Polar Night";

    public override string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    internal override string[] HexColors => new string[]
    {
        "#2E3440", "#3B4252", "#434C5E", "#4C566A",
    };
}
