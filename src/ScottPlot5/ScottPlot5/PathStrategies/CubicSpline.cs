namespace ScottPlot.PathStrategies;

using System.Linq;

/// <summary>
/// Connect points with Catmull-Rom cubic splines, see https://doi.org/10.1007/s42979-021-00770-x
/// NaN values will be skipped, producing a gap in the path.
/// </summary>
public class CubicSpline : IPathStrategy
{
    public double Tension { get; set; } = 1.0f;

    /// <summary>
    /// Organize into segments of connected points padded with empty points at the front and back
    /// </summary>
    /// <param name="pixels">Pixels</param>
    /// <returns>IEnumerable of arrays representing the segments</returns>
    private IEnumerable<Pixel[]> GetSegments(IEnumerable<Pixel> pixels)
    {
        // Reserve an empty element at the front
        List<Pixel> segment = [Pixel.NaN];

        foreach (var pixel in pixels)
        {
            if (float.IsNaN(pixel.X) || float.IsNaN(pixel.Y))
            {
                if (segment.Count > 1)
                {
                    // And one at the back
                    segment.Add(Pixel.NaN);
                    yield return segment.ToArray();

                    segment.Clear();
                    segment.Add(Pixel.NaN);
                }
            }
            else
            {
                segment.Add(pixel);
            }
        }

        if (segment.Count > 1)
        {
            segment.Add(Pixel.NaN);
            yield return segment.ToArray();
        }
    }

    public SKPath GetPath(IEnumerable<Pixel> pixels)
    {
        SKPath path = new();

        // Organize into segments of connected points
        foreach (var segment in GetSegments(pixels).Where(seg => seg.Length >= 4))
        {
            Pixel first = segment[1];
            Pixel second = segment[2];
            Pixel nextLast = segment[^3];
            Pixel last = segment[^2];

            bool closed = first == last;

            // Fill in the empty padded points
            segment[0] = closed ? nextLast : first;
            segment[^1] = closed ? second : last;

            path.MoveTo(first.ToSKPoint());

            for (int i = 2; i < segment.Length - 1; ++i)
            {
                Pixel p0 = segment[i - 2];
                Pixel p1 = segment[i - 1];
                Pixel p2 = segment[i];
                Pixel p3 = segment[i + 1];
                Pixel c1 = p1 + (p2 - p0) / (6.0f * (float)Tension);
                Pixel c2 = p2 - (p3 - p1) / (6.0f * (float)Tension);

                path.CubicTo(c1.ToSKPoint(), c2.ToSKPoint(), p2.ToSKPoint());
            }
        }

        return path;
    }
}
