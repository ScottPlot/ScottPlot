namespace ScottPlot.Markers;

internal class OpenCircleWithDot : IMarker
{
    public void Render(SKCanvas canvas, SKPaintAndFont paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;
        Drawing.DrawCircle(canvas, center, radius, markerStyle.LineStyle, paint);

        float dotRadius = markerStyle.LineStyle.Width > 0
            ? markerStyle.LineStyle.Width
            : 1;

        Drawing.FillCircle(canvas, center, dotRadius, markerStyle.FillStyle, paint);
    }
}
