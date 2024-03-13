namespace ScottPlot.Rendering.RenderActions;

internal class RenderFigureBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.FigureBackground?.Render(rp.Canvas, rp.FigureRect);
    }
}
