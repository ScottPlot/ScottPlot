namespace ScottPlot.DataSources;

public class SignalXYSourceGenericList<Tx, Ty> : ISignalXYSource, IDataSource, IGetNearest
{
    private readonly IReadOnlyList<Tx> Xs;
    private readonly IReadOnlyList<Ty> Ys;

    public int Count => Math.Min(Xs.Count, Ys.Count);

    public bool Rotated { get; set; } = false;

    public double XOffset { get; set; } = 0;
    public double YOffset { get; set; } = 0;
    public double YScale { get; set; } = 1;
    public double XScale { get; set; } = 1;

    public int MinimumIndex { get; set; } = 0;

    private int? _maximumIndex = null;
    public int MaximumIndex
    {
        get => _maximumIndex.GetValueOrDefault(Count - 1);
        set
        {
            _maximumIndex = value;
        }
    }

    bool IDataSource.PreferCoordinates => false;
    int IDataSource.Length => Xs.Count;
    int IDataSource.MinRenderIndex => MinimumIndex;
    int IDataSource.MaxRenderIndex => MaximumIndex;

    public bool UsePixelOverlap { get; } = false; // https://github.com/ScottPlot/ScottPlot/issues/3665

    public SignalXYSourceGenericList(IReadOnlyList<Tx> xs, IReadOnlyList<Ty> ys)
    {
        if (xs.Count != ys.Count)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");
        }

