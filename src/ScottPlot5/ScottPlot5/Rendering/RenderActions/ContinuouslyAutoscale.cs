namespace ScottPlot.Rendering.RenderActions;

public class ContinuouslyAutoscale : IRenderAction
{
    public void Render(RenderPack rp)
    {
        if (rp.Plot.Axes.ContinuouslyAutoscale)
        {
            rp.Plot.Axes.ContinuousAutoscaleAction.Invoke(rp);
        }
    }
}
