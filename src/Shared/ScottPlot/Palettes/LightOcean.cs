/* A 9-color palette by Arthurits created by a mixture of light greens, blues, and purples
 */

namespace ScottPlot.Palettes;

public class LightOcean : IPalette
{
    public string Name { get; } = "Light ocean";

    public string Description { get; } = "A 9-color palette by Arthurits created by a mixture of light greens, blues, and purples";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#dfedd9", "#dbecdc", "#dbede4",
        "#daeeec", "#daeef3", "#dae6f2",
        "#dadef1", "#dedaee", "#e5daed"
    };
}
