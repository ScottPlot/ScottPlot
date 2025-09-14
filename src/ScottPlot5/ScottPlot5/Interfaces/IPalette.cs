namespace ScottPlot;

public interface IPalette
{
    /// <summary>
    /// All colors in this palette
    /// </summary>
    public Color[] Colors { get; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Additional information such as the source of this palette
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Return the Nth color (wrapping around if N is larger than the number of colors)
    /// </summary>
    public Color GetColor(int index);
}

public static class IPaletteExtensions
{
    public static Color[] GetColors(this IPalette palette, int count)
    {
        return Enumerable.Range(0, count).Select(palette.GetColor).ToArray();
    }

    public static IPalette Inverted(this IPalette palette)
    {
        return new Palettes.Custom(palette.Colors.Select(x => x.Inverted()), $"{palette.Name} (inverted)", palette.Description);
    }

    public static IPalette InvertedHue(this IPalette palette)
    {
        return new Palettes.Custom(palette.Colors.Select(x => x.InvertedHue()), $"{palette.Name} (inverted hue)", palette.Description);
    }

    public static int Count(this IPalette palette)
    {
        return palette.Colors.Length;
    }
}
