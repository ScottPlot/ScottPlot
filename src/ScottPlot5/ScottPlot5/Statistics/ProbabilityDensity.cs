namespace ScottPlot.Statistics;

/// <summary>
/// Represents a Gaussian distribution of probabilities
/// for a normal distributed population.
/// </summary>
public class ProbabilityDensity
{
    public readonly double Mean;
    public readonly double StDev;

    public ProbabilityDensity(IEnumerable<double> values)
        : this(Descriptive.Mean(values), Descriptive.StandardDeviation(values))
    { }

    public ProbabilityDensity(double mean, double stDev)
    {
        Mean = mean;
        StDev = stDev;
    }

    public double GetY(double x, double mult = 1)
    {
        double u = (x - Mean) / StDev;
        return Math.Exp(-0.5 * u * u) * mult;
    }

    public double[] GetYs(double[] xs, double mult = 1)
    {
        return xs.Select(x => GetY(x, mult)).ToArray();
    }
}
