using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class TopAxis : AxisBase, IAxis
{
    public TopAxis(string text = "")
    {
        Label.Text = text;
        Edge = Edge.Top;
        Orientation = Orientation.Horizontal;
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        Label.Draw(canvas, info.DataRect.HorizontalCenter, 0, HorizontalAlignment.Center, VerticalAlignment.Top);
    }
}