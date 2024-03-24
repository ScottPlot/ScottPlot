namespace ScottPlot.Rendering.RenderActions;

public class RenderDataBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.CanvasState.Clip(rp.DataRect);
        rp.Plot.DataBackground?.Render(rp.Canvas, rp.DataRect);
        rp.CanvasState.DisableClipping();
    }
}
