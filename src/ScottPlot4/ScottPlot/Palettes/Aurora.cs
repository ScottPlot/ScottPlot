/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Aurora : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => "From the Nord collection of palettes: https://github.com/arcticicestudio/nord";

    internal override string[] HexColors => new string[]
    {
        "#BF616A", "#D08770", "#EBCB8B", "#A3BE8C", "#B48EAD",
    };
}
