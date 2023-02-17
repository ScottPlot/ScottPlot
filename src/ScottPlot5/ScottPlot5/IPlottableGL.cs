namespace ScottPlot;

/// <summary>
/// This interface is applied to plottables which can be rendered directly on the GPU using an OpenGL shader
/// </summary>
public interface IPlottableGL
{
    GRContext GRContext { get; }
    void Render(SKSurface surface, GRContext context);
    void FinishRender(GRContext context);
}
