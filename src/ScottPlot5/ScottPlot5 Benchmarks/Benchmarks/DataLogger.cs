using BenchmarkDotNet.Attributes;
using SkiaSharp;

namespace ScottPlotBench.Benchmarks;
public class DataLogger
{
    [Params(100, 1_000, 10_000, 100_000)]
    public int Points { get; set; }

    private ScottPlot.Plot Plot;

    private SKSurface Surface;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Plot = new ScottPlot.Plot();
        Surface = ScottPlot.Drawing.CreateSurface(400, 600);

        ScottPlot.RandomDataGenerator gen = new(0);
        double[] ys = gen.RandomSample(Points);

        var dl = Plot.Add.DataLogger();
        dl.Add(ys);
    }

    [Benchmark]
    public void DataLoggerRender()
    {
        Plot.Render(Surface);
    }
}
