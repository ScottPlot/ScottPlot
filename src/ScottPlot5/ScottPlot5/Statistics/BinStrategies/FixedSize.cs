namespace ScottPlot.Statistics.BinStrategies;

public class FixedSize : IBinStrategy
{
    public double[] Bins { get; }

    /// <summary>
    /// A collection of bins of size <paramref name="binSize"/> starting from <paramref name="minValue"/> and 
    /// increasing to include <paramref name="maxValue"/>
    /// </summary>
    public FixedSize(double binSize, double minValue, double maxValue)
    {
        if (maxValue <= minValue)
            throw new ArgumentException($"{maxValue} must be greater than {nameof(minValue)}");

        double span = maxValue - minValue;
        int count = (int)Math.Ceiling(span / binSize);
        double maxBin = minValue + count * binSize;

        Bins = [.. Enumerable.Range(0, count).Select(x => minValue + x * binSize), maxBin];
    }

    /// <summary>
    /// A collection of bins of size <paramref name="binSize"/> starting from the smallest value in <paramref name="values"/>
    /// and increasing to include the largest value in <paramref name="values"/>
    /// </summary>
    public FixedSize(double binSize, IEnumerable<double> values) : this(binSize, values.Min(), values.Max()) { }
}
