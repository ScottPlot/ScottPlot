namespace ScottPlot;

/// <summary>
/// Classes with a <see cref="LineStyle"/> can implement this
/// to guide addition of standard shortcuts to its most commonly used properties.
/// </summary>
public interface IHasLine
{
    public LineStyle LineStyle { get; set; }

    public float LineWidth { get; set; }
    public LinePattern LinePattern { get; set; }
    public Color LineColor { get; set; }
}
