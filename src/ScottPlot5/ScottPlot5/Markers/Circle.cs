namespace ScottPlot.Markers;

internal class Circle : IMarker
{
    private readonly bool isOutlined;
    public Circle(bool isOutlined)
    {
        this.isOutlined = isOutlined;
    }

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float radius = size / 2;

        if (isOutlined == false)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }

        if (outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }
    }
}
