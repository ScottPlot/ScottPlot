/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */

namespace ScottPlot.Palettes;

public class Amber : IPalette
{
    public string Name { get; } = "Amber";

    public string Description { get; } = string.Empty;

    public Color[] Colors { get; } = Color.FromHex(hexColors);

    private static readonly string[] hexColors =
    {
        "#FF6F00", "#FF8F00", "#FFA000", "#FFB300", "#FFC107"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
