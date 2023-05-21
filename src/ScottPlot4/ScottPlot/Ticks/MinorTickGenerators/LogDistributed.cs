using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace ScottPlot.Ticks.MinorTickGenerators;

public class LogDistributed : IMinorTickGenerator
{
    private int MinorTickCount;

    public LogDistributed(int minorTickCount = 10)
    {
        MinorTickCount = minorTickCount;
    }

    public double[] GetMinorPositions(double[] majorTickPositions, double min, double max)
    {
        if (majorTickPositions is null || majorTickPositions.Length < 2)
            return Array.Empty<double>();

        double majorTickSpacing = majorTickPositions[1] - majorTickPositions[0];
        double lowerBound = majorTickPositions.First() - majorTickSpacing;
        double upperBound = majorTickPositions.Last() + majorTickSpacing;

        List<double> minorTicks = new();
        for (double majorTick = lowerBound; majorTick <= upperBound; majorTick += majorTickSpacing)
        {
            double[] positions = GetLogDistributedPoints(MinorTickCount, majorTick, majorTick + majorTickSpacing, false);
            minorTicks.AddRange(positions);
        }

        return minorTicks.Where(x => x >= min && x <= max).ToArray();
    }

    /// <summary>
    /// Return log-distributed points between the min/max values
    /// </summary>
    /// <param name="count">number of divisions</param>
    /// <param name="min">lowest value</param>
    /// <param name="max">highest value</param>
    /// <param name="inclusive">if true, returned values will contain the min/max values themselves</param>
    public static double[] GetLogDistributedPoints(int count, double min, double max, bool inclusive)
    {
        double range = max - min;
        var values = DataGen.Range(1, 10, 10.0 / count)
            .Select(x => Math.Log10(x))
            .Select(x => x * range + min);
        return inclusive ? values.ToArray() : values.Skip(1).Take(count - 2).ToArray();
    }
}
