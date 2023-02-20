using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Statistics;

public class Histogram
{
    /// <summary>
    /// Number of values counted for each bin.
    /// </summary>
    public readonly int[] Counts;

    /// <summary>
    /// Lower edge for each bin.
    /// </summary>
    public readonly double[] BinEdges;

    /// <summary>
    /// Default behavior is that outlier values are not counted.
    /// If this is enabled, min/max outliers will be counted in the first/last bin.
    /// </summary>
    public readonly bool AddOutliersToEdgeBins;

    /// <summary>
    /// Lower edge of the first bin
    /// </summary>
    public readonly double Min;

    /// <summary>
    /// Upper edge of the last bin
    /// </summary>
    public readonly double Max;

    /// <summary>
    /// The calculated bin size.
    /// </summary>
    public double BinSize { get; private set; } = 0;

    /// <summary>
    /// Number of values that were smaller than the lower edge of the first bin.
    /// </summary>
    public int MinOutlierCount { get; private set; } = 0;

    /// <summary>
    /// Number of values that were greater than the upper edge of the last bin.
    /// </summary>
    public int MaxOutlierCount { get; private set; } = 0;

    /// <summary>
    /// Create a histogram which will count values supplied by <see cref="Add"/> and <see cref="AddRange"/>
    /// </summary>
    public Histogram(double min, double max, int binCount, bool addOutliersToEdgeBins = false)
    {
        Min = min;
        Max = max;
        AddOutliersToEdgeBins = addOutliersToEdgeBins;
        Counts = new int[binCount];
        BinEdges = new double[binCount];

        // create evenly sized bins
        BinSize = (Max - Min) / binCount;

        for (int i = 0; i < binCount; i++)
        {
            BinEdges[i] = min + BinSize * i;
        }
    }

    /// <summary>
    /// Return counts normalized so the sum of all counts equals 1
    /// </summary>
    public double[] GetProbability()
    {
        double total = Counts.Sum();
        return Counts.Select(x => x / total).ToArray();
    }

    /// <summary>
    /// Return counts normalized so the maximum value equals the given value
    /// </summary>
    public double[] GetNormalized(double maxValue = 1)
    {
        double mult = maxValue / Counts.Max();
        return Counts.Select(x => x * mult).ToArray();
    }

    /// <summary>
    /// Add a single value to the histogram
    /// </summary>
    public void Add(double value)
    {
        // place values in histogram
        if (value < Min)
        {
            MinOutlierCount += 1;
            if (AddOutliersToEdgeBins)
            {
                Counts[0] += 1;
            }
        }
        else if (value >= Max)
        {
            MaxOutlierCount += 1;
            if (AddOutliersToEdgeBins)
            {
                Counts[Counts.Length - 1] += 1;
            }
        }
        else
        {
            double distanceFromMin = value - Min;
            int binsFromMin = (int)(distanceFromMin / BinSize);
            Counts[binsFromMin] += 1;
        }
    }

    /// <summary>
    /// Add multiple values to the histogram
    /// </summary>
    public void AddRange(IEnumerable<double> values)
    {
        foreach (double value in values)
            Add(value);
    }

    /// <summary>
    /// Reset the histogram, setting all counts and values to zero
    /// </summary>
    public void Clear()
    {
        MinOutlierCount = 0;
        MaxOutlierCount = 0;
        for (int i = 0; i < Counts.Length; i++)
        {
            Counts[i] = 0;
        }
    }
}
