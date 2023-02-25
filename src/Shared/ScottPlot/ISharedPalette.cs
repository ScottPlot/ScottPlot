namespace ScottPlot;

internal interface ISharedPalette
{
    /// <summary>
    /// All colors in this palette
    /// </summary>
    public SharedColor[] Colors { get; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Additional information such as the source of this palette
    /// </summary>
    public string Description { get; }
}