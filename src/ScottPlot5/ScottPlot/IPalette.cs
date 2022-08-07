namespace ScottPlot;

/// <summary>
/// A palette is a list of colors
/// </summary>
public interface IPalette : IReadOnlyList<Color>
{
    /// <summary>
    /// Return the specified color in the palette (with rollover)
    /// </summary>
    Color GetColor(int index);
}
