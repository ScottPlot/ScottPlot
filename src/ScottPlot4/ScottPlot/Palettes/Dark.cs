/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * It is is both LCD and print friendly but not blind nor photocopy friendly
 */
namespace ScottPlot.Drawing.Colorsets
{
    public class Dark : HexColorset, IPalette
    {
        public override string[] hexColors => Common.HexPalettes.Dark.Colors;
    }
}
