namespace ScottPlot.Interactivity;

public record struct MouseButton(string name)
{
    public string Name { get; } = name;
}
