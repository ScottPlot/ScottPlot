using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public abstract class VerticalAxis : AxisBase
{
    public float MeasureWidth(ICanvas canvas)
    {
        float labelHeight = Label.Measure(canvas).Height;
        float width = labelHeight * 2;
        return width;
    }
}
