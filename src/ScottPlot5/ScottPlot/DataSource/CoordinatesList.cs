namespace ScottPlot.DataSource;

/// <summary>
/// This data source is a list of coordinates that users can modify
/// </summary>
public class CoordinatesList : List<Coordinates>, ICoordinateSource
{
    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        foreach (Coordinates point in this)
            rect.Expand(point);

        return rect;
    }

    public CoordinateRange GetLimitsX()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.XMin, rect.XMin);
    }

    public CoordinateRange GetLimitsY()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.YMin, rect.YMin);
    }
}
