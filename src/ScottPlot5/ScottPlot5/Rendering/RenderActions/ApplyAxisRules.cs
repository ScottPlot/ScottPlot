namespace ScottPlot.Rendering.RenderActions;

public class ApplyAxisRules : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.Rules.ForEach(x => x.Apply(rp));
    }
}
