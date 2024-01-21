namespace ScottPlot.DataSources;

public class SignalXYSourceGenericArray<TX, TY> : ISignalXYSource
{
    public TX[] Xs { get; set; }
    public TY[] Ys { get; set; }
    public bool Rotated
    {
        get => false;
        set => throw new NotImplementedException("rotation is not yet supported for generic SignalXY plots");
    }

    public double XOffset { get; set; } = 0;
    public double YOffset { get; set; } = 0;
    public int MinimumIndex { get; set; } = 0;
    public int MaximumIndex { get; set; }

    public SignalXYSourceGenericArray(TX[] xs, TY[] ys)
    {
        if (xs.Length != ys.Length)
        {
            throw new InvalidOperationException($"{nameof(xs)} and {nameof(ys)} must have equal length");
        }

        Xs = xs;
        Ys = ys;
        MaximumIndex = xs.Length - 1;
    }

    public AxisLimits GetAxisLimits()
    {
        double xMin = NumericConversion.GenericToDouble(Xs, MinimumIndex) + XOffset;
        double xMax = NumericConversion.GenericToDouble(Xs, MaximumIndex) + XOffset;
        CoordinateRange xRange = new(xMin, xMax);
        CoordinateRange yRange = GetRangeY(MinimumIndex, MaximumIndex);
        return new AxisLimits(xRange, yRange);
    }

    public Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes)
    {
        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPoint(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPoint(axes);
        IndexRange columnIndexRange = new(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
            .Select(pixelColumnIndex => GetColumnPixels(pixelColumnIndex, columnIndexRange, rp, axes))
            .SelectMany(x => x);

        // combine with one extra point before and after
        Pixel[] points = [.. PointBefore, .. VisiblePoints, .. PointAfter];

        // use interpolation at the edges to prevent points from going way off the screen
        if (PointBefore.Length > 0)
            SignalInterpolation.InterpolateBeforeX(rp, points);

        if (PointAfter.Length > 0)
            SignalInterpolation.InterpolateAfterX(rp, points);

        return points;
    }

    /// <summary>
    /// Return the vertical range covered by data between the given indices (inclusive)
    /// </summary>
    public CoordinateRange GetRangeY(int index1, int index2)
    {
        double min = NumericConversion.GenericToDouble(Ys, index1);
        double max = NumericConversion.GenericToDouble(Ys, index1);

        for (int i = index1; i <= index2; i++)
        {
            double value = NumericConversion.GenericToDouble(Ys, i);
            min = Math.Min(value, min);
            max = Math.Max(value, max);
        }

        return new CoordinateRange(min + YOffset, max + YOffset);
    }

    /// <summary>
    /// Get the index associated with the given X position
    /// </summary>
    public int GetIndexX(double x)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return GetIndex(x, range);
    }

    /// <summary>
    /// Get the index associated with the given X position limited to the given range
    /// </summary>
    public int GetIndex(double x, IndexRange indexRange)
    {
        NumericConversion.DoubleToGeneric(x - XOffset, out TX x2);
        int index = Array.BinarySearch(Xs, indexRange.Min, indexRange.Length, x2);

        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return index;
    }

    /// <summary>
    /// Given a pixel column, return the pixels to render its line.
    /// If the column contains no data, no pixels are returned.
    /// If the column contains one point, return that one pixel.
    /// If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    public IEnumerable<Pixel> GetColumnPixels(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float xPixel = pixelColumnIndex + rp.DataRect.Left;
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + unitsPerPixelX * pixelColumnIndex;
        double end = start + unitsPerPixelX;
        int startIndex = GetIndex(start, rng);
        int endIndex = GetIndex(end, rng);
        int pointsInRange = endIndex - startIndex;

        if (pointsInRange == 0)
        {
            yield break;
        }

        double yStart = NumericConversion.GenericToDouble(Ys, startIndex);
        yield return new Pixel(xPixel, axes.GetPixelY(yStart + YOffset)); // enter

        if (pointsInRange > 1)
        {
            double yEnd = NumericConversion.GenericToDouble(Ys, endIndex - 1);
            CoordinateRange yRange = GetRangeY(startIndex, endIndex - 1);
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
            yield return new Pixel(xPixel, axes.GetPixelY(yEnd) + YOffset); // exit
        }
    }

    /// <summary>
    /// If data is off to the screen to the left, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int firstIndex) GetFirstPoint(IAxes axes)
    {
        int pointBeforeIndex = GetIndexX(axes.XAxis.Min);

        if (pointBeforeIndex > MinimumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, pointBeforeIndex - 1) + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, pointBeforeIndex - 1) + YOffset;
            float beforeX = axes.GetPixelX(x);
            float beforeY = axes.GetPixelY(y);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], pointBeforeIndex);
        }
        else
        {
            return ([], MinimumIndex);
        }
    }

    /// <summary>
    /// If data is off to the screen to the right, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int lastIndex) GetLastPoint(IAxes axes)
    {
        int pointAfterIndex = GetIndexX(axes.XAxis.Max);

        if (pointAfterIndex <= MaximumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, pointAfterIndex) + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, pointAfterIndex) + YOffset;
            float afterX = axes.GetPixelX(x);
            float afterY = axes.GetPixelY(y);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], pointAfterIndex);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }
}
