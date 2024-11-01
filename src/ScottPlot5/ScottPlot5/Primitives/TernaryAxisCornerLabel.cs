namespace ScottPlot;

/// <summary>
/// Represents a label at a corner of the ternary plot triangle and includes styling information.
/// </summary>
public class TernaryAxisCornerLabel
{
    public Coordinates Position { get; set; }
    public string? LabelText { get; set; } = null;

    // Customizable offset and alignment properties
    public float OffsetX { get; set; } = 0;
    public float OffsetY { get; set; } = 0;
    public Alignment Alignment { get; set; } = Alignment.MiddleCenter;

    public LabelStyle LabelStyle { get; } = new()
    {
        Alignment = Alignment.MiddleCenter,
    };

    // Allow the user to set text size and color
    public float TextSize { get; set; } = 16f; // Default text size
    public Color TextColor { get; set; } = Colors.Black; // Default text color
    public bool IsBold { get; set; } = false; // Default to non-bold text

    public TernaryAxisCornerLabel(Coordinates position, string labelText)
    {
        Position = position;
        LabelText = labelText;
    }
}
