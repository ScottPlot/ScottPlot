namespace ScottPlot.Rendering.RenderActions;

public class CalculateLayout : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.CalculateLayout();
    }
}
