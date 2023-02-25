namespace ScottPlot;

public interface IPalette
{
    /// <summary>
    /// All colors in this palette
    /// </summary>
    public SharedColor[] Colors { get; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Additional information such as the source of this palette
    /// </summary>
    public string Description { get; }

    // NOTE: Implementing platforms should create their own GetColor() extension method
}