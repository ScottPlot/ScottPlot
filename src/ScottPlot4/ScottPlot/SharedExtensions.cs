using System.Drawing;
using System.Linq;

namespace ScottPlot;

internal static class SharedExtensions
{
    public static IPalette ToPalette(this ISharedPalette pal)
    {
        return new Palettes.Custom(pal.Colors.ToSDColors(), pal.Title, pal.Description);
    }

    public static Color ToSDColor(this SharedColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static Color[] ToSDColors(this SharedColor[] colors)
    {
        return colors.Select(x => x.ToSDColor()).ToArray();
    }
}
