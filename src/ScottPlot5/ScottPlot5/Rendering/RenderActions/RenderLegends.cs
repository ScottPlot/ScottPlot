namespace ScottPlot.Rendering.RenderActions;

public class RenderLegends : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Legend.Render(rp);
    }
}
