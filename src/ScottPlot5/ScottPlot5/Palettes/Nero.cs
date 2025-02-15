/* no info on where this palette originated */

namespace ScottPlot.Palettes;

public class Nero : IPalette
{
    public string Name { get; } = "Nero";

    public string Description { get; } = string.Empty;

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#013A20","#478C5C","#94C973","#BACC81","#CDD193"
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
