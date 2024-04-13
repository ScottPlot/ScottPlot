namespace ScottPlot.Rendering.RenderActions;

internal class ClearCanvas : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Canvas.Clear();
    }
}
