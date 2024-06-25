namespace ScottPlot;

public class Population
{
    public IReadOnlyList<double> Values { get; }
    public IReadOnlyList<double> SortedValues { get; }
    public int Count { get; }
    public double Min { get; }
    public double Max { get; }
    public double Mean { get; }
    public double Variance { get; }
    public double StandardDeviation { get; }
    public double StandardError { get; }
    public double Median { get; }

    public Population(double[] values)
    {
        Values = [.. values];
        SortedValues = [.. Values.OrderBy(x => x)];
        Count = values.Length;
        Min = values.Min();
        Max = values.Max();
        Mean = Statistics.Descriptive.Mean(values);
        Variance = Statistics.Descriptive.Variance(values);
        StandardDeviation = Statistics.Descriptive.StandardDeviation(values);
        StandardError = Statistics.Descriptive.StandardError(values);
        Median = Statistics.Descriptive.SortedMedian(SortedValues);
    }

    public double GetPercentile(double percentile) => Statistics.Descriptive.SortedPercentile(SortedValues, percentile);
}
