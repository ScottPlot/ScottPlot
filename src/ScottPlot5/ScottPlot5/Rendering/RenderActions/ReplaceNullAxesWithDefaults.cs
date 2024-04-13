namespace ScottPlot.Rendering.RenderActions;

public class ReplaceNullAxesWithDefaults : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.ReplaceNullAxesWithDefaults();
    }
}
