using ScottPlot.Renderable;
using ScottPlot.Space;

namespace ScottPlot.Plottable
{
    public interface IPlottable : IRenderable
    {
        int PointCount { get; }
        AxisLimits Limits { get; }
    }
}
