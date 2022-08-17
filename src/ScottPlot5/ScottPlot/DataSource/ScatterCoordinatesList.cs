namespace ScottPlot.DataSource;

public class ScatterCoordinatesList : IScatterSource
{
    public readonly List<Coordinates> Coordinates;

    public ScatterCoordinatesList(List<Coordinates> coordinates)
    {
        Coordinates = coordinates;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Coordinates;
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        foreach (Coordinates coordinate in Coordinates)
            rect.Expand(coordinate);

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
