namespace ScottPlot.Rendering.RenderActions;

internal class ExecutePlottableAxisManagers : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.PlottableList
            .OfType<IManagesAxisLimits>()
            .Where(x => x.ManageAxisLimits)
            .ToList()
            .ForEach(x => x.UpdateAxisLimits(rp.Plot));
    }
}
