namespace ScottPlot.Statistics;

public interface IBinStrategy
{
    /// <summary>
    /// Lower edges of each bin plus a final value representing the upper edge of the last bin
    /// </summary>
    public double[] Edges { get; }
}
