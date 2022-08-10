/* Sourced from RefactoringUI - Building Your Color Palette:
 * https://www.refactoringui.com/previews/building-your-color-palette
 */

namespace ScottPlot.Palettes;

public class Building : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#FF6F00","#FF8F00","#FFA000","#FFB300","#FFC107"
    };
}
