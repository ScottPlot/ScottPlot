/* This palette can be found in several places:
 * https://www.canva.com/colors/color-palettes/summer-splash/
 * https://color.adobe.com/My-Color-Theme-color-theme-17257636/
 * "The Indian Ocean" on https://evening-ridge-43372.herokuapp.com/
 */

namespace ScottPlot.Palettes;

public class SummerSplash : HexPaletteBase, IPalette
{
    public override string Name => "Summer Splash";

    public override string Description => string.Empty;

    internal override string[] HexColors => new string[]
    {
        "#05445E", "#189AB4", "#75E6DA" , "#D4F1F4"
    };
}
