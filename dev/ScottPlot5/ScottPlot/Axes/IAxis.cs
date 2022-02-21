using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public interface IAxis
{
    Edge Edge { get; }
    Orientation Orientation { get; }
    TextLabel Label { get; }
    ITickFactory TickFactory { get; set; }
    void Draw(ICanvas canvas, PlotConfig info);
}