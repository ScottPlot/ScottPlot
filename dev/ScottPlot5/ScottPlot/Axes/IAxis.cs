using Microsoft.Maui.Graphics;

namespace ScottPlot.Axes;

public interface IAxis
{
    Edge Edge { get; }
    Orientation Orientation { get; }
    TextLabel Label { get; }
    ITickFactory TickFactory { get; set; }
    float Measure(ICanvas canvas, Tick[]? ticks);
    void DrawAxisLabel(ICanvas canvas, PlotConfig info, float size, float offset);
    void DrawTicks(ICanvas canvas, PlotConfig info, Tick[] allTicks, float offset);
    void DrawGridLines(ICanvas canvas, PlotConfig info, Tick[] ticks);
    void DrawSpine(ICanvas canvas, PlotConfig config, float offset);
}