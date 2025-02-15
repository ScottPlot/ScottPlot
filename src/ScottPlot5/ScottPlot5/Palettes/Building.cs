/* Sourced from RefactoringUI - Building Your Color Palette:
 * https://www.refactoringui.com/previews/building-your-color-palette
 */

namespace ScottPlot.Palettes;

public class Building : IPalette
{
    public string Name { get; } = "Building";

    public string Description { get; } = string.Empty;

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#FF6F00","#FF8F00","#FFA000","#FFB300","#FFC107"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
