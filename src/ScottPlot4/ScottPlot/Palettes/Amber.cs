/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */

namespace ScottPlot.Palettes;

public class Amber : HexPaletteBase
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#FF6F00","#FF8F00","#FFA000","#FFB300","#FFC107"
    };
}
