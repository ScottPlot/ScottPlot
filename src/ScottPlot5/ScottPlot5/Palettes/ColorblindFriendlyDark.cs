/* This color palette is based on the ColorBlindFriendly pallette
 * sourced from the examples provided in:
 * Wong 2011, https://www.nature.com/articles/nmeth.1618.pdf
 * This 8-color palette has good overall variability and can be
 * differentiated by individuals with red-green color blindness.
 *
 * This version simply inverts the colors for use on dark backgrounds.
 */

namespace ScottPlot.Palettes;

public class ColorblindFriendlyDark : IPalette
{
    private static readonly string[] HexColors =
    {
        "#FFFFFF", "#1960FF", "#A94B16", "#FF618C", "#0F1BBD", "#FF8D4D", "#2AA1FF", "#338858"
    };

    public string Name { get; } = "Colorblind Friendly - Dark";

    public string Description { get; } =
        "A set of 8 colorblind-friendly colors with colors inverted for dark backgrounds.";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
