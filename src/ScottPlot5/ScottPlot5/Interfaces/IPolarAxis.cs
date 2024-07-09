namespace ScottPlot;

public interface IPolarAxis
{
    AxisLimits GetAxisLimits();

    /// <summary>
    /// Render axis
    /// </summary>
    /// <param name="rp">Render data</param>
    /// <param name="axes">Providing logic for coordinate/pixel conversions</param>
    void Render(RenderPack rp, IAxes axes);
}
