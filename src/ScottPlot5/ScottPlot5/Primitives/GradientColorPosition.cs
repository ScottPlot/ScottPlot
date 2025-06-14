namespace ScottPlot.Primitives;

/// <summary>
/// Defines position of a color in a gradient as a fractional position in the range 0..1
/// </summary>
public class GradientColorPosition(Color color, double position)
{
    public Color Color { get; set; } = color;
    public double Position { get; set; } = position;
}
