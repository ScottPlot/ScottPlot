namespace ScottPlot.Markers;

internal class HashTag : IMarker
{
    public bool Fill { get; set; } = false;
    public float LineWidth { get; set; } = 1;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        if (LineWidth == 0)
            return;

        float sixthOffset = size / 6;
        float halfOffset = size / 2;

        var path = new SKPath();
        // Vertical Lines
        path.MoveTo(center.X + sixthOffset, center.Y + halfOffset);
        path.LineTo(center.X + sixthOffset, center.Y - halfOffset);
        path.MoveTo(center.X - sixthOffset, center.Y + halfOffset);
        path.LineTo(center.X - sixthOffset, center.Y - halfOffset);

        // Horizontal Lines
        path.MoveTo(center.X + halfOffset, center.Y + sixthOffset);
        path.LineTo(center.X - halfOffset, center.Y + sixthOffset);
        path.MoveTo(center.X + halfOffset, center.Y - sixthOffset);
        path.LineTo(center.X - halfOffset, center.Y - sixthOffset);

        outline.ApplyToPaint(paint);
        paint.StrokeWidth = LineWidth;
        canvas.DrawPath(path, paint);
    }
}
