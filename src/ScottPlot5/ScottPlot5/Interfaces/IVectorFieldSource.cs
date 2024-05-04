namespace ScottPlot;

public interface IVectorFieldSource
{
    public IReadOnlyList<RootedCoordinateVector> GetRootedVectors();

    /// <summary>
    /// Return the point nearest a specific location given the X/Y pixel scaling information from a previous render.
    /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
    /// </summary>
    DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);

    public CoordinateRange GetLimitsX();
    public CoordinateRange GetLimitsY();

    AxisLimits GetLimits();
    public int MinRenderIndex { get; set; }
    public int MaxRenderIndex { get; set; }
}
