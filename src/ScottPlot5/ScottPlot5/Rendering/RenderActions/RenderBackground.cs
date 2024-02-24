namespace ScottPlot.Rendering.RenderActions;

public class RenderBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        using SKPaint backgroundPaint = new() { Color = rp.Plot.DataBackground.ToSKColor() };

        SKRect skDataRect = rp.DataRect.ToSKRect(); 
        rp.Canvas.DrawRect(skDataRect, backgroundPaint);
        
        if (rp.Plot.DataBackgroundImage != null)
        {
            //draw background image centered in dataRect
            SKPoint drawPos = new()
            {
                X = skDataRect.MidX - rp.Plot.DataBackgroundImage.Width / 2f,
                Y = skDataRect.MidY - rp.Plot.DataBackgroundImage.Height / 2f
            };
            
            using SKPaint backgroundImagePaint = new() { Color = rp.Plot.DataBackgroundImageColor.ToSKColor() };
            rp.Canvas.DrawBitmap(rp.Plot.DataBackgroundImage, drawPos, backgroundImagePaint);
        }
    }
}
