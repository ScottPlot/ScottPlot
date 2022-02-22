using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class BottomAxis : AxisBase, IAxis
{
    public BottomAxis(string text, bool ticks) : base(Edge.Bottom, text)
    {
        Ticks(ticks);
    }

    public void Draw(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yBottom = info.Height;
        Label.Draw(canvas, xCenter, yBottom, HorizontalAlignment.Center, VerticalAlignment.Bottom);

        foreach (Tick tick in ticks)
        {
            // TODO: move position logic here
            tick.DrawTickAndLabel(canvas, info);
        }
    }
}
