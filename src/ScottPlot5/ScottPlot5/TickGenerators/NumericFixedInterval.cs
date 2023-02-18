namespace ScottPlot.TickGenerators;

public class NumericFixedInterval : ITickGenerator
{
    public double Interval { get; set; }

    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    public int MaxTickCount { get; set; } = 10_000;

    public NumericFixedInterval(int interval = 1)
    {
        Interval = interval;
    }

    public void Regenerate(CoordinateRange range, PixelLength size)
    {
        List<Tick> ticks = new();

        double lowest = range.Min - range.Min % Interval;
        double highest = range.Max - range.Max % Interval + Interval;
        int tickCount = (int)((highest - lowest) / Interval);
        tickCount = Math.Min(tickCount, MaxTickCount);

        for (int i = 0; i < tickCount; i++)
        {
            double position = lowest + i * Interval;
            string label = position.ToString();
            ticks.Add(new Tick(position, label, true));
        }

        Ticks = ticks.Where(x => range.Contains(x.Position)).ToArray();
    }
}
