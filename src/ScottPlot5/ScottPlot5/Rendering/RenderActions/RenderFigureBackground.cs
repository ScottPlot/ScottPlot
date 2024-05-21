namespace ScottPlot.Rendering.RenderActions;

public class RenderFigureBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.FigureBackground?.Render(rp.Canvas, rp.ScaledFigureRect);
    }
}
