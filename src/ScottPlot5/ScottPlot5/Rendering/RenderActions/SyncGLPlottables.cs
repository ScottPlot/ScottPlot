namespace ScottPlot.Rendering.RenderActions;

public class SyncGLPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        var glPlottables = rp.Plot.Plottables.OfType<IPlottableGL>();
        if (glPlottables.Any())
            glPlottables.First().GLFinish();
    }
}
