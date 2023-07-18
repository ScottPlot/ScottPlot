namespace ScottPlot.Rendering;

/// <summary>
/// The renderer contains all logic for rendering a plot onto a surface
/// </summary>
public interface IRenderer
{
    RenderDetails Render(RenderPack rp);
}
