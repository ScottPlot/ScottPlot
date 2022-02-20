using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class LeftAxis : AxisBase, IAxis
{
    public LeftAxis(string text = "")
    {
        Label.Text = text;
        Edge = Edge.Left;
        Orientation = Orientation.Vertical;
    }

    public PixelSize GetSize(ICanvas canvas)
    {
        float width = MeasureWidth(canvas);
        return new PixelSize(width, float.NaN);
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        float xLeft = 0;
        float yCenter = info.DataRect.VerticalCenter;
        Label.Draw(canvas, xLeft, yCenter, -90);
    }
}
