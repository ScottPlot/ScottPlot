using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A "plottable" is a type of "renderable" that draws custom data on the figure.
    /// </summary>
    public interface IPlottable : IRenderable
    {
        int PointCount { get; }
        LegendItem[] LegendItems { get; } // TODO: interface segregation
        AxisLimits2D GetLimits();
        string ErrorMessage(bool deepValidation = false); // TODO: interface segregation
    }
}
