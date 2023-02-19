using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Statistics;

#pragma warning disable RS0016 // Ignore warnings that this class is not yet documented

[Obsolete("This class is a work in progress. Its API may change as it continues to evolve.")]
public class Histogram
{
    /// <summary>
    /// Number of values counted for each bin.
    /// </summary>
    public readonly int[] Counts;

    /// <summary>
    /// Normalized values calculated for each bin.
    /// </summary>
    public readonly double[] Normalized;

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
    private readonly double Min;

    /// <summary>
    /// Upper edge of the last bin
    /// </summary>
    private readonly double Max;

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
        Normalized = new double[binCount];
        BinEdges = new double[binCount];

        // create evenly sized bins
        BinSize = (Max - Min) / binCount;

        for (int i = 0; i < binCount; i++)
        {
            BinEdges[i] = min + BinSize * i;
        }
    }

    /// <summary>
    /// Add a single value to the histogram
    /// </summary>
    public void Add(double value)
    {
        //For performance reasons,only recalculate the normalized
        //array if value is not an outlier, or AddOutliersToEdgeBins is set true 
        bool recalcNormalized = false;

        // place values in histogram
        if (value < Min)
        {
            MinOutlierCount += 1;
            if (AddOutliersToEdgeBins)
            {
                Counts[0] += 1;
                recalcNormalized = true;
            }
        }
        else if (value >= Max)
        {
            MaxOutlierCount += 1;
            if (AddOutliersToEdgeBins)
            {
                Counts[Counts.Length - 1] += 1;
                recalcNormalized = true;
            }
        }
        else
        {
            double distanceFromMin = value - Min;
            int binsFromMin = (int)(distanceFromMin / BinSize);
            Counts[binsFromMin] += 1;
            recalcNormalized = true;
        }

        // normalize the available data
        if (recalcNormalized)
        {
            double binScale = Counts.Sum() * BinSize;
            for (int i = 0; i < Counts.Length; i++) Normalized[i] = Counts[i] / binScale;
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
            Normalized[i] = 0;
        }
    }
}
