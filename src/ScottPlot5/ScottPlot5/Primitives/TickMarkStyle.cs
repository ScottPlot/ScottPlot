namespace ScottPlot;

public class TickMarkStyle
{
    public float Length;
    public float Width;
    public bool Hairline;
    public Color Color;
    public bool AntiAlias;

    public void Render(SKCanvas canvas, SKPaint paint, PixelLine pxLine)
    {
        ApplyToPaint(paint);
        if (Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        Drawing.DrawLine(canvas, paint, pxLine);
    }

    public void ApplyToPaint(SKPaint paint)
    {
        paint.IsAntialias = AntiAlias;
        paint.IsStroke = true;
        paint.Color = Color.ToSKColor();
        paint.StrokeWidth = Hairline ? 1 : Width;
    }
}
