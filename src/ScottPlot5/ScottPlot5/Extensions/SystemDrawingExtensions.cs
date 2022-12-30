namespace ScottPlot.Extensions;

public static class SystemDrawingExtensions
{
    public static Color ToColor(this System.Drawing.Color color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }

    public static PixelRect ToPixelRect(this System.Drawing.Rectangle rect)
    {
        return new PixelRect(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public static PixelRect ToPixelRect(this System.Drawing.RectangleF rect)
    {
        return new PixelRect(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public static PixelSize ToPixelSize(this System.Drawing.Size size)
    {
        return new PixelSize(size.Width, size.Height);
    }

    public static PixelSize ToPixelSize(this System.Drawing.SizeF size)
    {
        return new PixelSize(size.Width, size.Height);
    }

    public static Pixel ToPixel(this System.Drawing.Point point)
    {
        return new Pixel(point.X, point.Y);
    }

    public static Pixel ToPixel(this System.Drawing.PointF point)
    {
        return new Pixel(point.X, point.Y);
    }
}
