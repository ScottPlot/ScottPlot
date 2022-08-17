namespace ScottPlot.DataSource;

/// <summary>
/// This data source has Xs and Ys defined in fixed-length arrays.
/// Changes made to the contents of the arrays will appear when the plot is rendered.
/// </summary>
public class CoordinatesArray : ICoordinateSource
{
    private readonly Coordinates[] Coordinates;

    public int Count => Coordinates.Length;

    public CoordinatesArray(Coordinates[] coordinates)
    {
        Coordinates = coordinates;
    }

    public Coordinates this[int index]
    {
        get => Coordinates[index];
        set => Coordinates[index] = value;
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        for (int i = 0; i < Count; i++)
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

    public IEnumerator<Coordinates> GetEnumerator()
    {
        int i = 0;
        while (i < Count)
        {
            i++;
            yield return Coordinates[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
