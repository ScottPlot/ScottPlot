namespace ScottPlot.Palettes;

public class Palette : IPalette
{
    public virtual string Name { get; }

    public virtual string Description { get; }

    public Color[] Colors { get; protected set; }

    public Palette()
    {
        Colors = Array.Empty<Color>();
        Name = string.Empty;
        Description = string.Empty;
    }

    public Palette(Color[] colors, string name, string description)
    {
        Colors = colors;
        Name = name;
        Description = description;
    }

    public Palette(string[] colors, string name, string description)
    {
        Colors = Color.FromHex(colors);
        Name = name;
        Description = description;
    }

    public int Length => Colors.Length;

    public IEnumerator<Color> GetEnumerator() => ((IEnumerable<Color>)Colors).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Colors.GetEnumerator();

    public Color GetColor(int index) => Colors[index % Colors.Length];

    public Color[] FromHexColors(string[] hexColors) => Color.FromHex(hexColors);
}
