namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray(double[] xs, double[] ys) : ISignalXYSource
{
    public double[] Xs = xs;
    public double[] Ys = ys;

    public int Length => Ys.Length;

    /// <summary>
    /// Return the horizontal span covered by these data
    /// </summary>
    public CoordinateRangeStruct GetRangeX()
    {
        return new CoordinateRangeStruct(Xs[0], Xs[Length - 1]);
    }

    /// <summary>
    /// Return the vertical span covered by these data
    /// </summary>
    public CoordinateRangeStruct GetRangeY()
    {
        return GetRangeY(0, Length - 1);
    }

    /// <summary>
    /// Return the axis limits covered by these data
    /// </summary>
    public AxisLimits GetAxisLimits()
    {
        CoordinateRangeStruct xRange = GetRangeX();
        CoordinateRangeStruct yRange = GetRangeY();
        return new AxisLimits(xRange, yRange);
    }

    /// <summary>
    /// Return the vertical range covered by data between the given indices (inclusive)
    /// </summary>
    public CoordinateRangeStruct GetRangeY(int index1, int index2)
    {
        double min = Ys[index1];
        double max = Ys[index1];

        for (int i = index1; i <= index2; i++)
        {
            min = Math.Min(Ys[i], min);
            max = Math.Max(Ys[i], max);
        }

        return new CoordinateRangeStruct(min, max);
    }

    /// <summary>
    /// Get the index associated with the given X position
    /// </summary>
    public int GetIndexX(double x)
    {
        IndexRange range = new(0, Xs.Length - 1);
        return GetIndex(x, range);
    }

    /// <summary>
    /// Get the index associated with the given X position limited to the given range
    /// </summary>
    public int GetIndex(double x, IndexRange indexRange)
    {
        int index = Array.BinarySearch(Xs, indexRange.Min, indexRange.Length, x);

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

        yield return new Pixel(xPixel, axes.GetPixelY(Ys[startIndex])); // enter

        if (pointsInRange > 1)
        {
            CoordinateRangeStruct yRange = GetRangeY(startIndex, endIndex - 1);
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
            yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
            yield return new Pixel(xPixel, axes.GetPixelY(Ys[endIndex - 1])); // exit
        }
    }

    /// <summary>
    /// Return pixels to render to display this signal.
    /// May return one extra point on each side of the plot outside the data area.
    /// </summary>
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
    /// If data is off to the screen to the left, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int firstIndex) GetFirstPoint(IAxes axes)
    {
        int pointBeforeIndex = GetIndexX(axes.XAxis.Min);

        if (pointBeforeIndex > 0)
        {
            float beforeX = axes.GetPixelX(Xs[pointBeforeIndex - 1]);
            float beforeY = axes.GetPixelY(Ys[pointBeforeIndex - 1]);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], pointBeforeIndex);
        }
        else
        {
            return ([], 0);
        }
    }

    /// <summary>
    /// If data is off to the screen to the right, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int lastIndex) GetLastPoint(IAxes axes)
    {
        int pointAfterIndex = GetIndexX(axes.XAxis.Max);

        if (pointAfterIndex <= Ys.Length - 1)
        {
            float afterX = axes.GetPixelX(Xs[pointAfterIndex]);
            float afterY = axes.GetPixelY(Ys[pointAfterIndex]);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], pointAfterIndex);
        }
        else
        {
            return ([], Length - 1);
        }
    }

    /// <summary>
    /// If the point to the left of the graph is extremely far outside the data area, 
    /// modify it using interpolation so it's closer to the data area to prevent render artifacts.
    /// </summary>
    private void InterpolateBefore(RenderPack rp, Pixel[] pixels)
    {
        if (pixels.Length >= 2)
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
        if (pixels.Length >= 2)
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
