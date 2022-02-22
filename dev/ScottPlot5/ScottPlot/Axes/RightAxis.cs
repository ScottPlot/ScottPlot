using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public class RightAxis : AxisBase, IAxis
{
    public RightAxis(string text, bool ticks) : base(Edge.Right, text)
    {
        Ticks(ticks);
    }

    public void DrawAxisLabel(ICanvas canvas, PlotConfig info, float size, float offset)
    {
        float xRight = info.DataRect.Right + size + offset;
        float yCenter = info.DataRect.VerticalCenter;
        Label.Draw(canvas, xRight, yCenter, 90, invert: true);
    }

    public void DrawGridLines(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        DrawHorizontalGridLines(canvas, info, ticks);
    }

    public void DrawSpine(ICanvas canvas, PlotConfig config, float offset)
    {
        canvas.StrokeColor = SpineColor;
        canvas.StrokeSize = SpineLineWidth;
        canvas.DrawLine(
            config.DataRect.Right + offset,
            config.DataRect.Top,
            config.DataRect.Right + offset,
            config.DataRect.Bottom);
    }

    public void DrawTicks(ICanvas canvas, PlotConfig info, Tick[] ticks, float offset)
    {
        foreach (Tick tick in ticks)
        {
            float y = info.GetPixelY(tick.Position);

            PointF pt1 = new(info.DataRect.Right + offset, y);
            PointF pt2 = new(pt1.X + tick.TickMarkLength, pt1.Y);
            canvas.StrokeColor = tick.TickMarkColor;
            canvas.DrawLine(pt1, pt2);

            PointF pt3 = new(pt2.X + tick.TextPadding, pt2.Y);
            tick.Label.Draw(canvas, pt3.X, pt3.Y, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
    }
}