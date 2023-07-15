/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */

namespace ScottPlot.Palettes;

public class OneHalfDark : IPalette
{
    public string Name { get; } = "One Half (Dark)";

    public string Description { get; } = "A Sublime color scheme by Son A. Pham: https://github.com/sonph/onehalf";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#e06c75", "#98c379", "#e5c07b", "#61aff0", "#c678dd", "#56b6c2", "#dcdfe4"
    };
}
