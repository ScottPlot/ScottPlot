namespace ScottPlot.TickGenerators;

public class NumericFixedInterval(int interval = 1) : ITickGenerator
{
    public double Interval { get; set; } = interval;

    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 10_000;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle)
    {
        List<Tick> ticks = [];

        double lowest = range.TrueMin - range.TrueMin % Interval;
        double highest = range.TrueMax - range.TrueMax % Interval + Interval;
        int tickCount = (int)((highest - lowest) / Interval);
        tickCount = Math.Min(tickCount, MaxTickCount);

        for (int i = 0; i < tickCount; i++)
        {
            double position = range.IsInverted
                ? lowest + i * Interval
                : highest - i * Interval;

            string label = position.ToString();
            Tick tick = new(position, label, true);
            ticks.Add(tick);
        }

        Ticks = ticks.Where(x => range.Contains(x.Position)).ToArray();
    }
}
