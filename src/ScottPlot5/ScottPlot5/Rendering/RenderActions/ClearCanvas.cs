namespace ScottPlot.Rendering.RenderActions;

public class ClearCanvas : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Canvas.Clear(rp.Plot.FigureBackground.ToSKColor());
    }
}
