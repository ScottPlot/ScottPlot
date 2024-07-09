using ScottPlot.PolarAxes;

namespace ScottPlot;

/// <summary>
/// Circular axis in polar coordinates
/// <para>Concentric circles centered at the origin, sometimes it may not be round</para>
/// </summary>
public interface ICircularAxis : IPolarAxis
{
    /// <summary>
    /// Axis line displayed on the plot
    /// </summary>
    CircularAxisLine[] AxisLines { get; }
}
