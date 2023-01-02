namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as a collection of coordinates
/// </summary>
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
        foreach (Coordinates coordinate in Coordinates)
            rect.Expand(coordinate);
        return rect;
    }

    public CoordinateRange GetLimitsX()
    {
        return GetLimits().Rect.XRange;
    }

    public CoordinateRange GetLimitsY()
    {
        return GetLimits().Rect.YRange;
    }
}
