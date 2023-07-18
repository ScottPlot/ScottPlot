namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.XAxis.TickGenerator.Regenerate(rp.Plot.XAxis.Range, rp.Plot.XAxis.Edge, rp.DataRect.Width);
        rp.Plot.YAxis.TickGenerator.Regenerate(rp.Plot.YAxis.Range, rp.Plot.YAxis.Edge, rp.DataRect.Height);
    }
}
