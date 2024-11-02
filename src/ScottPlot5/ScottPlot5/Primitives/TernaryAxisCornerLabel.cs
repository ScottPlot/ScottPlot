namespace ScottPlot;

/// <summary>
/// Represents a label at a corner of the ternary plot triangle and includes styling information.
/// </summary>
public class TernaryAxisCornerLabel
{
    public Coordinates Position { get; set; }

    public LabelStyle LabelStyle { get; } = new()
    {
        Alignment = Alignment.MiddleCenter,
    };
    
    public string? LabelText { get; set; } = null;

    public TernaryAxisCornerLabel(Coordinates position, string labelText)
    {
        Position = position;
        LabelText = labelText;
    }
}
