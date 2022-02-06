using System;
using Microsoft.Maui.Graphics;

namespace ScottPlot;

// NOTE: bottom pixel should be a higher value than the top pixel
public class PixelRect
{
    public readonly float Bottom;
    public readonly float Top;
    public readonly float Right;
    public readonly float Left;
    public PixelSize Size => new PixelSize(Width, Height);

    public float Width => Right - Left;
    public float Height => Bottom - Top;
    public bool HasPositiveArea => (Width > 0) && (Height > 0);

    public PointF LocationF => new(Left, Top);
    public SizeF SizeF => new(Width, Height);
    public RectangleF RectangleF => new(LocationF, SizeF);

    public Pixel TopLeft => new(Left, Top);
    public Pixel TopRight => new(Right, Top);
    public Pixel BottomLeft => new(Left, Bottom);
    public Pixel BottomRight => new(Right, Bottom);

    public PixelRect(float left, float right, float bottom, float top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public PixelRect(PixelSize size)
    {
        Left = 0;
        Right = size.Width;
        Top = 0;
        Bottom = size.Height;
    }

    public PixelRect(PixelSize size, Pixel location)
    {
        Left = location.X;
        Right = location.X + size.Width;
        Top = location.Y;
        Bottom = location.Y + size.Height;
    }

    public PixelRect Contract(float left, float right, float bottom, float top)
    {
        return new PixelRect(Left + left, Right - right, Bottom - bottom, Top + top);
    }

    public PixelRect Expand(float left, float right, float bottom, float top)
    {
        return new PixelRect(Left - left, Right + right, Bottom + bottom, Top - top);
    }
}
