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

    // TODO: THIS
    //public Color GetColor(int index);
}
