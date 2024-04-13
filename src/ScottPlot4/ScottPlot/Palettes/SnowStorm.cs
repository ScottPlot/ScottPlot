/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class SnowStorm : IPalette
{
    public string Name { get; } = "Snow Storm";

    public string Description { get; } = "From the " +
        "Nord collection of palettes: https://github.com/arcticicestudio/nord";

    public System.Drawing.Color[] Colors { get; } = HexColors.Select(System.Drawing.ColorTranslator.FromHtml).ToArray();

    private static readonly string[] HexColors =
    {
        "#D8DEE9", "#E5E9F0", "#ECEFF4"
    };
}
