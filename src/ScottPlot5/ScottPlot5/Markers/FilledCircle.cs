namespace ScottPlot.Markers;

internal class FilledCircle : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        Drawing.DrawCircle(canvas, center, radius, markerStyle.FillStyle, paint);
        Drawing.DrawCircle(canvas, center, radius, markerStyle.OutlineStyle, paint);
    }
}
