/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Aurora : IPalette
{
    public string Name { get; } = "Aurora";

    public string Description { get; } = "From the Nord " +
        "collection of palettes: https://github.com/arcticicestudio/nord";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#BF616A", "#D08770", "#EBCB8B", "#A3BE8C", "#B48EAD",
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
