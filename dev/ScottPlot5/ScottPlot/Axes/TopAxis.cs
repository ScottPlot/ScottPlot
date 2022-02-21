using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class TopAxis : AxisBase, IAxis
{
    public TopAxis(string text, bool ticks) : base(Edge.Top, text)
    {
        Ticks(ticks);
    }

    public void Draw(ICanvas canvas, PlotConfig info)
    {
        Label.Draw(canvas, info.DataRect.HorizontalCenter, 0, HorizontalAlignment.Center, VerticalAlignment.Top);
    }
}