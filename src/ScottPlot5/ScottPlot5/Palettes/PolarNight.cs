/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */

namespace ScottPlot.Palettes;

public class PolarNight : IPalette
{
    public string Name { get; } = "Polar Night";

    public string Description { get; } = "From the " +
        "Nord collection of palettes: https://github.com/arcticicestudio/nord";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#2E3440", "#3B4252", "#434C5E", "#4C566A",
    };
}
