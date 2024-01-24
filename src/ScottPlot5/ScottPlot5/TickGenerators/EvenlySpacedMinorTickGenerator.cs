namespace ScottPlot.TickGenerators;

public class EvenlySpacedMinorTickGenerator(double minorTickPerMajorTick) : IMinorTickGenerator
{
    public double MinorTicksPerMajorTick { get; set; } = minorTickPerMajorTick;

    public IEnumerable<double> GetMinorTicks(double[] majorTicks, CoordinateRange visibleRange)
    {
        if (majorTicks == null || majorTicks.Length < 2)
            return Array.Empty<double>();

        double majorTickSpacing = majorTicks[1] - majorTicks[0];
        double minorTickSpacing = majorTickSpacing / MinorTicksPerMajorTick;

        List<double> majorTicksWithPadding = [majorTicks[0] - majorTickSpacing, .. majorTicks];

        List<double> minorTicks = [];
        foreach (var majorTickPosition in majorTicksWithPadding)
        {
            for (int i = 1; i < MinorTicksPerMajorTick; i++)
            {
                double minorTickPosition = majorTickPosition + minorTickSpacing * i;
                if (visibleRange.Contains(minorTickPosition))
                    minorTicks.Add(minorTickPosition);
            }
        }

        return minorTicks;
    }
}
