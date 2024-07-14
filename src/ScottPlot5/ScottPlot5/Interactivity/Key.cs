namespace ScottPlot.Interactivity;

public readonly record struct Key
{
    /// <summary>
    /// A name that uniquely identifies a specific key (cap insensitive)
    /// </summary>
    public string Name { get; }

    public Key(string name)
    {
        Name = name.ToLower();
    }
}
