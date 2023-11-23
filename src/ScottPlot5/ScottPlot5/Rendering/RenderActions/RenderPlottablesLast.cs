namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottablesLast : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.PlottableList
            .Where(x => x.IsVisible)
            .OfType<IRenderLast>()
            .ToList()
            .ForEach(x => x.RenderLast(rp));
    }
}
