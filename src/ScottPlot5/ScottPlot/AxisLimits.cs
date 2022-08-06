namespace ScottPlot;

/// <summary>
/// This object represents the rectangular visible area on a 2D coordinate system.
/// It simply stores a <see cref="CoordinateRect"/> but has axis-related methods to act upon it.
/// </summary>
public struct AxisLimits
{
    public CoordinateRect Rect;

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
        if (double.IsNaN(Rect.XMin))
        {
            Rect.XMin = newLimits.Rect.XMin;
        }
        else if (!double.IsNaN(newLimits.Rect.XMin))
        {
            Rect.XMin = Math.Min(Rect.XMin, newLimits.Rect.XMin);
        }

        if (double.IsNaN(Rect.XMax))
        {
            Rect.XMax = newLimits.Rect.XMax;
        }
        else if (!double.IsNaN(newLimits.Rect.XMax))
        {
            Rect.XMax = Math.Min(Rect.XMax, newLimits.Rect.XMax);
        }

        if (double.IsNaN(Rect.YMin))
        {
            Rect.YMin = newLimits.Rect.YMin;
        }
        else if (!double.IsNaN(newLimits.Rect.YMin))
        {
            Rect.YMin = Math.Min(Rect.YMin, newLimits.Rect.YMin);
        }

        if (double.IsNaN(Rect.YMax))
        {
            Rect.YMax = newLimits.Rect.YMax;
        }
        else if (!double.IsNaN(newLimits.Rect.YMax))
        {
            Rect.YMax = Math.Min(Rect.YMax, newLimits.Rect.YMax);
        }
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
