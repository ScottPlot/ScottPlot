namespace ScottPlot.Markers;

internal class Square : IMarker
{
    private readonly bool isOutlined;
    public Square(bool isOutlined)
    {
        this.isOutlined = isOutlined;
    }
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        PixelRect rect = new(center: center, radius: size / 2);

        if (isOutlined == false)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawRect(rect.ToSKRect(), paint);
        }

        if (outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }
}
