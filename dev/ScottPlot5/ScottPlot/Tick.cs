using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot;

public class Tick
{
    public readonly Edge Edge = Edge.Bottom;
    public readonly double Position;
    public DateTime DateTime => DateTime.FromOADate(Position);

    public bool IsMajor = false;
    public string Label = string.Empty;
    public Color Color = Colors.Black;
    public float TickLength = 5;
    public float TextPadding = 3;

    public Tick(double position, Edge edge)
    {
        Position = position;
        Edge = edge;
    }

    public Tick(DateTime position, Edge edge)
    {
        Position = position.ToOADate();
        Edge = edge;
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        if (Edge == Edge.Bottom)
            DrawBottom(canvas, info);
        else if (Edge == Edge.Left)
            DrawLeft(canvas, info);
        else
            throw new NotImplementedException($"Unsupported {nameof(Edge)}: {Edge}");
    }

    private void DrawBottom(ICanvas canvas, PlotInfo info)
    {
        PointF pt1 = new(info.GetPixelX(Position), info.DataRect.Bottom);
        PointF pt2 = new(pt1.X, pt1.Y + TickLength);
        PointF pt3 = new(pt2.X, pt2.Y + TextPadding);

        // TODO: MEASURE STRING AFTER MAUI GRAPHICS SUPPORTS IT
        canvas.DrawLine(pt1, pt2);
        canvas.FontColor = Color;
        canvas.FontSize = 12;
        canvas.DrawString(Label, pt3.X, pt3.Y + 10, HorizontalAlignment.Center);
    }

    private void DrawLeft(ICanvas canvas, PlotInfo info)
    {
        PointF pt1 = new(info.DataRect.Left, info.GetPixelY(Position));
        PointF pt2 = new(pt1.X - TickLength, pt1.Y);
        PointF pt3 = new(pt2.X - TextPadding, pt2.Y);

        // TODO: MEASURE STRING AFTER MAUI GRAPHICS SUPPORTS IT
        canvas.DrawLine(pt1, pt2);
        canvas.FontColor = Color;
        canvas.FontSize = 12;
        canvas.DrawString(Label, pt3.X, pt3.Y + 4, HorizontalAlignment.Right);
    }
}
