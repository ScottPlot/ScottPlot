namespace ScottPlot.Rendering.RenderActions;

public class RenderGridsAbovePlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IGrid grid in rp.Plot.Grids.Where(x => !x.IsBeneathPlottables))
        {
            grid.Render(rp);
        }
    }
}
