namespace ScottPlot.TickGenerators;

public class NumericFixedInterval(double interval = 1) : ITickGenerator
{
    public double Interval { get; set; } = interval;

    public Func<double, string> LabelFormatter { get; set; } = LabelFormatters.Numeric;

    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 10_000;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        List<Tick> ticks = [];

        double lowest = range.Min - range.Min % Interval - Interval;
        double highest = range.Max - range.Max % Interval + Interval;
        int tickCount = (int)((highest - lowest) / Interval);
        tickCount = Math.Min(tickCount, MaxTickCount);

        for (int i = 0; i < tickCount; i++)
        {
            double position = range.IsInverted
                ? lowest + i * Interval
                : highest - i * Interval;

            string label = LabelFormatter(position);
            Tick tick = new(position, label, true);
            ticks.Add(tick);
        }

        Ticks = ticks.Where(x => range.Contains(x.Position)).ToArray();
    }
}
