namespace ScottPlot.Palettes;

internal class Palette : PaletteBase, IPalette
{
    public override string Name { get; }

    public override string Description { get; }

    public override Color[] Colors { get; }

    public Palette(Color[] colors, string name, string description)
    {
        Colors = colors;
        Name = name;
        Description = description;
    }

    public Palette(string[] hexColors, string name, string description)
    {
        Colors = Color.FromHex(hexColors);
        Name = name;
        Description = description;
    }
}
