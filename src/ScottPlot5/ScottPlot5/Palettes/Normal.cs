/* A color palette based on Anton Tsitsulin's 6-color palette
 * http://tsitsul.in/blog/coloropt
 * https://github.com/xgfs/coloropt
 */

namespace ScottPlot.Palettes;

public class Normal : IPalette
{
    public string Name { get; } = "Xgfs Normal 6";

    public string Description { get; } = "A color palette adapted from " +
        "Tsitsulin's 6-color normal xgfs palette: http://tsitsul.in/blog/coloropt";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#4053d3", "#ddb310", "#b51d14",
        "#00beff", "#fb49b0", "#00b25d", "#cacaca",
    };
}
