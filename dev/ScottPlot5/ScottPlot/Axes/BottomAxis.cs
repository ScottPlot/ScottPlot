using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class BottomAxis : AxisBase, IAxis
{
    public BottomAxis(string text = "")
    {
        Label.Text = text;
        Edge = Edge.Bottom;
        Orientation = Orientation.Horizontal;
    }

    public void Draw(ICanvas canvas, PlotConfig info)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yBottom = info.Height;
        Label.Draw(canvas, xCenter, yBottom, HorizontalAlignment.Center, VerticalAlignment.Bottom);
    }
}
