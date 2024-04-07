namespace ScottPlot;

/// <summary>
/// Strategy for generating a path that connects a collection of pixels
/// </summary>
public interface IPathStrategy
{
    public SKPath GetPath(IEnumerable<Pixel> pixels);
}
