using System;
using System.Collections.Generic;

namespace ScottPlot.Statistics;

#pragma warning disable RS0016 // Ignore warnings that this class is not yet documented

[Obsolete("This class is a work in progress. Its API may change as it continues to evolve.")]
public class Histogram
{
    /// <summary>
    /// Number of values counted for each bin
    /// </summary>
    public readonly int[] Counts;

    /// <summary>
    /// Lower edge for each bin
    /// </summary>
    public readonly double[] BinEdges;

    /// <summary>
    /// Number of values that were smaller than the lower edge of the smallest bin
    /// </summary>
    public int MinOutlierCount { get; private set; } = 0;

    /// <summary>
    /// Number of values that were greater than the upper edge of the smallest bin
    /// </summary>
    public int MaxOutlierCount { get; private set; } = 0;

    /// <summary>
    /// Default behavior is that outlier values are not counted.
    /// If this is enabled, min/max outliers will be counted in the first/last bin.
    /// </summary>
    public bool AddOutliersToEdgeBins { get; set; } = false;

    /// <summary>
    /// Create a histogram which will count values supplied by <see cref="Add"/> and <see cref="AddRange"/>
    /// </summary>
    public Histogram(double firstBin, double binSize, int binCount)
    {
        Counts = new int[binCount];
        BinEdges = new double[binCount];

        for (int i = 0; i < binCount; i++)
        {
            BinEdges[i] = firstBin + binSize * i;
        }
    }

    /// <summary>
    /// Add a single value to the histogram
    /// </summary>
    public void Add(double value)
    {
        throw new NotImplementedException();
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
    /// Reset the histogram, setting all counts to zero
    /// </summary>
    public void Clear()
    {
        MinOutlierCount = 0;
        MaxOutlierCount = 0;
        for (int i = 0; i < Counts.Length; i++)
            Counts[i] = 0;
    }
}
