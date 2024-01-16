namespace ScottPlot;

/// <summary>
/// Implement this interface to create a custom grid
/// </summary>
public interface IGrid
{
    bool IsVisible { get; set; }
    bool IsBeneathPlottables { get; set; }
    void Render(RenderPack rp);
    void Replace(IXAxis xAxis); // TODO: remove this
    void Replace(IYAxis yAxis); // TODO: remove this
}
