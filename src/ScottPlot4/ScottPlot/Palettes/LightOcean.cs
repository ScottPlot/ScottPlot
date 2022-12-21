/* A 9-color palette by Arthurits created by a mixture of light greens, blues, and purples
 */

namespace ScottPlot.Palettes;

public class LightOcean : HexPaletteBase, IPalette
{
    public override string Name => "Light ocean";

    public override string Description => "A 9-color palette by Arthurits created by a mixture of light greens, blues, and purples";

    internal override string[] HexColors => new string[]
    {
        "#dfedd9", "#dbecdc", "#dbede4",
        "#daeeec", "#daeef3", "#dae6f2",
        "#dadef1", "#dedaee", "#e5daed"
    };
}
