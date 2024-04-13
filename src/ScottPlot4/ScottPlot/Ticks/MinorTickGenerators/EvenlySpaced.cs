using System;
using System.Collections.Generic;

#nullable enable

namespace ScottPlot.Ticks.MinorTickGenerators;

public class EvenlySpaced : IMinorTickGenerator
{
    public double[] GetMinorPositions(double[] majorTicks, double min, double max)
    {
        int divisions = 5;

        if (majorTicks is null || majorTicks.Length < 2)
            return Array.Empty<double>();

        double majorTickSpacing = majorTicks[1] - majorTicks[0];
        double minorTickSpacing = majorTickSpacing / divisions;

        List<double> majorTicksWithPadding = new();
        majorTicksWithPadding.Add(majorTicks[0] - majorTickSpacing);
        majorTicksWithPadding.AddRange(majorTicks);

        List<double> minorTicks = new();
        foreach (var majorTickPosition in majorTicksWithPadding)
        {
            for (int i = 1; i < divisions; i++)
            {
                double minorTickPosition = majorTickPosition + minorTickSpacing * i;
                if ((minorTickPosition > min) && (minorTickPosition < max))
                    minorTicks.Add(minorTickPosition);
            }
        }

        return minorTicks.ToArray();
    }
}
