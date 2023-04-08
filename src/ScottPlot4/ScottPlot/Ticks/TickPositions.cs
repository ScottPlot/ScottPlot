using System;

namespace ScottPlot.Ticks;

#nullable enable

public class TickPositions
{
    public double[] Major { get; }
    public double[] Minor { get; }
    public string[] Labels { get; }

    public TickPositions(double[] major, double[] minor, string[] labels)
    {
        if (major.Length != labels.Length)
            throw new InvalidOperationException($"{nameof(major)} must have the same length as {nameof(labels)}");

        Major = major ?? Array.Empty<double>();
        Minor = minor ?? Array.Empty<double>();
        Labels = labels ?? Array.Empty<string>();
    }

    public static TickPositions Empty => new(
        major: Array.Empty<double>(),
        minor: Array.Empty<double>(),
        labels: Array.Empty<string>());
}
