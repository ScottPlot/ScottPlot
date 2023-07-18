namespace ScottPlot.Rendering.RenderActions;

public class RenderBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Canvas.Clear(rp.Plot.FigureBackground.ToSKColor());

        using SKPaint paint = new() { Color = rp.Plot.DataBackground.ToSKColor() };
        rp.Canvas.DrawRect(rp.DataRect.ToSKRect(), paint);
    }
}
