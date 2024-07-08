namespace ScottPlot.Interactivity.Keys;

public readonly struct CustomKey : IKey
{
    public required string Name { get; init; }
    public override string ToString() => Name;
}
