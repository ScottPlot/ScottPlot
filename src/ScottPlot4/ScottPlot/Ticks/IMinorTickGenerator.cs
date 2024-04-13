namespace ScottPlot.Ticks;

public interface IMinorTickGenerator
{
    /// <summary>
    /// Return an array of minor tick marks for the given range
    /// </summary>
    /// <param name="majorTickPositions">Locations of visible major ticks. Must be evenly spaced.</param>
    /// <param name="min">Do not include minor ticks less than this value.</param>
    /// <param name="max">Do not include minor ticks greater than this value.</param>
    /// <returns>Array of minor tick positions (empty at positions occupied by major ticks)</returns>
    double[] GetMinorPositions(double[] majorTickPositions, double min, double max);
}
