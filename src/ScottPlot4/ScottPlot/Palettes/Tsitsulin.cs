/* A 25-color pelette based on Anton Tsitsulin's 12-color palette
 * http://tsitsul.in/blog/coloropt
 * https://github.com/xgfs/coloropt
 */
namespace ScottPlot.Drawing.Colorsets
{
    public class Tsitsulin : HexColorset, IPalette
    {
        public override string[] hexColors => Common.HexPalettes.Tsitsulin.Colors;
    }
}
