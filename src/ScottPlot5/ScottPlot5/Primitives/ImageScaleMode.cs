namespace ScottPlot;

public enum ImageScaleMode
{
    /// <summary>
    /// No scaling. SKBitmap will be aligned on the upper-left.
    /// </summary>
    None,

    /// <summary>
    /// Fill image in X and Y to completely fill the area.
    /// The aspect ratio may change, appearing to disort the image.
    /// </summary>
    Stretch,

    /// <summary>
    /// Zoom in to the image until it fills the entire area.
    /// The image will be scaled to preserve the original aspect ratio.
    /// </summary>
    Fill,
}

public static class ImageScaleModeExtensions
{
    public static PixelRect GetImageRect(this ImageScaleMode scaleMode, int bmpWidth, int bmpHeight, PixelRect targetRect)
    {
        float left, right, bottom, top;

        switch (scaleMode)
        {
            case ImageScaleMode.Stretch:
                return targetRect;

            case ImageScaleMode.None:
                left = targetRect.Left;
                right = Math.Min(targetRect.Right, left + bmpWidth);
                top = targetRect.Top;
                bottom = Math.Min(targetRect.Bottom, top + bmpHeight);
                return new PixelRect(left, right, bottom, top);

            case ImageScaleMode.Fill:
                float ratioX = targetRect.Width / bmpWidth;
                float ratioY = targetRect.Height / bmpHeight;
                float minRatio = Math.Min(ratioX, ratioY);
                float width = minRatio * bmpWidth;
                float height = minRatio * bmpHeight;
                left = targetRect.HorizontalCenter - width / 2;
                right = targetRect.HorizontalCenter + width / 2;
                bottom = targetRect.VerticalCenter + height / 2;
                top = targetRect.VerticalCenter - height / 2;
                return new PixelRect(left, right, bottom, top);

            default:
                throw new NotImplementedException($"{scaleMode}");

        }
    }
}