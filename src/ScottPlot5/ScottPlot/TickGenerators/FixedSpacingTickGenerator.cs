namespace ScottPlot.TickGenerators;

public class FixedSpacingTickGenerator : ITickGenerator
{
    public double InterTickSpacing = 1;

    public Tick[] GenerateTicks(double min, double max, float edgeSize)
    {
        List<Tick> ticks = new();

        double lowest = min - min % InterTickSpacing;
        double highest = max - max % InterTickSpacing + InterTickSpacing;
        int tickCount = (int)((highest - lowest) / InterTickSpacing);

        for (int i = 0; i < tickCount; i++)
        {
            double position = lowest + i * InterTickSpacing;
            string label = position.ToString();
            ticks.Add(new Tick(position, label, true));
        }

        return ticks.ToArray();
    }
}
