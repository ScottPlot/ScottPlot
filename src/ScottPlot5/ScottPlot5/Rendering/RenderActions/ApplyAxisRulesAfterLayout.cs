namespace ScottPlot.Rendering.RenderActions;

public class ApplyAxisRulesAfterLayout : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.Rules.ForEach(x => x.Apply(rp, beforeLayout: false));
    }
}
