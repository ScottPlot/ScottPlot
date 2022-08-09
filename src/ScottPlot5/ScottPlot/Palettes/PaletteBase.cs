namespace ScottPlot.Palettes;

public abstract class PaletteBase : IPalette
{
    public abstract Color[] Colors { get; }

    public abstract string Name { get; }

    public abstract string Description { get; }

    public int Length => Colors.Length;

    public IEnumerator<Color> GetEnumerator() => ((IEnumerable<Color>)Colors).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Colors.GetEnumerator();

    public Color GetColor(int index)
    {
        return Colors[index % Colors.Length];
    }
}
