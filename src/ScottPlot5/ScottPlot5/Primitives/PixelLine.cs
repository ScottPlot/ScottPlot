namespace ScottPlot;

/// <summary>
/// Describes a straight line in pixel space
/// </summary>
public readonly struct PixelLine
{
    public readonly float X1;
    public readonly float X2;
    public readonly float Y1;
    public readonly float Y2;

    public Pixel Pixel1 => new(X1, Y1);

    public Pixel Pixel2 => new(X2, Y2);

    public PixelLine(float x1, float y1, float x2, float y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public PixelLine(Pixel px1, Pixel px2)
    {
        X1 = px1.X;
        Y1 = px1.Y;
        X2 = px2.X;
        Y2 = px2.Y;
    }

    public void Draw(SKCanvas canvas, SKPaint paint)
    {
        canvas.DrawLine(X1, Y1, X2, Y2, paint);
    }
}
