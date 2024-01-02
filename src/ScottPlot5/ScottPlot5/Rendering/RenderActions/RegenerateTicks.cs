namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.Bottom.TickGenerator.Regenerate(rp.Plot.Axes.Bottom.Range, rp.Plot.Axes.Bottom.Edge, rp.DataRect.Width);
        rp.Plot.Axes.Left.TickGenerator.Regenerate(rp.Plot.Axes.Left.Range, rp.Plot.Axes.Left.Edge, rp.DataRect.Height);
    }
}
