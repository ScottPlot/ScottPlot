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
    Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes);
}
