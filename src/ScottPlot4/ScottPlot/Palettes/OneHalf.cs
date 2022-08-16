/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */

namespace ScottPlot.Palettes;

public class OneHalf : HexPaletteBase, IPalette
{
    public override string Name => "One Half";

    public override string Description => "A Sublime color scheme by Son A. Pham: https://github.com/sonph/onehalf";

    internal override string[] HexColors => new string[]
    {
        "#383a42", "#e4564a", "#50a14f", "#c18402", "#0084bc", "#a626a4", "#0897b3"
    };
}
