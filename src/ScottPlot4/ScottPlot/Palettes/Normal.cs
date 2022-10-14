/* A color palette based on Anton Tsitsulin's 6-color palette
 * http://tsitsul.in/blog/coloropt
 * https://github.com/xgfs/coloropt
 */

namespace ScottPlot.Palettes;

public class Normal : HexPaletteBase, IPalette
{
    public override string Name => "XgfsNormal6";

    public override string Description => "A color palette adapted from Tsitsulin's 6-color normal xgfs palette: http://tsitsul.in/blog/coloropt";

    internal override string[] HexColors => new string[]
    {
        "#4053d3", "#ddb310", "#b51d14",
        "#00beff", "#fb49b0", "#00b25d", "#cacaca",
    };
}
