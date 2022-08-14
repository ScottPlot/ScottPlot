using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Implement this interface to create a custom grid
/// </summary>
public interface IGrid
{
    public bool IsBeneathPlottables { get; set; }
    public void Render(SKSurface surface, PixelRect dataRect, Axis.IAxis axisView);
}
