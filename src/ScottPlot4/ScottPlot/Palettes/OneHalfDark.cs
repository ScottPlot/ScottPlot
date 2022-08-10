/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */

namespace ScottPlot.Palettes;

public class OneHalfDark : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#e06c75", "#98c379", "#e5c07b", "#61aff0", "#c678dd", "#56b6c2", "#dcdfe4"
    };
}
