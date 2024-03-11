namespace ScottPlot.Markers;

internal class Diamond : IMarker
{
    private readonly bool isOutlined;
    public Diamond(bool isOutlined)
    {
        this.isOutlined = isOutlined;
    }
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float offset = size / 2;

        // 4 corners
        SKPoint[] pointsList = new SKPoint[]
        {
            new SKPoint(center.X + offset, center.Y),
            new SKPoint(center.X, center.Y + offset),
            new SKPoint(center.X - offset, center.Y),
            new SKPoint(center.X, center.Y - offset),

        };

        var path = new SKPath();
        path.AddPoly(pointsList);

        if (isOutlined == false)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawPath(path, paint);
        }

        if (outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawPath(path, paint);
        }
    }
}
