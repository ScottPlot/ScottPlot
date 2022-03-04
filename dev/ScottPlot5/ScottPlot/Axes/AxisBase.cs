using Microsoft.Maui.Graphics;
using System.Linq;

namespace ScottPlot.Axes;

public abstract class AxisBase
{
    public Edge Edge { get; private set; }
    public Orientation Orientation { get; private set; }
    public ITickFactory TickFactory { get; set; }
    public TextLabel Label { get; } = new();

    public Color SpineColor = Colors.Black;
    public float SpineLineWidth = 1.0f;
    public float SpineOffset = 0.5f;

    public AxisBase(Edge edge, string text)
    {
        Edge = edge;
        Orientation = edge switch
        {
            Edge.Left => Orientation.Vertical,
            Edge.Right => Orientation.Vertical,
            Edge.Top => Orientation.Horizontal,
            Edge.Bottom => Orientation.Horizontal,
            _ => throw new System.NotImplementedException(),
        };
        TickFactory = new TickFactories.EmptyTickFactory(edge);
        Label.Text = text;
    }

    public void Ticks(bool enable)
    {
        TickFactory = enable
            ? new TickFactories.LegacyNumericTickFactory(Edge)
            : new TickFactories.EmptyTickFactory(Edge);
    }

    public float Measure(ICanvas canvas, Tick[]? ticks)
    {
        if (Orientation is Orientation.Horizontal)
        {
            float size = Label.Measure(canvas).Height;

            if (ticks is not null && ticks.Any())
                size += ticks.Select(x => x.Measure(canvas).Height).Max();

            return size;
        }
        else
        {
            float size = Label.Measure(canvas).Height;

            if (ticks is not null && ticks.Any())
                size += ticks.Select(x => x.Measure(canvas).Width).Max();

            return size;
        }
    }

    public static void DrawVerticalGridLines(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        foreach (Tick tick in ticks)
        {
            float x = info.GetPixelX(tick.Position);

            PointF ptGridTop = new(x, info.DataRect.Top);
            PointF ptGridBottom = new(x, info.DataRect.Bottom);
            canvas.StrokeSize = tick.GridLineWidth;
            canvas.StrokeColor = tick.GridLineColor;
            if (tick.GridLineWidth > 0)
                canvas.DrawLine(ptGridTop, ptGridBottom);
        }
    }

    public static void DrawHorizontalGridLines(ICanvas canvas, PlotConfig info, Tick[] ticks)
    {
        foreach (Tick tick in ticks)
        {
            float y = info.GetPixelY(tick.Position);

            PointF ptGridLeft = new(info.DataRect.Left, y);
            PointF ptGridRight = new(info.DataRect.Right, y);
            canvas.StrokeSize = tick.GridLineWidth;
            canvas.StrokeColor = tick.GridLineColor;
            if (tick.GridLineWidth > 0)
                canvas.DrawLine(ptGridLeft, ptGridRight);
        }
    }
}