namespace ScottPlot.Statistics;

public interface IBinStrategy
{
    /// <summary>
    /// An array containing the ordered lower edges of each bin
    /// plus a final value representing the upper edge of the last bin
    /// </summary>
    public double[] Bins { get; }
}
