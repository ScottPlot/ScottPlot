using ScottPlot.PolarAxes;

namespace ScottPlot;

/// <summary>
/// Radial axis in polar coordinates
/// <para>Axis extending outward from the origin</para>
/// </summary>
public interface IRadialAxis : IPolarAxis
{
    /// <summary>
    /// Axis line displayed on the plot
    /// </summary>
    Spoke[] Spokes { get; }
}
