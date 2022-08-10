/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * It is is both LCD and print friendly but not blind nor photocopy friendly
 */

namespace ScottPlot.Palettes;

public class Dark : HexPaletteBase, IPalette
{
    public override string Name => GetType().Name;

    public override string Description => "A qualitative 8-color palette generated using colorbrewer2.org";

    internal override string[] HexColors => new string[]
    {
        "#1b9e77","#d95f02","#7570b3","#e7298a","#66a61e",
        "#e6ab02","#a6761d","#666666",
    };
}
