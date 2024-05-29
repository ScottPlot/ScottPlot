namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottablesLast : IRenderAction
{
    public void Render(RenderPack rp)
    {
        IRenderLast[] visibleLastPlottables = rp.Plot.PlottableList
            .Where(x => x.IsVisible)
            .OfType<IRenderLast>()
            .ToArray();

        foreach (IRenderLast plottable in visibleLastPlottables)
        {
            plottable.RenderLast(rp);
        }
    }
}
