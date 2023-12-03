using System;
using System.Linq;

namespace ScottPlot.Ticks;

#nullable enable

public class TickCollection
{
    public double[] Major { get; }
    public double[] Minor { get; }
    public string[] Labels { get; }

    public TickCollection(double[] major, double[] minor, string[] labels)
    {
        if (major.Length != labels.Length)
            throw new InvalidOperationException($"{nameof(major)} must have the same length as {nameof(labels)}");

        Major = major ?? Array.Empty<double>();
        Minor = minor ?? Array.Empty<double>();
        Labels = labels ?? Array.Empty<string>();
    }

    public TickCollection(Tick[] ticks)
    {
        Major = ticks.Where(x => x.IsMajor).Select(x => x.Position).ToArray();
        Minor = ticks.Where(x => !x.IsMajor).Select(x => x.Position).ToArray();
        Labels = ticks.Where(x => x.IsMajor).Select(x => x.Label).ToArray();
    }

    public static TickCollection Empty => new(
        major: Array.Empty<double>(),
        minor: Array.Empty<double>(),
        labels: Array.Empty<string>());

    public static TickCollection First(TickCollection ticks) => new(
        major: new double[] { ticks.Major.First() },
        minor: Array.Empty<double>(),
        labels: new string[] { ticks.Labels.First() });

    public static TickCollection Last(TickCollection ticks) => new(
        major: new double[] { ticks.Major.Last() },
        minor: Array.Empty<double>(),
        labels: new string[] { ticks.Labels.Last() });

    public static TickCollection Two(TickCollection ticks) => new(
        major: new double[] { ticks.Major.First(), ticks.Major.Last() },
        minor: Array.Empty<double>(),
        labels: new string[] { ticks.Labels.First(), ticks.Labels.Last() });

    public static TickCollection Manual(Tick[] ticks) => new(
        major: ticks.Where(x => x.IsMajor).Select(x => x.Position).ToArray(),
        minor: ticks.Where(x => !x.IsMajor).Select(x => x.Position).ToArray(),
        labels: ticks.Where(x => x.IsMajor).Select(x => x.Label).ToArray());
}
