namespace ScottPlot;

public interface IDataStreamer2Source
{
    /// <summary>
    /// X position of the first data point
    /// </summary>
    double XOffset { get; set; }

    /// <summary>
    /// Shift Y position of all values by this amount
    /// </summary>
    double YOffset { get; set; }

    /// <summary>
    /// Multiply Y values by this scale factor (before applying offset)
    /// </summary>
    public double YScale { get; set; }

    bool HasNewData { get; }
    bool WasRendered { get; set; }
    /// <summary>
    /// Return pixels to render to display this signal.
    /// May return one extra point on each side of the plot outside the data area.
    /// </summary>
    Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle);

    /// <summary>
    /// Return pixels to render to display this signal.
    /// May return one extra point on each side of the plot outside the data area.
    /// </summary>
    Pixel[] GetPixelsToDrawVertically(RenderPack rp, IAxes axes, ConnectStyle connectStyle);

    /// <summary>
    /// Return the point nearest a specific location given the X/Y pixel scaling information from a previous render.
    /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
    /// </summary>
    DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);

    /// <summary>
    /// Return the point nearest a specific X location given the X/Y pixel scaling information from a previous render.
    /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
    /// </summary>
    DataPoint GetNearestX(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);

    /// <summary>
    /// Add a data point to the source
    /// </summary>
    /// <param name="coordinates">Data point</param>
    void Add(Coordinates coordinates);

    /// <summary>
    /// Called after data logger has been rendered
    /// </summary>
    void OnRendered();

    IList<Coordinates> Coordinates { get; }
    CoordinateRange GetRangeX();
    CoordinateRange GetRangeY(CoordinateRange newRangeX);
}
