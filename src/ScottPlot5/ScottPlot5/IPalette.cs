namespace ScottPlot;

/// <summary>
/// A palette is a list of colors
/// </summary>
public interface IPalette : IEnumerable<Color>
{
    public Color[] Colors { get; }

    /// <summary>
    /// Return the specified color in the palette (with rollover)
    /// </summary>
    Color GetColor(int index);

    public int Length { get; }

    string Name { get; }

    string Description { get; }
}
