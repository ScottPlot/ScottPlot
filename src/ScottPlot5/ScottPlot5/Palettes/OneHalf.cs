/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */

namespace ScottPlot.Palettes;

public class OneHalf : IPalette
{
    public string Name { get; } = "One Half";

    public string Description { get; } = "A Sublime color scheme " +
        "by Son A. Pham: https://github.com/sonph/onehalf";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#383a42", "#e4564a", "#50a14f", "#c18402", "#0084bc", "#a626a4", "#0897b3"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
