using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public abstract class AxisBase
{
    public Edge Edge { get; protected set; }
    public Orientation Orientation { get; protected set; }
    public TextLabel Label { get; } = new();
}