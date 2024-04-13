/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class Frost : IPalette
{
    public string Name { get; } = "Frost";

    public string Description { get; } = "From the Nord " +
        "collection of palettes: https://github.com/arcticicestudio/nord";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#8FBCBB", "#88C0D0", "#81A1C1", "#5E81AC",
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
