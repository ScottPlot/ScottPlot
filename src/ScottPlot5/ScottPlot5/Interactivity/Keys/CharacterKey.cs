namespace ScottPlot.Interactivity.Keys;
public readonly struct CharacterKey : IKey
{
    public required char Character { get; init; }
    public override string ToString() => Character.ToString();
}
