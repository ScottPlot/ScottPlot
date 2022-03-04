using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class BottomAxis : AxisBase, IAxis
{
    public BottomAxis(string text, bool ticks) : base(Edge.Bottom, text)
    {
        Ticks(ticks);
    }

    public void DrawAxisLabel(ICanvas canvas, PlotConfig info, float size, float offset)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yBottom = info.DataRect.Bottom + size + offset;
        Label.Draw(canvas, xCenter, yBottom, HorizontalAlignment.Center, VerticalAlignment.Bottom);
    }

    public void DrawGridLines(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        DrawVerticalGridLines(canvas, info, ticks);
    }

    public void DrawSpine(ICanvas canvas, PlotConfig config, float offset)
    {
        canvas.StrokeColor = SpineColor;
        canvas.StrokeSize = SpineLineWidth;
        canvas.DrawLine(
            config.DataRect.Left,
            config.DataRect.Bottom + offset,
            config.DataRect.Right, 
            config.DataRect.Bottom + offset);
    }

    public void DrawTicks(ICanvas canvas, PlotConfig info, Tick[] ticks, float offset)
    {
        foreach (Tick tick in ticks)
        {
            float x = info.GetPixelX(tick.Position);

            PointF pt1 = new(x, info.DataRect.Bottom + offset);
            PointF pt2 = new(pt1.X, pt1.Y + tick.TickMarkLength);
            canvas.StrokeColor = tick.TickMarkColor;
            canvas.DrawLine(pt1, pt2);

            PointF pt3 = new(pt2.X, pt2.Y + tick.TextPadding);
            tick.Label.Draw(canvas, pt3.X, pt3.Y, HorizontalAlignment.Center, VerticalAlignment.Top);
        }
    }
}
