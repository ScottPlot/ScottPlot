namespace ScottPlot.Markers;

internal class CircleWithLineLeft : IMarker
{
    public void Render(SKCanvas canvas, Paint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        //Horizontal line
        SKPath path = new();
        path.MoveTo(center.X + radius, center.Y);
        path.LineTo(center.X - radius, center.Y);
        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);

        //Circle
        var circleCenter = center.MovedLeft(radius);
        var circleRadius = size / 4;
        Drawing.FillCircle(canvas, circleCenter, circleRadius, markerStyle.FillStyle, paint);
        Drawing.DrawCircle(canvas, circleCenter, circleRadius, markerStyle.OutlineStyle, paint);

    }
}
