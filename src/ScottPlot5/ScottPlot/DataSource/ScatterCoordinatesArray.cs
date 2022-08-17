namespace ScottPlot.DataSource;

public class ScatterCoordinatesArray : IScatterSource
{
    public readonly Coordinates[] Coordinates;

    public ScatterCoordinatesArray(Coordinates[] coordinates)
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

        for (int i = 0; i < Coordinates.Length; i++)
        {
            rect.Expand(Coordinates[i]);
        }

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
