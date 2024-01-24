
namespace ScottPlot.TickGenerators;

public class LogMinorTickGenerator : IMinorTickGenerator
{
    public int Divisions { get; set; } = 5;

    public IEnumerable<double> GetMinorTicks(double[] majorPositions, CoordinateRange visibleRange)
    {
        if (majorPositions == null || majorPositions.Length < 2)
            return Array.Empty<double>();

        // determine range of major ticks to iterate (assume even spacing of all ticks)
        double deltaMajor = majorPositions[1] - majorPositions[0];
        double lowestMajor = majorPositions.First() - deltaMajor;
        double highestMajor = majorPositions.Last() + deltaMajor;

        // pre-calculate the log-distributed offset positions between major ticks
        IEnumerable<double> minorTickOffsets = Enumerable.Range(1, Divisions - 1)
            .Select(x => deltaMajor * Math.Log10(x * 10 / Divisions));

        // iterate major ticks and collect minor ticks with offsets
        List<double> minorTicks = [];
        for (double major = lowestMajor; major <= highestMajor; major += deltaMajor)
        {
            minorTicks.AddRange(minorTickOffsets.Select(offset => major + offset));
        }

        return minorTicks;
    }
}
