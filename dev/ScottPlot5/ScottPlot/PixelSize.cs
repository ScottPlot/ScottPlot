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

    public SizeF SizeF => new(Width, Height);

    public bool HasPositiveArea => (Width > 0) && (Height > 0);

    public RectangleF RectangleF => new(0, 0, Width, Height);
}
