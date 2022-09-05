namespace ScottPlot.TickGenerators;

public class FixedSpacingTickGenerator : ITickGenerator
{
    public double InterTickSpacing = 1;

    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    public int MaxTickCount { get; set; } = 10_000;

    public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
    {
        return Ticks.Where(x => range.Contains(x.Position));
    }

    public void Regenerate(CoordinateRange range, PixelLength size)
    {
        List<Tick> ticks = new();

        double lowest = range.Min - range.Min % InterTickSpacing;
        double highest = range.Max - range.Max % InterTickSpacing + InterTickSpacing;
        int tickCount = (int)((highest - lowest) / InterTickSpacing);
        tickCount = Math.Min(tickCount, MaxTickCount);

        for (int i = 0; i < tickCount; i++)
        {
            double position = lowest + i * InterTickSpacing;
            string label = position.ToString();
            ticks.Add(new Tick(position, label, true));
        }

        Ticks = ticks.ToArray();
    }
}
