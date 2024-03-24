namespace ScottPlot.Rendering.RenderActions;

public class RenderDataBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.ClipToDataArea();
        rp.Plot.DataBackground?.Render(rp.Canvas, rp.DataRect);
        rp.DisableClipping();
    }
}
