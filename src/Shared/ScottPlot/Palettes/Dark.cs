/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * It is is both LCD and print friendly but not blind nor photocopy friendly
 */

namespace ScottPlot.Palettes;

public class Dark : IPalette
{
    public string Name { get; } = "Dark";

    public string Description { get; } = "A qualitative 8-color palette generated using colorbrewer2.org";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#1b9e77","#d95f02","#7570b3","#e7298a","#66a61e",
        "#e6ab02","#a6761d","#666666",
    };
}
