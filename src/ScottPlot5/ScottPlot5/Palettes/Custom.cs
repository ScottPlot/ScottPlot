namespace ScottPlot.Palettes;

internal class Custom : IPalette
{
    public Color[] Colors { get; }

    public string Name { get; }

    public string Description { get; }

    public Custom(Color[] colors, string name, string description)
    {
        Colors = colors;
        Name = name;
        Description = description;
    }

    public Custom(string[] hex, string name, string description)
    {
        Colors = Color.FromHex(hex);
        Name = name;
        Description = description;
    }

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
