namespace ScottPlot.Rendering.RenderActions;

public class RenderStartingEvent : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.RenderManager.RenderStarting.Invoke(this, rp);
    }
}
