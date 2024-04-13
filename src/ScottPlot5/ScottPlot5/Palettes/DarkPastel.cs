/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * This palette is the lighter-color version of the 'Dark' palette.
 */

namespace ScottPlot.Palettes;

public class DarkPastel : IPalette
{
    public string Name { get; } = "Dark Pastel";

    public string Description { get; } = "A qualitative 8-color palette generated using colorbrewer2.org";

    public Color[] Colors { get; } = Color.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#66c2a5", "#fc8d62", "#8da0cb", "#e78ac3", "#a6d854",
        "#ffd92f", "#e5c494", "#b3b3b3",
    };

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
