using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class RightAxis : AxisBase, IAxis
{
    public RightAxis(string text = "")
    {
        Label.Text = text;
        Edge = Edge.Right;
        Orientation = Orientation.Vertical;
    }

    public PixelSize GetSize(ICanvas canvas)
    {
        float width = MeasureWidth(canvas);
        return new PixelSize(width, float.NaN);
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        // TODO: render right axis
    }
}