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

    public void Draw(ICanvas canvas, PlotConfig info)
    {
        float xRight = info.FigureRect.Right - Label.Measure(canvas).Height * 2;
        float yCenter = info.DataRect.VerticalCenter;
        Label.Draw(canvas, xRight, yCenter, 90);
    }
}