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

    public System.Drawing.Color[] Colors { get; } = HexColors.Select(System.Drawing.ColorTranslator.FromHtml).ToArray();

    private static readonly string[] HexColors =
    {
        "#BF616A", "#D08770", "#EBCB8B", "#A3BE8C", "#B48EAD",
    };
}
