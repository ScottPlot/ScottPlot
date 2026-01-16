namespace ScottPlot.Markers;

internal class TriangleWithLineRight: IMarker
{
    public void Render(SKCanvas canvas, Paint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float halfWidth = size / 2;

        //Horizontal line
        SKPath line = new();
        line.MoveTo(center.X + halfWidth, center.Y);
        line.LineTo(center.X - halfWidth, center.Y);
        Drawing.DrawPath(canvas, paint, line, markerStyle.LineStyle);

        //Triangle
        var triangleHeight = (float)(size * .8);
        var triangleWidth = (float)(size * .5);
        var centerOfTriangle = center.MovedRight(halfWidth - (triangleWidth / 2));
        
        var left = centerOfTriangle.X - (triangleWidth / 2);
        var right = centerOfTriangle.X + (triangleWidth / 2);
        var top = centerOfTriangle.Y - (triangleHeight / 2);
        var bottom = centerOfTriangle.Y + (triangleHeight / 2);

        SKPoint[] pointsList =
        {
            new (right, top),
            new (left, centerOfTriangle.Y),
            new (right, bottom),
        };

        SKPath path = new();
        path.AddPoly(pointsList);

        PixelRect rect = new(left, right, bottom, top);
        Drawing.FillPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
