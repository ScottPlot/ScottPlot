using Microsoft.Maui.Graphics;

namespace ScottPlot.Plottable;

public interface IPlottable
{
    void Draw(ICanvas canvas, PlotView view, PlotStyle style);
}