using ScottPlot.Renderable;
using ScottPlot.Space;

namespace ScottPlot.Plottable
{
    public interface IPlottable : IRenderable
    {
        int XAxisIndex { get; set; }
        int YAxisIndex { get; set; }
        int PointCount { get; }
        AxisLimits2D Limits { get; }
    }
}