        Xs = xs;
        Ys = ys;
    }

    public AxisLimits GetAxisLimits()
    {
        if (Xs.Count == 0)
            return AxisLimits.NoLimits;

        double xMin = NumericConversion.GenericToDouble(Xs, MinimumIndex) * XScale + XOffset;
        double xMax = NumericConversion.GenericToDouble(Xs, MaximumIndex) * XScale + XOffset;
        CoordinateRange xRange = new(xMin, xMax);
        CoordinateRange yRange = GetRangeY(MinimumIndex, MaximumIndex);
        return Rotated
            ? new AxisLimits(yRange, xRange)
            : new AxisLimits(xRange, yRange);
    }

    public Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        return Rotated
            ? GetPixelsToDrawVertically(rp, axes, connectStyle)
            : GetPixelsToDrawHorizontally(rp, axes, connectStyle);
    }

    public Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointX(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointX(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        if (visibleRange.IsValid && NumericConversion.GenericToDouble(Xs, dataIndexFirst) > NumericConversion.GenericToDouble(Xs, dataIndexLast))
            throw new InvalidDataException("Xs must contain only ascending values. " +
                $"The value at index {dataIndexFirst} ({Xs[dataIndexFirst]}) is greater than the value at index {dataIndexLast} ({Xs[dataIndexLast]})");

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = visibleRange.Length <= 0
            ? []
            : Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
            .Select(pixelColumnIndex => GetColumnPixelsX(pixelColumnIndex, visibleRange, rp, axes))
            .SelectMany(x => x);

        Pixel[] leftOutsidePoint = PointBefore, rightOutsidePoint = PointAfter;
        if (axes.XAxis.Range.Span < 0)
        {
            leftOutsidePoint = PointAfter;
            rightOutsidePoint = PointBefore;
        }

        // combine with one extra point before and after
        Pixel[] points = [.. leftOutsidePoint, .. VisiblePoints, .. rightOutsidePoint];

        // use interpolation at the edges to prevent points from going way off the screen
        if (leftOutsidePoint.Length > 0)
            SignalInterpolation.InterpolateBeforeX(rp, points, connectStyle);

        if (rightOutsidePoint.Length > 0)
            SignalInterpolation.InterpolateAfterX(rp, points, connectStyle);

        return points;
    }

    public Pixel[] GetPixelsToDrawVertically(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointY(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointY(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        if (visibleRange.IsValid && NumericConversion.GenericToDouble(Xs, dataIndexFirst) > NumericConversion.GenericToDouble(Xs, dataIndexLast))
            throw new InvalidDataException("Xs must contain only ascending values. " +
                $"The value at index {dataIndexFirst} ({Xs[dataIndexFirst]}) is greater than the value at index {dataIndexLast} ({Xs[dataIndexLast]})");

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = visibleRange.Length <= 0
            ? []
            : Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Height))
            .Select(pixelRowIndex => GetColumnPixelsY(pixelRowIndex, visibleRange, rp, axes))
            .SelectMany(x => x);

        Pixel[] bottomOutsidePoint = PointBefore, topOutsidePoint = PointAfter;
        if (axes.YAxis.Range.Span < 0)
        {
            bottomOutsidePoint = PointAfter;
            topOutsidePoint = PointBefore;
        }

        // combine with one extra point before and after
        Pixel[] points = [.. bottomOutsidePoint, .. VisiblePoints, .. topOutsidePoint];

        // use interpolation at the edges to prevent points from going way off the screen
        if (bottomOutsidePoint.Length > 0)
            SignalInterpolation.InterpolateBeforeY(rp, points, connectStyle);

        if (topOutsidePoint.Length > 0)
            SignalInterpolation.InterpolateAfterY(rp, points, connectStyle);

        return points;
    }

    /// <summary>
    /// Return the vertical range covered by data between the given indices (inclusive)
    /// </summary>
    public CoordinateRange GetRangeY(int index1, int index2)
    {
        double min = NumericConversion.GenericToDouble(Ys, index1);
        double max = NumericConversion.GenericToDouble(Ys, index1);

        var minIndex = Math.Min(index1, index2);
        var maxIndex = Math.Max(index1, index2);

        for (int i = minIndex; i <= maxIndex; i++)
        {
            double value = NumericConversion.GenericToDouble(Ys, i);
            min = Math.Min(value, min);
            max = Math.Max(value, max);
        }

        return new CoordinateRange(min * YScale + YOffset, max * YScale + YOffset);
    }

    /// <summary>
    /// Get the index associated with the given X position
    /// </summary>
    public int GetIndex(double x)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return GetIndex(x, range);
    }

    /// <summary>
    /// Get the index associated with the given X position limited to the given range
    /// </summary>
    public int GetIndex(double x, IndexRange indexRange)
    {
        var (_, index) = SearchIndex(x, indexRange);
        return index;
    }

    /// <summary>
    /// Given a pixel column, return the pixels to render its line.
    /// If the column contains no data, no pixels are returned.
    /// If the column contains one point, return that one pixel.
    /// If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    public IEnumerable<Pixel> GetColumnPixelsX(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float xPixel = pixelColumnIndex + rp.DataRect.Left;
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + unitsPerPixelX * pixelColumnIndex;
        double end = start + unitsPerPixelX;

        if (UsePixelOverlap)
        {
            end += unitsPerPixelX * .01;
        }

        var (startIndex, _) = SearchIndex(start, rng);
        var (endIndex, _) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;
        double yStart = NumericConversion.GenericToDouble(Ys, firstIndex) * YScale + YOffset;
        double yEnd = NumericConversion.GenericToDouble(Ys, lastIndex) * YScale + YOffset;

        yield return new Pixel(xPixel, axes.GetPixelY(yStart)); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(firstIndex, lastIndex); //YOffset is added in GetRangeY

            if (yStart > yEnd)
            { //signal amplitude is decreasing, so we'll return the maximum before the minimum
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
            }
            else
            { //signal amplitude is increasing, so we'll return the minimum before the maximum
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
            }
        }

        if (pointsInRange > 1)
        {
            yield return new Pixel(xPixel, axes.GetPixelY(yEnd)); // exit
        }
    }

    /// <summary>
    /// Given a pixel column, return the pixels to render its line.
    /// If the column contains no data, no pixels are returned.
    /// If the column contains one point, return that one pixel.
    /// If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    public IEnumerable<Pixel> GetColumnPixelsY(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        // here rowColumnIndex will count upwards from the bottom, but pixels are measured from the top of the plot
        float yPixel = rp.DataRect.Bottom - pixelColumnIndex;
        double unitsPerPixelY = axes.YAxis.Height / rp.DataRect.Height;
        double start = axes.YAxis.Min + unitsPerPixelY * pixelColumnIndex;
        double end = start + unitsPerPixelY;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelY * .01;
        end += overlap;

        var (startIndex, _) = SearchIndex(start, rng);
        var (endIndex, _) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;
        double yStart = NumericConversion.GenericToDouble(Ys, firstIndex) * YScale + YOffset;
        double yEnd = NumericConversion.GenericToDouble(Ys, lastIndex) * YScale + YOffset;

        yield return new Pixel(axes.GetPixelX(yStart), yPixel); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(firstIndex, lastIndex); //YOffset is added in GetRangeY

            if (yStart > yEnd)
            { //signal amplitude is decreasing, so we'll return the maximum before the minimum
                yield return new Pixel(axes.GetPixelX(yRange.Max), yPixel); // max
                yield return new Pixel(axes.GetPixelX(yRange.Min), yPixel); // min
            }
            else
            { //signal amplitude is increasing, so we'll return the minimum before the maximum
                yield return new Pixel(axes.GetPixelX(yRange.Min), yPixel); // min
                yield return new Pixel(axes.GetPixelX(yRange.Max), yPixel); // max
            }
        }

        if (pointsInRange > 1)
        {
            yield return new Pixel(axes.GetPixelX(yEnd), yPixel); // exit
        }
    }

    /// <summary>
    /// If data is off to the screen to the left, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointX(IAxes axes)
    {
        if (Xs.Count == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Min : axes.XAxis.Max); // if axis is reversed first index will on the right limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, firstPointIndex - 1) * XScale + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, firstPointIndex - 1) * YScale + YOffset;
            float beforeX = axes.GetPixelX(x);
            float beforeY = axes.GetPixelY(y);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], firstPointIndex);
        }
        else
        {
            return ([], MinimumIndex);
        }
    }

    /// <summary>
    /// If data is off to the screen to the bottom, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointY(IAxes axes)
    {
        if (Xs.Count == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Min : axes.YAxis.Max); // if axis is reversed first index will on the top limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, firstPointIndex - 1) * XScale + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, firstPointIndex - 1) * YScale + YOffset;
            float beforeX = axes.GetPixelX(y);
            float beforeY = axes.GetPixelY(x);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], firstPointIndex);
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
    private (Pixel[] pointAfter, int lastIndex) GetLastPointX(IAxes axes)
    {
        if (Xs.Count == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Max : axes.XAxis.Min); // if axis is reversed last index will on the left limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, lastPointIndex) * XScale + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, lastPointIndex) * YScale + YOffset;
            float afterX = axes.GetPixelX(x);
            float afterY = axes.GetPixelY(y);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], lastPointIndex - 1);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }

    /// <summary>
    /// If data is off to the screen to the top, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointAfter, int lastIndex) GetLastPointY(IAxes axes)
    {
        if (Xs.Count == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Max : axes.YAxis.Min); // if axis is reversed last index will on the bottom limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            double x = NumericConversion.GenericToDouble(Xs, lastPointIndex) * XScale + XOffset;
            double y = NumericConversion.GenericToDouble(Ys, lastPointIndex) * YScale + YOffset;
            float afterX = axes.GetPixelX(y);
            float afterY = axes.GetPixelY(x);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], lastPointIndex - 1);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }

    /// <summary>
    /// Search the index associated with the given X position
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return SearchIndex(x, range);
    }

    /// <summary>
    /// Search the index associated with the given X position limited to the given range
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x, IndexRange indexRange)
    {
        NumericConversion.DoubleToGeneric((x - XOffset) / XScale, out Tx x2);
        int index = SignalXYSourceGenericList<Tx, Ty>.BinarySearch(Xs, indexRange.Min, indexRange.Max, x2);

        // If x is not exactly matched to any value in Xs, BinarySearch returns a negative number. We can bitwise negation to obtain the position where x would be inserted (i.e., the next highest index).
        // If x is below the min Xs, BinarySearch returns -1. Here, bitwise negation returns 0 (i.e., x would be inserted at the first index of the array).
        // If x is above the max Xs, BinarySearch returns -maxIndex. Bitwise negation of this value returns maxIndex + 1 (i.e., the position after the last index). However, this index is beyond the array bounds, so we return the final index instead.
        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return (SearchedPosition: index, LimitedIndex: index > indexRange.Max ? indexRange.Max : index);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestFast(this, mouseLocation, renderInfo, maxDistance);

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestXFast(this, mouseLocation, renderInfo, maxDistance);

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation)
    {
        return Rotated
            ? GetIndex(mouseLocation.Y)
            : GetIndex(mouseLocation.X);
    }

    Coordinates IDataSource.GetCoordinate(int index)
    {
        double x = NumericConversion.GenericToDouble(Xs, index);
        double y = NumericConversion.GenericToDouble(Ys, index);
        return Rotated ? new Coordinates(y, x) : new Coordinates(x, y);
    }

    Coordinates IDataSource.GetCoordinateScaled(int index)
    {
        double x = DataSourceUtilities.ScaleXY(Xs, index, XScale, XOffset);
        double y = DataSourceUtilities.ScaleXY(Ys, index, YScale, YOffset);
        return Rotated ? new Coordinates(y, x) : new Coordinates(x, y);
    }

    double IDataSource.GetX(int index)
    {
        return Rotated ?
            NumericConversion.GenericToDouble(Ys, index) :
            NumericConversion.GenericToDouble(Xs, index);
    }

    double IDataSource.GetXScaled(int index)
    {
        return Rotated ?
            DataSourceUtilities.ScaleXY(Ys, index, YScale, YOffset) :
            DataSourceUtilities.ScaleXY(Xs, index, XScale, XOffset);
    }

    double IDataSource.GetY(int index)
    {
        return Rotated ?
            NumericConversion.GenericToDouble(Xs, index) :
            NumericConversion.GenericToDouble(Ys, index);
    }

    double IDataSource.GetYScaled(int index)
    {
        return Rotated ?
            DataSourceUtilities.ScaleXY(Xs, index, XScale, XOffset) :
            DataSourceUtilities.ScaleXY(Ys, index, YScale, YOffset);
    }
    bool IDataSource.IsSorted() => true;

    private static int BinarySearch(IReadOnlyList<Tx> list, int lo, int hi, Tx value)
    {
        double valueAsDouble = NumericConversion.GenericToDouble(ref value);
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2; // This is just a trick to ensure we don't overflow, it's equivalent to (lo + hi) / 2

            Tx genericX = list[mid];
            double x = NumericConversion.GenericToDouble(ref genericX);
            if (x == valueAsDouble)
                return mid;
            else if (valueAsDouble > x)
                lo = mid + 1;
            else
                hi = mid - 1;
        }

        return ~lo; // We mimic the BCL implementation here by returning the ones-complement of the index where it would slot in and maintain sorted order
    }
}
