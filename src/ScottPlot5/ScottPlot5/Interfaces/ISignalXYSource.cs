namespace ScottPlot;

public interface ISignalXYSource
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

    /// <summary>
    /// Do not display data below this index
    /// </summary>
    public int MinimumIndex { get; set; }

    /// <summary>
    /// Do not display data above this index
    /// </summary>
    public int MaximumIndex { get; set; }

    /// <summary>
    /// If enabled, Xs will be vertical and Ys will be horizontal.
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
    DataPoint GetNearestX(Coordinates location, RenderDetails renderInfo, float maxDistance = 15);
}
