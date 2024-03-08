namespace ScottPlot.Rendering.RenderActions;

public class RenderDataBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        using SKPaint paint = new() { Color = rp.Plot.DataBackground.Color.ToSKColor() };

        Drawing.Fillectangle(rp.Canvas, rp.DataRect, paint);

        if (rp.Plot.DataBackground.Image is not null)
        {
            Drawing.DrawImage(
                canvas: rp.Canvas,
                image: rp.Plot.DataBackground.Image,
                target: rp.Plot.DataBackground.GetImageRect(rp.DataRect));
        }
    }
}
