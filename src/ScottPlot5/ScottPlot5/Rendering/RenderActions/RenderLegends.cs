namespace ScottPlot.Rendering.RenderActions;

public class RenderLegends : IRenderAction
{
    public void Render(RenderPack rp)
    {
        LegendItem[] items = rp.Plot.Plottables.SelectMany(x => x.LegendItems).ToArray();

        foreach (ILegend legend in rp.Plot.Legends)
        {
            legend.Render(rp);
        }
    }
}
