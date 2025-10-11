namespace ScottPlot.Palettes;

public class Custom : IPalette
{
    public Color[] Colors { get; }

    public string Name { get; }

    public string Description { get; }

    public Custom(IEnumerable<Color> colors, string? name, string? description = null)
    {
        Colors = colors.ToArray();
        Name = name ?? "unnamed";
        Description = description ?? "no description";
    }

    public Custom(string[] hex, string? name = null, string? description = null) : this(Color.FromHex(hex), name, description) { }

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
