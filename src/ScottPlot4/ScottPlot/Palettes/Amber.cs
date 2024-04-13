/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */

namespace ScottPlot.Palettes;

public class Amber : IPalette
{
    public string Name { get; } = "Amber";

    public string Description { get; } = string.Empty;

    public System.Drawing.Color[] Colors { get; } = HexColors.Select(System.Drawing.ColorTranslator.FromHtml).ToArray();

    private static readonly string[] HexColors =
    {
        "#FF6F00", "#FF8F00", "#FFA000", "#FFB300", "#FFC107"
    };
}
