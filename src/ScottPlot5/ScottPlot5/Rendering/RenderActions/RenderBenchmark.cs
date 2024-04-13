namespace ScottPlot.Rendering.RenderActions;

public class RenderBenchmark : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Benchmark.Render(rp);
    }
}
