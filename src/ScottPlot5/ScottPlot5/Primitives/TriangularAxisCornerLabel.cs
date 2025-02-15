namespace ScottPlot;

/// <summary>
/// Represents a label at a corner of the triangular plot triangle and includes styling information.
/// </summary>
public class TriangularAxisCornerLabel
{
    public Coordinates Position { get; set; }

    public LabelStyle LabelStyle { get; } = new()
    {
        Alignment = Alignment.MiddleCenter,
    };

    public string? LabelText { get; set; } = null;

    public TriangularAxisCornerLabel(Coordinates position, string labelText)
    {
        Position = position;
        LabelText = labelText;
    }
}
