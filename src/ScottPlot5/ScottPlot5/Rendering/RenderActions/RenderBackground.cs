using System.Drawing;
using ScottPlot.AxisRules;

namespace ScottPlot.Rendering.RenderActions;

public class RenderBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        using SKPaint backgroundPaint = new() { Color = rp.Plot.DataBackground.ToSKColor() };

        SKRect skDataRect = rp.DataRect.ToSKRect();
        rp.Canvas.DrawRect(skDataRect, backgroundPaint);

        SKBitmap? backgroundImage = rp.Plot.DataBackgroundImage;
        if (backgroundImage != null)
        {
            //calculate dest image size
            SKSize destSize = new();
            SKSize srcSize = new(backgroundImage.Width, backgroundImage.Height);

            switch (rp.Plot.DataBackgroundScalingStyle)
            {
                case ImageScalingStyle.None:
                    //Selecting minimum of DataBackgroundImageSize and DataRectSize for clipping. 
                    destSize.Width = Math.Min(backgroundImage.Width, skDataRect.Width);
                    destSize.Height = Math.Min(backgroundImage.Height, skDataRect.Height);

                    //clipping on width
                    if (backgroundImage.Width > skDataRect.Width)
                    {
                        //set src width to ensure clipping
                        srcSize.Width = Math.Max(0f, backgroundImage.Width - skDataRect.Width / 2f);
                    }

                    //clipping on height
                    if (backgroundImage.Height > skDataRect.Height)
                    {
                        //set src height to ensure clipping
                        srcSize.Height = Math.Max(0f, backgroundImage.Height - skDataRect.Height / 2f);
                    }
                    break;

                case ImageScalingStyle.StretchToFill:
                    destSize.Width = skDataRect.Width;
                    destSize.Height = skDataRect.Height;
                    break;

                default: //intentional default fall-through
                case ImageScalingStyle.FillRetainAspect:
                    float scale = Math.Min
                    (
                        skDataRect.Width / backgroundImage.Width,
                        skDataRect.Height / backgroundImage.Height
                    );

                    destSize.Width = scale * backgroundImage.Width;
                    destSize.Height = scale * backgroundImage.Height;
                    break;
            }

            //Create scrRect
            SKRect srcRect = SKRect.Create
            (
                (backgroundImage.Width - srcSize.Width) / 2f,
                (backgroundImage.Height - srcSize.Height) / 2f,
                srcSize.Width,
                srcSize.Height
            );

            //Create destRect
            SKRect destRect = SKRect.Create
            (
                skDataRect.Left + (skDataRect.Width - destSize.Width) / 2f,
                skDataRect.Top + (skDataRect.Height - destSize.Height) / 2f,
                destSize.Width,
                destSize.Height
            );

            using SKPaint backgroundImagePaint = new() { Color = rp.Plot.DataBackgroundImageColor.ToSKColor() };
            rp.Canvas.DrawBitmap(backgroundImage, srcRect, destRect, backgroundImagePaint);
        }
    }
}
