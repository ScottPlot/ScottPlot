namespace ScottPlot.PathStrategies;

using System.Linq;

/// <summary>
/// Connect points with Catmull-Rom cubic splines, see https://doi.org/10.1007/s42979-021-00770-x
/// NaN values will be skipped, producing a gap in the path.
/// </summary>
public class CubicSpline : IPathStrategy
{
    public double Tension = 6;

    public SKPath GetPath(IEnumerable<Pixel> pixels)
    {
        SKPath path = new();

        // Organize into groups of connected points
        IEnumerable<IEnumerable<Pixel>> segments = pixels.GroupBy(pixel => float.IsNaN(pixel.X) || float.IsNaN(pixel.Y))
                                                         .Where(group => group.Key == false);

        foreach (var segment in segments.Where(segment => segment.Take(2).Count() == 2))
        {
            // Reserve two empty elements at the front and at the back
            Pixel[] array = EnumerableExtensions.One(Pixel.NaN).Concat(segment)
                                                               .Concat(EnumerableExtensions.One(Pixel.NaN))
                                                               .ToArray();

            Pixel first = array[1];
            Pixel second = array[2];
            Pixel nextLast = array[array.Length - 3];
            Pixel last = array[array.Length - 2];

            bool closed = first == last;

            array[0] = closed ? nextLast : first - (second - first);
            array[array.Length - 1] = closed ? second : last + (last - nextLast);

            path.MoveTo(first.ToSKPoint());

            for (int i = 2; i < array.Length - 1; ++i)
            {
                Pixel p0 = array[i - 2];
                Pixel p1 = array[i - 1];
                Pixel p2 = array[i];
                Pixel p3 = array[i + 1];
                Pixel c1 = p1 + (p2 - p0) / (float)Tension;
                Pixel c2 = p2 - (p3 - p1) / (float)Tension;

                path.CubicTo(c1.ToSKPoint(), c2.ToSKPoint(), p2.ToSKPoint());
            }
        }

        return path;
    }
}
