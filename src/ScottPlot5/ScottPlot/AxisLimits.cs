namespace ScottPlot;

/// <summary>
/// This object represents the rectangular visible area on a 2D coordinate system.
/// It simply stores a <see cref="CoordinateRect"/> but has axis-related methods to act upon it.
/// </summary>
public struct AxisLimits
{
    public CoordinateRect Rect;
    public bool XHasBeenSet => !double.IsNaN(Rect.Width);
    public bool YHasBeenSet => !double.IsNaN(Rect.Height);

    public AxisLimits(CoordinateRect rect)
    {
        Rect = rect;
    }

    public AxisLimits(double xMin, double xMax, double yMin, double yMax)
    {
        Rect = new(xMin, xMax, yMin, yMax);
    }

    public static AxisLimits NoLimits => new(double.NaN, double.NaN, double.NaN, double.NaN);

    public void Expand(AxisLimits newLimits)
    {
        if (!XHasBeenSet)
        {
            Rect.XMin = newLimits.Rect.XMin;
            Rect.XMax = newLimits.Rect.YMax;
        }

        if (!YHasBeenSet)
        {
            Rect.YMin = newLimits.Rect.YMin;
            Rect.YMax = newLimits.Rect.YMax;
        }

        if (XHasBeenSet && newLimits.XHasBeenSet)
        {
            Rect.XMin = Math.Min(Rect.XMin, newLimits.Rect.XMin);
            Rect.XMax = Math.Max(Rect.XMax, newLimits.Rect.XMax);
        }

        if (YHasBeenSet & newLimits.YHasBeenSet)
        {
            Rect.YMin = Math.Min(Rect.YMin, newLimits.Rect.YMin);
            Rect.YMax = Math.Max(Rect.YMax, newLimits.Rect.YMax);
        }
    }

    public void ExpandX(double x)
    {
        if (!XHasBeenSet)
        {
            Rect.XMin = x;
            Rect.XMax = x;
        }

        if (XHasBeenSet && !double.IsNaN(x))
        {
            Rect.XMin = Math.Min(Rect.XMin, x);
            Rect.XMax = Math.Max(Rect.XMax, x);
        }
    }

    public void ExpandY(double y)
    {
        if (!YHasBeenSet)
        {
            Rect.YMin = y;
            Rect.YMax = y;
        }

        if (YHasBeenSet && !double.IsNaN(y))
        {
            Rect.YMin = Math.Min(Rect.YMin, y);
            Rect.YMax = Math.Max(Rect.YMax, y);
        }
    }

    public void Expand(Coordinates point)
    {
        ExpandX(point.X);
        ExpandY(point.Y);
    }

    public CoordinateRect WithPan(double deltaX, double deltaY)
    {
        return new CoordinateRect(Rect.XMin + deltaX, Rect.XMax + deltaX, Rect.YMin + deltaY, Rect.YMax + deltaY);
    }

    public CoordinateRect WithZoom(double fracX, double fracY)
    {
        return WithZoom(fracX, fracY, Rect.XCenter, Rect.YCenter);
    }

    public CoordinateRect WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
    {
        double spanLeftX = zoomToX - Rect.XMin;
        double spanRightX = Rect.XMax - zoomToX;
        double newMinX = zoomToX - spanLeftX / fracX;
        double newMaxX = zoomToX + spanRightX / fracX;

        double spanLeftY = zoomToY - Rect.YMin;
        double spanRightY = Rect.YMax - zoomToY;
        double newMinY = zoomToY - spanLeftY / fracY;
        double newMaxY = zoomToY + spanRightY / fracY;

        return new CoordinateRect(newMinX, newMaxX, newMinY, newMaxY);
    }
}
