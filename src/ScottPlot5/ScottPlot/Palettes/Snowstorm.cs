/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Snowstorm : HexPaletteBase, IPalette
{
    public override string Name => this.GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#D8DEE9", "#E5E9F0", "#ECEFF4"
    };
}
