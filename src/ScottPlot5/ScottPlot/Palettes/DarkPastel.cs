/* This a qualitative 8-color palette generated using https://colorbrewer2.org
 * © Cynthia Brewer, Mark Harrower and The Pennsylvania State University
 * This palette is the lighter-color version of the 'Dark' palette.
 */
namespace ScottPlot.Palettes
{
    public class DarkPastel : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.DarkPastel.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
