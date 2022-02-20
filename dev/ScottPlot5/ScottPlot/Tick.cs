using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot;

public class Tick
{
    public readonly Edge Edge = Edge.Bottom;
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

        if (Edge == Edge.Left || Edge == Edge.Right)
        {
            return labelSize.WidenedBy(TickMarkLength + TextPadding);
        }
        else if (Edge == Edge.Bottom || Edge == Edge.Top)
        {
            return labelSize.HeightenedBy(TickMarkLength + TextPadding);
        }
        else
        {
            throw new InvalidOperationException($"unsupported edge: {Edge}");
        }
    }

    public void DrawGridLine(ICanvas canvas, PlotInfo info)
    {
        if (Edge == Edge.Bottom)
            DrawVerticalGridLine(canvas, info);
        else if (Edge == Edge.Left)
            DrawHorizontalGridLine(canvas, info);
        else
            throw new NotImplementedException($"Unsupported {nameof(Edge)}: {Edge}");
    }

    public void DrawTickAndLabel(ICanvas canvas, PlotInfo info)
    {
        if (Edge == Edge.Bottom)
            DrawBottomTickAndLabel(canvas, info);
        else if (Edge == Edge.Left)
            DrawLeftTickAndLabel(canvas, info);
        else
            throw new NotImplementedException($"Unsupported {nameof(Edge)}: {Edge}");
    }

    private void DrawVerticalGridLine(ICanvas canvas, PlotInfo info)
    {
        // TODO: reduce duplication
        float x = info.GetPixelX(Position);

        PointF ptGridTop = new(x, info.DataRect.Top);
        PointF ptGridBottom = new(x, info.DataRect.Bottom);
        canvas.StrokeSize = GridLineWidth;
        canvas.StrokeColor = GridLineColor;
        if (GridLineWidth > 0)
            canvas.DrawLine(ptGridTop, ptGridBottom);
    }
    private void DrawHorizontalGridLine(ICanvas canvas, PlotInfo info)
    {
        // TODO: reduce duplication
        float y = info.GetPixelY(Position);

        PointF ptGridLeft = new(info.DataRect.Left, y);
        PointF ptGridRight = new(info.DataRect.Right, y);
        canvas.StrokeSize = GridLineWidth;
        canvas.StrokeColor = GridLineColor;
        if (GridLineWidth > 0)
            canvas.DrawLine(ptGridLeft, ptGridRight);
    }

    private void DrawBottomTickAndLabel(ICanvas canvas, PlotInfo info)
    {
        // TODO: reduce duplication
        float x = info.GetPixelX(Position);

        PointF pt1 = new(x, info.DataRect.Bottom);
        PointF pt2 = new(pt1.X, pt1.Y + TickMarkLength);
        canvas.StrokeColor = TickMarkColor;
        canvas.DrawLine(pt1, pt2);

        PointF pt3 = new(pt2.X, pt2.Y + TextPadding);
        Label.Draw(canvas, pt3.X, pt3.Y, HorizontalAlignment.Center, VerticalAlignment.Top);
    }

    private void DrawLeftTickAndLabel(ICanvas canvas, PlotInfo info)
    {
        // TODO: reduce duplication
        float y = info.GetPixelY(Position);

        PointF pt1 = new(info.DataRect.Left, y);
        PointF pt2 = new(pt1.X - TickMarkLength, pt1.Y);
        canvas.StrokeColor = TickMarkColor;
        canvas.DrawLine(pt1, pt2);

        PointF pt3 = new(pt2.X - TextPadding, pt2.Y);
        Label.Draw(canvas, pt3.X, pt3.Y, HorizontalAlignment.Right, VerticalAlignment.Center);
    }
}
