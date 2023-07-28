using System.Drawing;
using System.Linq;

namespace ScottPlot;

public static class SharedExtensions
{
    public static Color Convert(this SharedColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static Color[] Convert(this SharedColor[] colors)
    {
        return colors.Select(x => x.Convert()).ToArray();
    }

    public static SharedColor Convert(this Color color)
    {
        return new SharedColor(color.R, color.G, color.B, color.A);
    }

    public static SharedColor[] Convert(this Color[] colors)
    {
        return colors.Select(x => x.Convert()).ToArray();
    }

    public static Color GetColor(this IPalette pal, int index)
    {
        int colorIndex = index % pal.Colors.Length;
        Color color = pal.Colors[colorIndex];
        return color;
    }

    public static Color GetColor(this IPalette pal, int index, double alpha)
    {
        int colorIndex = index % pal.Colors.Length;
        Color color = pal.Colors[colorIndex];
        return Color.FromArgb((byte)(alpha * 255), color.R, color.G, color.B);
    }

    public static int Count(this IPalette pal)
    {
        return pal.Colors.Length;
    }

    public static Color[] GetColors(this IPalette pal, int count, int offset = 0, double alpha = 1)
    {
        return Enumerable.Range(offset, count)
            .Select(x => pal.GetColor(x, alpha))
            .ToArray();
    }
}
