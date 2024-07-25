namespace ScottPlot;

public interface ISignalXYSource
{
    /// <summary>
    /// Number of values in the data source
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Position value (X when not Rotated) of the first data point
    /// </summary>
    double PositionOffset { get; set; }

    /// <summary>
    /// Multiply Position values (X when not Rotated) by this scale factor (before applying offset)
    /// </summary>
    double PositionScale { get; set; }

    /// <summary>
    /// Shift Amplitude  (Y when not Rotated) of all values by this amount
    /// </summary>
    double AmplitudeOffset { get; set; }

    /// <summary>
    /// Multiply Amplitude values  (Y when not Rotated) by this scale factor (before applying offset)
    /// </summary>
    public double AmplitudeScale { get; set; }

    /// <summary>
    /// Do not display data below this index
    /// </summary>
    public int MinimumIndex { get; set; }

    /// <summary>
    /// Do not display data above this index
    /// </summary>
    public int MaximumIndex { get; set; }

    /// <summary>
    /// If enabled, Positions will be vertical and Amplitudes will be horizontal.
    /// </summary>
    public bool Rotated { get; set; }

    /// <summary>
    /// Return the axis limits covered by these data
    /// </summary>
    AxisLimits GetAxisLimits();

    /// <summary>
    /// Return pixels to render to display this signal.
    /// May return one extra point on each side of the plot outside the data area.
    /// </summary>
    Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes, ConnectStyle connectStyle);

    /// <summary>
    /// Return the point nearest a specific location given the X/Y pixel scaling information from a previous render.
    /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
    /// </summary>
    DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);

    /// <summary>
    /// Return the point nearest a specific X location given the X/Y pixel scaling information from a previous render.
    /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
    /// </summary>
    DataPoint GetNearestPosition(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);
}
