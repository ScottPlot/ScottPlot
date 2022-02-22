using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class RightAxis : AxisBase, IAxis
{
    public RightAxis(string text, bool ticks) : base(Edge.Right, text)
    {
        Ticks(ticks);
    }

    public void Draw(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        float xRight = info.FigureRect.Right - Label.Measure(canvas).Height * 2;
        float yCenter = info.DataRect.VerticalCenter;
        Label.Draw(canvas, xRight, yCenter, 90);

        foreach (Tick tick in ticks)
        {
            // TODO: move position logic here
            tick.DrawTickAndLabel(canvas, info);
        }
    }
}