namespace ScottPlot.Rendering.RenderActions;

internal class ContinuouslyAutoscale : IRenderAction
{
    public void Render(RenderPack rp)
    {
        if (rp.Plot.Axes.ContinuouslyAutoscale)
        {
            rp.Plot.Axes.ContinuousAutoscaleAction.Invoke(rp);
        }
    }
}
