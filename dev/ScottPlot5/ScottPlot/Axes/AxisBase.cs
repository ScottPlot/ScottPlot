using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public abstract class AxisBase
{
    public Edge Edge { get; protected set; }
    public Orientation Orientation { get; protected set; }
    public TextLabel Label { get; } = new();

    public float MeasureWidth(ICanvas canvas)
    {
        float labelHeight = Label.Measure(canvas).Height;
        float width = labelHeight * 2;
        return width;
    }

    public float MeasureHeight(ICanvas canvas)
    {
        float labelHeight = Label.Measure(canvas).Height;
        float height = labelHeight * 2;
        return height;
    }
}