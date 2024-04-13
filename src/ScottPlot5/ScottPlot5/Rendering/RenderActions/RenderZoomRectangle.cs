namespace ScottPlot.Rendering.RenderActions;

public class RenderZoomRectangle : IRenderAction
{
    public void Render(RenderPack rp)
    {
        if (rp.Plot.ZoomRectangle.IsVisible)
        {
            rp.Plot.ZoomRectangle.Render(rp);
        }
    }
}
