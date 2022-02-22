using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public interface IAxis
{
    Edge Edge { get; }
    Orientation Orientation { get; }
    TextLabel Label { get; }
    ITickFactory TickFactory { get; set; }
    float Size(ICanvas canvas, Tick[]? ticks);
    void Draw(ICanvas canvas, PlotConfig info, Tick[] ticks);
}