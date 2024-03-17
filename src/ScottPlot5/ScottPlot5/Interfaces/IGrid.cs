namespace ScottPlot;

/// <summary>
/// Implement this interface to create a custom grid
/// </summary>
public interface IGrid
{
    bool IsVisible { get; set; }
    bool IsBeneathPlottables { get; set; }
    IXAxis XAxis { get; set; }
    IYAxis YAxis { get; set; }
    GridStyle XAxisStyle { get; set; }
    GridStyle YAxisStyle { get; set; }
    void Render(RenderPack rp);
}
