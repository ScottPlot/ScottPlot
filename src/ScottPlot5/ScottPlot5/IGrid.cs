using ScottPlot.Axis;

namespace ScottPlot;

/// <summary>
/// Implement this interface to create a custom grid
/// </summary>
public interface IGrid
{
    bool IsBeneathPlottables { get; set; }
    void Render(SKSurface surface, PixelRect dataRect);
    void Replace(IXAxis xAxis);
    void Replace(IYAxis yAxis);
}
