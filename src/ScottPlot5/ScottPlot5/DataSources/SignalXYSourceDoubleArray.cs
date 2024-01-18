namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray : ISignalXYSource
{
    readonly double[] Xs;
    readonly double[] Ys;

    public double XOffset { get; set; } = 0;
    public double YOffset { get; set; } = 0;
    public int MinimumIndex { get; set; } = 0;
    public int MaximumIndex { get; set; }

    public SignalXYSourceDoubleArray(double[] xs, double[] ys)
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
        double xMin = Xs[MinimumIndex] + XOffset;
        double xMax = Xs[MaximumIndex] + XOffset;
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
            InterpolateBefore(rp, points);

        if (PointAfter.Length > 0)
            InterpolateAfter(rp, points);

        return points;
    }

    /// <summary>
    /// Return the vertical range covered by data between the given indices (inclusive)
    /// </summary>
    public CoordinateRange GetRangeY(int index1, int index2)
    {
        double min = Ys[index1];
        double max = Ys[index1];

        for (int i = index1; i <= index2; i++)
        {
            min = Math.Min(Ys[i], min);
            max = Math.Max(Ys[i], max);
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
        int index = Array.BinarySearch(Xs, indexRange.Min, indexRange.Length, x - XOffset);

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

        yield return new Pixel(xPixel, axes.GetPixelY(Ys[startIndex] + YOffset)); // enter

        if (pointsInRange > 1)
        {
            CoordinateRange yRange = GetRangeY(startIndex, endIndex - 1);
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
            yield return new Pixel(xPixel, axes.GetPixelY(Ys[endIndex - 1] + YOffset)); // exit
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
            float beforeX = axes.GetPixelX(Xs[pointBeforeIndex - 1] + XOffset);
            float beforeY = axes.GetPixelY(Ys[pointBeforeIndex - 1] + YOffset);
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
            float afterX = axes.GetPixelX(Xs[pointAfterIndex] + XOffset);
            float afterY = axes.GetPixelY(Ys[pointAfterIndex] + YOffset);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], pointAfterIndex);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }

    /// <summary>
    /// If the point to the left of the graph is extremely far outside the data area, 
    /// modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    private void InterpolateBefore(RenderPack rp, Pixel[] pixels)
    {
        if (pixels.Length <= 2)
            return;

        Pixel lastOutsidePoint = pixels[0];
        Pixel firstInsidePoint = pixels[1];

        if (lastOutsidePoint.X != firstInsidePoint.X)
        {
            float x0 = -1 + rp.DataRect.Left;
            float yDelta = lastOutsidePoint.Y - firstInsidePoint.Y;
            float xDelta1 = x0 - firstInsidePoint.X;
            float xDelta2 = lastOutsidePoint.X - firstInsidePoint.X;
            float y0 = firstInsidePoint.Y + yDelta * xDelta1 / xDelta2;
            pixels[0] = new Pixel(x0, y0);
        }
    }

    /// <summary>
    /// If the point to the right of the graph is extremely far outside the data area, 
    /// modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    private void InterpolateAfter(RenderPack rp, Pixel[] pixels)
    {
        if (pixels.Length <= 2)
            return;

        Pixel lastInsidePoint = pixels[pixels.Length - 2];
        Pixel firstOutsidePoint = pixels[pixels.Length - 1];

        if (firstOutsidePoint.X != lastInsidePoint.X)
        {
            float x1 = rp.DataRect.Width + rp.DataRect.Left;
            float yDelta = firstOutsidePoint.Y - lastInsidePoint.Y;
            float xDelta1 = x1 - lastInsidePoint.X;
            float xDelta2 = firstOutsidePoint.X - lastInsidePoint.X;
            float y1 = lastInsidePoint.Y + yDelta * xDelta1 / xDelta2;
            pixels[pixels.Length - 1] = new Pixel(x1, y1);
        }
    }
}
