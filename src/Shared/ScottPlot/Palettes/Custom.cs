namespace ScottPlot.Palettes;

internal class Custom : IPalette
{
    public SharedColor[] Colors { get; }

    public string Name { get; }

    public string Description { get; }

    public Custom(SharedColor[] colors, string name, string description)
    {
        Colors = colors;
        Name = name;
        Description = description;
    }

    public Custom(string[] hex, string name, string description)
    {
        Colors = SharedColor.FromHex(hex);
        Name = name;
        Description = description;
    }
}
