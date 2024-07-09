namespace ScottPlot.Interactivity.Keys;
public readonly struct CharacterKey : IKey
{
    public required char Character { get; init; }

    public string Name => Character.ToString();
}
