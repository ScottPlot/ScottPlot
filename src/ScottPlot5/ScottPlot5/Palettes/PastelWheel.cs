/* A 12-color palette based on lighter tints of the classic color wheel 
 */

namespace ScottPlot.Palettes;

public class PastelWheel : IPalette
{
    public string Name { get; } = "Pastel wheel";

    public string Description { get; } = "A 12-color palette by Arthurits created by lightening the color wheel";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#f8c5c7", "#fadec3", "#fbf6c4",
        "#e1ecc8", "#d7e8cb", "#daebd7",
        "#d9eef3", "#cadbed", "#c7d2e6",
        "#d4d1e5", "#e8d3e6", "#f8c7de"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
