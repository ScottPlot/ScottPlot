/* A 25-color pelette based on Anton Tsitsulin's 12-color palette
 * http://tsitsul.in/blog/coloropt
 * https://github.com/xgfs/coloropt
 */
namespace ScottPlot.Palettes
{
    public class Tsitsulin : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Tsitsulin.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
