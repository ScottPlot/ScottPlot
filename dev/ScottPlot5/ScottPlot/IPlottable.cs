using Microsoft.Maui.Graphics;

namespace ScottPlot;

public interface IPlottable
{
    void Draw(ICanvas canvas, PlotInfo layout);
}