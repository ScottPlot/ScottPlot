namespace ScottPlot.Statistics;

/// <summary>
/// A histogram that accumulates the number of values observed in a continuous range of user defined bins
/// </summary>
public class Histogram
{
    /// <summary>
    /// Number of values in each bin
    /// </summary>
    public int[] Counts { get; }

    /// <summary>
    /// Lower edge of each bin
    /// </summary>
    public double[] Bins { get; }

    /// <summary>
    /// Lower edge of each bin plus a final value representing the upper edge of the last bin
    /// </summary>
    public double[] Edges { get; }

    /// <summary>
    /// Size of the first bin (distance between the first pair of bin edges)
    /// </summary>
    public double FirstBinSize => Edges[1] - Edges[0];

    /// <summary>
    /// If enabled, values below or above the bin range will be accumulated in the lowest or highest bin
    /// </summary>
    public bool IncludeOutliers { get; set; } = false;

    private Histogram(IEnumerable<double> edges)
    {
        Edges = [.. edges];
        Bins = Edges.Take(Edges.Length - 1).ToArray();
        Counts = new int[Edges.Length - 1];
    }

    /// <summary>
    /// A collection of bins of size <paramref name="binSize"/> 
    /// where the first bin's left edge is <paramref name="firstBin"/> 
    /// and the last bin's left edge is <paramref name="lastBin"/>
    /// </summary>
    public static Histogram WithBinSize(double binSize, double firstBin, double lastBin)
    {
        if (lastBin <= firstBin)
            throw new ArgumentException($"{lastBin} must be greater than {nameof(firstBin)}");

        double span = lastBin - firstBin;
        int binCount = (int)(span / binSize);
        double[] edges = Enumerable.Range(0, binCount + 1).Select(x => firstBin + x * binSize).ToArray();
        return new Histogram(edges);
    }

    /// <summary>
    /// A collection of bins of size <paramref name="binSize"/> starting from the smallest value in <paramref name="values"/>
    /// and increasing to include the largest value in <paramref name="values"/>
    /// </summary>
    public static Histogram WithBinSize(double binSize, IEnumerable<double> values)
    {
        Histogram hist = WithBinSize(binSize, values.Min(), values.Max());
        hist.AddRange(values);
        return hist;
    }

    /// <summary>
    /// A collection of <paramref name="count"/> evenly sized bins.
    /// <paramref name="minValue"/> is the lower edge of the first bin.
    /// <paramref name="maxValue"/> is the lower edge of the last bin.
    /// </summary>
    public static Histogram WithBinCount(int count, double minValue, double maxValue)
    {
        if (maxValue <= minValue)
            throw new ArgumentException($"{maxValue} must be greater than {nameof(minValue)}");

        double[] edges = new double[count + 1];

        for (int i = 0; i < count; i++)
        {
            edges[i] = minValue + i * (maxValue - minValue) / count;
        }

        edges[^1] = maxValue;

        return new Histogram(edges);
    }

    /// <summary>
    /// A collection of <paramref name="count"/> evenly sized bins spaced to include the full range of <paramref name="values"/>
    /// </summary>
    public static Histogram WithBinCount(int count, IEnumerable<double> values)
    {
        Histogram hist = WithBinCount(count, values.Min(), values.Max());
        hist.AddRange(values);
        return hist;
    }

    public void Clear()
    {
        for (int i = 0; i < Counts.Length; i++)
        {
            Counts[i] = 0;
        }
    }

    public void Add(double value)
    {
        if (value < Edges[0])
        {
            if (IncludeOutliers)
            {
                Counts[0] += 1;
            }
            return;
        }

        if (value > Edges[^1])
        {
            if (IncludeOutliers)
            {
                Counts[^1] += 1;
            }
            return;
        }

        // TODO: improve performance using binary search
        for (int i = 0; i < Counts.Length; i++)
        {
            if (value >= Edges[i] && value < Edges[i + 1])
            {
                Counts[i] += 1;
                break;
            }
        }

        if (value == Edges[^1])
        {
            Counts[^1] += 1;
        }
    }

    public void AddRange(IEnumerable<double> values)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    /// <summary>
    /// Return counts normalized so the sum of all values equals 1
    /// </summary>
    public double[] GetProbability(double scale = 1)
    {
        int valuesCounted = Counts.Sum();
        return Counts.Select(x => scale * x / valuesCounted).ToArray();
    }

    /// <summary>
    /// Return the probability of each bin scaled so the peak is <paramref name="maxValue"/>
    /// </summary>
    public double[] GetNormalized(double maxValue = 1)
    {
        double scale = maxValue / Counts.Max();
        return Counts.Select(x => x * scale).ToArray();
    }

    /// <summary>
    /// Return the cumulative sum of all counts.
    /// Each value is the number of counts in that bin plus all bins below it.
    /// </summary>
    public int[] GetCumulativeCounts()
    {
        int[] cumulative = new int[Counts.Length];
        cumulative[0] = Counts[0];
        for (int i = 1; i < Counts.Length; i++)
        {
            cumulative[i] = cumulative[i - 1] + Counts[i];
        }
        return cumulative;
    }

    /// <summary>
    /// Return the cumulative probability histogram.
    /// Each value is the fraction of counts in that bin plus all bins below it.
    /// </summary>
    public double[] GetCumulativeProbability(double scale = 1.0)
    {
        int[] cumulative = GetCumulativeCounts();
        double final = cumulative.Last();
        return cumulative.Select(x => scale * x / final).ToArray();
    }
}
