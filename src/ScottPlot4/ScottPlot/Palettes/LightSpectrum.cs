/* A 9-color palette by Arthurits created by lightening the colors in the visible spectrum
 */

namespace ScottPlot.Palettes;

public class LightSpectrum : HexPaletteBase, IPalette
{
    public override string Name => "Light spectrum";

    public override string Description => "A 9-color palette by Arthurits created by lightening the colors in the visible spectrum";

    internal override string[] HexColors => new string[]
    {
        "#fce5e6", "#fff8e7", "#fffce8",
        "#eff5e4", "#e7f2e6", "#ddf0f5",
        "#e6f2fc", "#e6eaf7", "#eee0f0"
    };
}
