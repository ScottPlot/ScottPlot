namespace ScottPlot.TickGenerators;

public class LogDecadeMinorTickGenerator : IMinorTickGenerator
{
    public int TicksPerDecade { get; set; } = 10;

    public IEnumerable<double> GetMinorTicks(double[] majorPositions, CoordinateRange visibleRange)
    {
        var minDecadeTick = Math.Floor(visibleRange.Min);
        var maxDecadeTick = Math.Ceiling(visibleRange.Max);

        if (minDecadeTick == maxDecadeTick)
            return [];

        // pre-calculate the log-distributed offset positions between major ticks
        IEnumerable<double> minorTickOffsets = Enumerable.Range(1, TicksPerDecade - 1)
            .Select(x => Math.Log10(x * 10 / TicksPerDecade));

        // iterate major ticks and collect minor ticks with offsets
        List<double> minorTicks = [];
        for (double major = minDecadeTick; major <= maxDecadeTick; major += 1)
        {
            minorTicks.AddRange(minorTickOffsets.Select(offset => major + offset));
        }

        return minorTicks;
    }
}
