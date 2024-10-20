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
    /// Lower edge of the smallest bin
    /// </summary>
    public double BinMin { get; }

    /// <summary>
    /// Upper edge of the largest bin
    /// </summary>
    public double BinMax { get; }

    /// <summary>
    /// If enabled, values added outside the range of bins will be counted in the nearest bin
    /// </summary>
    public bool CountOutliers = false;

    public Histogram(double[] binEdges)
    {
        Edges = binEdges;
        Bins = Edges.Take(Edges.Length - 1).ToArray();
        BinMin = binEdges[0];
        BinMax = binEdges[^1];
        Counts = new int[binEdges.Length - 1];
    }

    public Histogram(IEnumerable<double> values, double[] binEdges) : this(binEdges)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    public void Add(double value)
    {
        // TODO: improve performance using binary search

        if (value <= BinMin)
        {
            if (CountOutliers)
            {
                Counts[0] += 1;
            }
            return;
        }

        if (value >= BinMax)
        {
            if (CountOutliers)
            {
                Counts[^1] += 1;
            }
            return;
        }

        for (int i = 0; i < Counts.Length; i++)
        {
            double lower = Edges[i];
            double upper = Edges[i + 1];
            bool isLastBin = i == Edges.Length - 2;
            if (value >= lower && value < upper)
            {
                Counts[i] += 1;
                break;
            }
        }
    }
}
