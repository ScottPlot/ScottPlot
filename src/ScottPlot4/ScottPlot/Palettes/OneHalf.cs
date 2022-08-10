/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */

namespace ScottPlot.Palettes;

public class OneHalf : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#383a42", "#e4564a", "#50a14f", "#c18402", "#0084bc", "#a626a4", "#0897b3"
    };
}
