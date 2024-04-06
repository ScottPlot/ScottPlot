using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace ScottPlotBench;

public static class Configurations
{
    public static IConfig Fast =>

        DefaultConfig.Instance.AddJob(Job.Default
                .WithLaunchCount(1)
                .WithIterationTime(Perfolizer.Horology.TimeInterval.Millisecond * 200)
                .WithWarmupCount(1)
                .WithIterationCount(1))
            .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig(true)));

    public static IConfig Default => DefaultConfig.Instance
        .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig(true)));
}
