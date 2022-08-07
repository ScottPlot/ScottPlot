/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * It is is both LCD and print friendly but not blind nor photocopy friendly
 */
namespace ScottPlot.Palettes
{
    public class Dark : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Dark.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
