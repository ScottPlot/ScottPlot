namespace ScottPlot;

/// <summary>
/// Implement this interface to create a custom grid
/// </summary>
public interface IGrid
{
    bool IsBeneathPlottables { get; set; }
    void Render(RenderPack rp);
    void Replace(IXAxis xAxis);
    void Replace(IYAxis yAxis);
}
