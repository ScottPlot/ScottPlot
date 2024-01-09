namespace ScottPlot;

public interface ISignalXYSource
{
    // TODO: support XOffset and YOffset

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
