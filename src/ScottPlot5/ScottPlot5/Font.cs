namespace ScottPlot;

/// <summary>
/// Represents a font which may or may not exist on the system
/// </summary>
public readonly struct Font
{
    public readonly string Name;

    public readonly float Size;

    public Font(string name, float size)
    {
        Name = name;
        Size = size;
    }
}
