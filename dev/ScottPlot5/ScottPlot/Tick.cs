using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot;

public class Tick
{
    public readonly Edge Edge;
    public Orientation Orientation => (Edge == Edge.Left || Edge == Edge.Right) ? Orientation.Vertical : Orientation.Horizontal;

    public readonly double Position;
    public DateTime DateTime => DateTime.FromOADate(Position);

    public readonly TextLabel Label = new();

    public float TextPadding = 3;

    public float TickMarkLength = 5;
    public Color TickMarkColor = Colors.Black;

    public float GridLineWidth = 0;
    public Color GridLineColor = Colors.Black.WithAlpha(.2f);

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

    public PixelSize Measure(ICanvas canvas)
    {
        PixelSize labelSize = Label.Measure(canvas);

        if (Orientation is Orientation.Vertical)
        {
            return labelSize.WidenedBy(TickMarkLength + TextPadding * 4);
        }

        if (Orientation is Orientation.Horizontal)
        {
            return labelSize.HeightenedBy(TickMarkLength + TextPadding * 2);
        }

        throw new InvalidOperationException($"unsupported {Orientation.GetType()}: {Orientation}");
    }
}
