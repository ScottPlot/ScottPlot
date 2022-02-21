using Microsoft.Maui.Graphics;
using System.Linq;

namespace ScottPlot.Axes;

public abstract class AxisBase
{
    // TODO: make readonly
    public Edge Edge { get; private set; }
    public Orientation Orientation { get; private set; }
    public ITickFactory TickFactory { get; set; }
    public TextLabel Label { get; } = new();

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

    public float Size(ICanvas canvas, Tick[]? ticks)
    {
        if (Orientation is Orientation.Horizontal)
        {
            float size = Label.Measure(canvas).Height;

            if (ticks is not null)
                size += ticks.Select(x => x.Measure(canvas).Height).Max();

            return size;
        }
        else
        {
            float size = Label.Measure(canvas).Height;

            if (ticks is not null)
                size += ticks.Select(x => x.Measure(canvas).Width).Max();

            return size;
        }
    }
}