/* Most plot types will want to inherit this */

using ScottPlot.Axes;
using SkiaSharp;

namespace ScottPlot.Plottables;

public abstract class PlottableBase : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IXAxis? XAxis { get; set; } = null;
    public IYAxis? YAxis { get; set; } = null;

    public virtual AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public abstract void Render(SKSurface surface, PixelRect dataRect);
}
