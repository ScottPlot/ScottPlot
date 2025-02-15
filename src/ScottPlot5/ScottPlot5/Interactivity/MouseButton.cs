namespace ScottPlot.Interactivity;

/// <summary>
/// Represents a physical button on a mouse
/// </summary>
public record struct MouseButton(string name)
{
    public string Name { get; } = name;
}
