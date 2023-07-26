namespace ScottPlot.TickGenerators;

public class NumericManual : ITickGenerator
{
    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    public int MaxTickCount { get; set; } = 50;

    public NumericManual(Tick[] ticks)
    {
        Ticks = ticks;
    }

    public NumericManual(double[] positions, string[] labels)
    {
        if (positions.Length != labels.Length)
        {
            throw new ArgumentException("length mismatch");
        }

        Ticks = Enumerable
            .Range(0, positions.Length)
            .Select(x => new Tick(positions[x], labels[x], true))
            .ToArray();
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size)
    {
    }
}
