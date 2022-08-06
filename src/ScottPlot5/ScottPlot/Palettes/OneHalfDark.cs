/* Sourced from Son A. Pham's Sublime color scheme by the same name
 * https://github.com/sonph/onehalf
 */
namespace ScottPlot.Palettes
{
    public class OneHalfDark : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.OneHalfDark.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
