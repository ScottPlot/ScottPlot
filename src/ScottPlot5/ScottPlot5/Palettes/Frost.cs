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

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#8FBCBB", "#88C0D0", "#81A1C1", "#5E81AC",
    };
}
