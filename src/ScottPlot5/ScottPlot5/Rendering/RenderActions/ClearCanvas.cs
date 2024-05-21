namespace ScottPlot.Rendering.RenderActions;

public class ClearCanvas : IRenderAction
{
    public void Render(RenderPack rp)
    {
        if (rp.Plot.RenderManager.ClearCanvasBeforeEachRender)
        {
            rp.Canvas.Clear();
        }
    }
}
