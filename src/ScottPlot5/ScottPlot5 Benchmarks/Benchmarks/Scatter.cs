using BenchmarkDotNet.Attributes;
using SkiaSharp;

namespace ScottPlotBench.Benchmarks;
public class Scatter
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
        double[] xs = gen.RandomSample(Points);
        double[] ys = gen.RandomSample(Points);

        Plot.Add.ScatterLine(xs, ys);
    }

    [Benchmark]
    public void ScatterLines()
    {
        Plot.Render(Surface);
    }
}
