using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public abstract class HorizontalAxis : AxisBase
{
    public float MeasureHeight(ICanvas canvas)
    {
        float labelHeight = Label.Measure(canvas).Height;
        float height = labelHeight * 2;
        return height;
    }
}
