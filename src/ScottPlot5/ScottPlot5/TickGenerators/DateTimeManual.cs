namespace ScottPlot.TickGenerators;

public class DateTimeManual : ITickGenerator
{
    public Tick[] Ticks { get; private set; } = [];

    private readonly List<Tick> TickList = new();

    public int MaxTickCount { get; set; } = 50;

    public DateTimeManual()
    {
    }

    public DateTimeManual(Tick[] ticks)
    {
        TickList.AddRange(ticks);
    }

    public DateTimeManual(DateTime[] positions, string[] labels)
    {
        if (positions.Length != labels.Length)
            throw new ArgumentException($"{nameof(positions)} must have same length as {nameof(labels)}");

        for (int i = 0; i < positions.Length; i++)
        {
            AddMajor(positions[i], labels[i]);
        }
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        Ticks = TickList.Where(x => range.Contains(x.Position)).ToArray();
    }

    public void Add(Tick tick)
    {
        TickList.Add(tick);
    }

    public void AddMajor(DateTime position, string label)
    {
        Add(Tick.Major(position.ToOADate(), label));
    }

    public void AddMinor(DateTime position)
    {
        Add(Tick.Minor(position.ToOADate()));
    }
}
