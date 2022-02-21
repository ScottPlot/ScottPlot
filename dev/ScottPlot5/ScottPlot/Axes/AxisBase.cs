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
}