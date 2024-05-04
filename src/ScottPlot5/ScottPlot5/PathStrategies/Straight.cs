namespace ScottPlot.PathStrategies;

/// <summary>
/// Connect points with straight lines.
/// NaN values will be skipped, producing a gap in the path.
/// </summary>
internal class Straight : IPathStrategy
{
    public SKPath GetPath(IEnumerable<Pixel> pixels)
    {
        SKPath path = new();

        bool move = true;

        foreach (var pixel in pixels)
        {
            if (float.IsNaN(pixel.X) || float.IsNaN(pixel.Y))
            {
                move = true;
            }
            else if (move)
            {
                path.MoveTo(pixel.ToSKPoint());
                move = false;
            }
            else
            {
                path.LineTo(pixel.ToSKPoint());
            }
        }

        return path;
    }
}
