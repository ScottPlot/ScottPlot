using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class LeftAxis : AxisBase, IAxis
{
    public LeftAxis(string text, bool ticks) : base(Edge.Left, text)
    {
        Ticks(ticks);
    }

    public void Draw(ICanvas canvas, PlotConfig info)
    {
        float xLeft = 0;
        float yCenter = info.DataRect.VerticalCenter;
        Label.Draw(canvas, xLeft, yCenter, -90);
    }
}
