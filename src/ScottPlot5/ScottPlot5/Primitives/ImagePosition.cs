namespace ScottPlot;

/// <summary>
/// Describes how to size and position an image inside a given rectangle
/// </summary>
public enum ImagePosition
{
    /// <summary>
    /// Image placed at the upper-left of the rectangle with no scaling.
    /// </summary>
    TopLeft,

    /// <summary>
    /// Image placed at the center of the rectangle with no scaling.
    /// </summary>
    Center,

    /// <summary>
    /// Fill image in X and Y to completely fill the area.
    /// The aspect ratio may change, appearing to distort the image.
    /// </summary>
    Stretch,

    /// <summary>
    /// Scale the image as large as possible such that it will fit
    /// entirely within the rectangle. This may result in whitespace
    /// on the edges if the image and rectangle have different aspect ratios.
    /// </summary>
    Fill,

    /// <summary>
    /// Scale the image by zooming in so it entirely fills the
    /// rectangle. It the aspect ratios are not the same, some of
    /// the image may lie outside the rectangle after scaling.
    Zoom,
}

public static class ImageScaleModeExtensions
{
    /// <summary>
    /// Return the image rectangle to display inside a target rectangle
    /// </summary>
    public static PixelRect GetRect(this ImagePosition position, PixelSize imageSize, PixelRect targetRect)
    {
        float imgWidth = imageSize.Width;
        float imgHeight = imageSize.Height;

        switch (position)
        {
            case ImagePosition.TopLeft:
                {
                    float left = targetRect.Left;
                    float top = targetRect.Top;
                    float right = left + imgWidth;
                    float bottom = top + imgHeight;
                    return new PixelRect(left, right, bottom, top);
                }

            case ImagePosition.Center:
                {
                    float left = targetRect.HorizontalCenter - imgWidth / 2;
                    float top = targetRect.VerticalCenter - imgHeight / 2;
                    float right = left + imgWidth;
                    float bottom = top + imgHeight;
                    return new PixelRect(left, right, bottom, top);
                }

            case ImagePosition.Stretch:
                {
                    return targetRect;
                }

            case ImagePosition.Fill:
                {
                    float ratioX = targetRect.Width / imgWidth;
                    float ratioY = targetRect.Height / imgHeight;
                    float minRatio = Math.Min(ratioX, ratioY);
                    float width = minRatio * imgWidth;
                    float height = minRatio * imgHeight;
                    float left = targetRect.HorizontalCenter - width / 2;
                    float right = targetRect.HorizontalCenter + width / 2;
                    float bottom = targetRect.VerticalCenter + height / 2;
                    float top = targetRect.VerticalCenter - height / 2;
                    return new PixelRect(left, right, bottom, top);
                }

            case ImagePosition.Zoom:
                {
                    float ratioX = targetRect.Width / imgWidth;
                    float ratioY = targetRect.Height / imgHeight;
                    float maxRatio = Math.Max(ratioX, ratioY);
                    float width = maxRatio * imgWidth;
                    float height = maxRatio * imgHeight;
                    float left = targetRect.HorizontalCenter - width / 2;
                    float right = targetRect.HorizontalCenter + width / 2;
                    float bottom = targetRect.VerticalCenter + height / 2;
                    float top = targetRect.VerticalCenter - height / 2;
                    return new PixelRect(left, right, bottom, top);
                }

            default:
                throw new NotImplementedException($"{position}");

        }
    }
}
