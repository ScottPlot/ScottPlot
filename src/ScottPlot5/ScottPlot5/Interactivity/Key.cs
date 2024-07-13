namespace ScottPlot.Interactivity;

public readonly record struct Key(string name)
{
    /// <summary>
    /// A name that uniquely identifies a specific key
    /// </summary>
    public string Name { get; } = name;
}
