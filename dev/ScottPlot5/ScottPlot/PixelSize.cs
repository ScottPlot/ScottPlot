using Microsoft.Maui.Graphics;

namespace ScottPlot;

public struct PixelSize
{
    public readonly float Width;
    public readonly float Height;

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"Pixel Size: Width = {Width} px, Height = {Height} px";
    }

    public static PixelSize FromSizeF(SizeF size) => new(size.Width, size.Height);

    public SizeF SizeF => new(Width, Height);

    public bool HasPositiveArea => (Width > 0) && (Height > 0);

    public RectangleF RectangleF => new(0, 0, Width, Height);
    public PixelSize WithWidth(float width) => new(width, Height);
    public PixelSize WithHeight(float height) => new(Width, height);
    public PixelSize WidenedBy(float additionalWidth) => new(Width + additionalWidth, Height);
    public PixelSize HeightenedBy(float additionalHeight) => new(Width, Height + additionalHeight);
}
