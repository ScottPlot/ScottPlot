namespace ScottPlot.Rendering.RenderActions;

public class RenderBorders : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.FigureBorder.Render(rp.Canvas, rp.FigureRect, rp.Paint, contract: true);
        rp.Plot.DataBorder.Render(rp.Canvas, rp.DataRect, rp.Paint);
    }
}
