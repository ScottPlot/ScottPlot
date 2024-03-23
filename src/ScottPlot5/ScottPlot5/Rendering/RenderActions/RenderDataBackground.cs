namespace ScottPlot.Rendering.RenderActions;

public class RenderDataBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        using RenderPack.RestoreState _ = rp.PushClipToDataArea();
        rp.Plot.DataBackground?.Render(rp.Canvas, rp.DataRect);
    }
}
