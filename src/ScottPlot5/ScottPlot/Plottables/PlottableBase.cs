/* Most plot types will want to inherit this */

using ScottPlot.Axis.AxisTranslation;
using SkiaSharp;

namespace ScottPlot.Plottables;

public abstract class PlottableBase : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IXAxisTranslator? XAxis { get; set; } = null;
    public IYAxisTranslator? YAxis { get; set; } = null;

    public virtual AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public abstract void Render(SKSurface surface, PixelRect dataRect);
}
