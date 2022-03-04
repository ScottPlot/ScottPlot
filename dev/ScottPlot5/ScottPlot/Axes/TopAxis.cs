using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class TopAxis : AxisBase, IAxis
{
    public TopAxis(string text, bool ticks) : base(Edge.Top, text)
    {
        Ticks(ticks);
    }

    public void DrawAxisLabel(ICanvas canvas, PlotConfig info, float size, float offset)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yTop = info.DataRect.Top - size - offset;
        Label.Draw(canvas, xCenter, yTop, HorizontalAlignment.Center, VerticalAlignment.Top);
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
            config.DataRect.Top - offset, 
            config.DataRect.Right, 
            config.DataRect.Top - offset);
    }

    public void DrawTicks(ICanvas canvas, PlotConfig info, Tick[] ticks, float offset)
    {
        foreach (Tick tick in ticks)
        {
            float x = info.GetPixelX(tick.Position);

            PointF pt1 = new(x, info.DataRect.Top - offset);
            PointF pt2 = new(pt1.X, pt1.Y - tick.TickMarkLength);
            canvas.StrokeColor = tick.TickMarkColor;
            canvas.DrawLine(pt1, pt2);

            PointF pt3 = new(pt2.X, pt2.Y - tick.TextPadding);
            tick.Label.Draw(canvas, pt3.X, pt3.Y, HorizontalAlignment.Center, VerticalAlignment.Bottom);
        }
    }
}