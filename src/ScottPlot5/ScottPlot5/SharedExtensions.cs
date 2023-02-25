namespace ScottPlot;

public static class SharedExtensions
{
    public static Color Convert(this SharedColor color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }
    public static SharedColor Convert(this Color color)
    {
        return new SharedColor(color.Red, color.Green, color.Blue, color.Alpha);
    }

    public static Color[] Convert(this SharedColor[] colors)
    {
        return colors.Select(x => x.Convert()).ToArray();
    }

    public static SharedColor[] Convert(this Color[] colors)
    {
        return colors.Select(x => x.Convert()).ToArray();
    }

    public static Color GetColor(this IPalette pal, int index)
    {
        int colorIndex = index % pal.Colors.Length;
        SharedColor color = pal.Colors[colorIndex];
        return color.Convert();
    }
}
