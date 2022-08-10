/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Frost : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    internal override string[] HexColors => new string[]
    {
        "#8FBCBB", "#88C0D0", "#81A1C1", "#5E81AC",
    };
}
