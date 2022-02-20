using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public interface IAxis
{
    Edge Edge { get; }
    TextLabel Label { get; }
    PixelSize GetSize(ICanvas canvas);
    void Draw(ICanvas canvas, PlotInfo info);
}