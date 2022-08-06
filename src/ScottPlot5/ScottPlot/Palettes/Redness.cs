/* Sourced from Color Hex:
 * https://www.color-hex.com/
 * https://www.color-hex.com/color-palette/616
 */

namespace ScottPlot.Palettes
{
    public class Redness : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Redness.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
