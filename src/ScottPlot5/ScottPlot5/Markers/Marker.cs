namespace ScottPlot.Markers;

public abstract class Marker : IMarker
{
    protected static void Rotate(SKPath path, Pixel center, Angle rotateAngle)
    {
        float degrees = (float)rotateAngle.Normalized.Degrees;
        if (degrees <= 0)
        {
            return;
        }

        SKMatrix matrix = SKMatrix.CreateIdentity()
            .PostConcat(SKMatrix.CreateRotationDegrees(degrees, center.X, center.Y));
        path.Transform(matrix);
    }

    public abstract void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle);
}
