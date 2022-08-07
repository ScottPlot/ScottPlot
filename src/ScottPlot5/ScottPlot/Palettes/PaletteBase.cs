namespace ScottPlot.Palettes;

public abstract class PaletteBase : IPalette, IReadOnlyList<Color>
{
    protected abstract Color[] colors { get; }

    public Color this[int index] => colors[index];

    public Color GetColor(int index) => colors[index % colors.Length];

    public int Count => colors.Length;

    public IEnumerator<Color> GetEnumerator() => ((IEnumerable<Color>)colors).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => colors.GetEnumerator();
}
