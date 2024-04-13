/* Sourced from Color Hex:
 * https://www.color-hex.com/
 * https://www.color-hex.com/color-palette/616
 */

namespace ScottPlot.Palettes;

public class Redness : IPalette
{
    public string Name { get; } = "Redness";

    public string Description { get; } = string.Empty;

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#FF0000", "#FF4F00", "#FFA900", "#900303", "#FF8181"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}

