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
    public float XSpan => X2 - X1;
    public float YSpan => Y2 - Y1;
    public float Slope => (X1 == X2) ? float.NaN : YSpan / XSpan;
    public float SlopeRadians => (float)Math.Atan(Slope);
    public float SlopeDegrees => SlopeRadians * 180 / (float)Math.PI;
    public float AngleRadians => (float)Math.Atan2(XSpan, -YSpan);
    public float AngleDegrees => AngleRadians * 180 / (float)Math.PI;
    public float YIntercept => Y1 - Slope * X1;
    public float Length => (float)Math.Sqrt(XSpan * XSpan + YSpan * YSpan);
    public float DeltaX => X2 - X1;
    public float DeltaY => Y2 - Y1;

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

    public override string ToString()
    {
        return $"PixelLine from ({X1}, {Y1}) to ({X2}, {Y2})";
    }

    public PixelLine BackedUpBy(float distance)
    {
        float dX = distance * (float)Math.Cos(SlopeRadians);
        float dY = distance * (float)Math.Sin(SlopeRadians);
        return WithDelta(dX, dY);
    }

    public PixelLine WithDelta(float dx, float dy)
    {
        return new PixelLine(X1 + dx, Y1 + dy, X2 + dx, Y2 + dy);
    }

    /// <summary>
    /// Return the X position on the line at the given Y
    /// </summary>
    public float X(float y = 0)
    {
        float dX = Y1 - y;
        float x = X1 - dX * Slope;
        return x;
    }

    /// <summary>
    /// Return the Y position on the line at the given X
    /// </summary>
    public float Y(float x = 0)
    {
        float y = Slope * x + YIntercept;
        return y;
    }
}
