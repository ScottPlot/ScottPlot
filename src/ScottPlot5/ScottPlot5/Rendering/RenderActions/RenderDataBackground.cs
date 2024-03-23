namespace ScottPlot.Rendering.RenderActions;

public class RenderDataBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        SKCanvas canvas = rp.Canvas;
        canvas.Save();

        rp.ClipToDataArea();
        rp.Plot.DataBackground?.Render(rp.Canvas, rp.DataRect);
        canvas.Restore();
    }
}
