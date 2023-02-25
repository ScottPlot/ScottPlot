/* Sourced from NordConEmu:
 * https://github.com/arcticicestudio/nord-conemu
 * Seems to be an extended version of Aurora
 * suggested background: #2e3440
 */

namespace ScottPlot.Palettes;

public class Nord : IPalette
{
    public string Name { get; } = "Nord";

    public string Description => "From the Nord " +
        "ConEmu color scheme: https://github.com/arcticicestudio/nord-conemu";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#bf616a", "#a3be8c", "#ebcb8b", "#81a1c1", "#b48ead", "#88c0d0", "#e5e9f0"
    };
}
