namespace ScottPlot;

/// <summary>
/// Defines position of a color in a gradient at a particular position in axis units
/// </summary>
public class AxisGradientColorPosition(Color color, double position)
{
    public Color Color { get; set; } = color;
    public double Position { get; set; } = position;
}
