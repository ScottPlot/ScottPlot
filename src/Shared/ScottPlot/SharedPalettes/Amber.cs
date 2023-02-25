/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */

namespace ScottPlot.SharedPalettes;

public class Amber : ISharedPalette
{
    public string Title { get; } = "Amber";

    public string Description { get; } = string.Empty;

    public SharedColor[] Colors { get; } = SharedColor.FromHex(hexColors);

    private static readonly string[] hexColors =
    {
        "#FF6F00", "#FF8F00", "#FFA000", "#FFB300", "#FFC107"
    };
}