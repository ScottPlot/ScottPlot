namespace ScottPlot.Rendering.RenderActions;

public class RenderBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        using SKPaint paint = new() { Color = rp.Plot.DataBackground.ToSKColor() };
        rp.Canvas.DrawRect(rp.DataRect.ToSKRect(), paint);
    }
}
