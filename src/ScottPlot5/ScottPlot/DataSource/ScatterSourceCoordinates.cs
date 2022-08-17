namespace ScottPlot.DataSource;

public class ScatterSourceCoordinates : IScatterSource
{
    private readonly IReadOnlyList<Coordinates> Coordinates;

    public ScatterSourceCoordinates(IReadOnlyList<Coordinates> coordinates)
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
        for (int i = 0; i < Coordinates.Count; i++)
            rect.Expand(Coordinates[i]);
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
