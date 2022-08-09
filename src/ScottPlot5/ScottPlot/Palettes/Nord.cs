/* Sourced from NordConEmu:
 * https://github.com/arcticicestudio/nord-conemu
 * Seems to be an extended version of Aurora
 * suggested background: #2e3440
 */

namespace ScottPlot.Palettes;

public class Nord : HexPaletteBase, IPalette
{
    public override string Name => this.GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#bf616a", "#a3be8c", "#ebcb8b", "#81a1c1", "#b48ead", "#88c0d0", "#e5e9f0"
    };
}
