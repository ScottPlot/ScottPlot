namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        // TODO: shouldn't all axis ticks be regenerated???
        rp.Plot.Axes.Bottom.RegenerateTicks(rp.DataRect.Width);
        rp.Plot.Axes.Left.RegenerateTicks(rp.DataRect.Height);
    }
}
