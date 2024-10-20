namespace ScottPlot.Statistics.BinStrategies;

public class FixedCount : IBinStrategy
{
    public double[] Bins { get; }

    /// <summary>
    /// A collection of <paramref name="count"/> evenly sized bins between <paramref name="minValue"/> and <paramref name="maxValue"/>
    /// </summary>
    public FixedCount(int count, double minValue, double maxValue)
    {
        if (maxValue <= minValue)
            throw new ArgumentException($"{maxValue} must be greater than {nameof(minValue)}");

        double binSize = (maxValue - minValue) / count;

        Bins = [.. Enumerable.Range(0, count).Select(x => minValue + x * binSize), maxValue];
    }

    /// <summary>
    /// A collection of <paramref name="count"/> evenly sized bins between the min and max values in <paramref name="values"/>
    /// </summary>
    public FixedCount(int count, IEnumerable<double> values) : this(count, values.Min(), values.Max()) { }
}
