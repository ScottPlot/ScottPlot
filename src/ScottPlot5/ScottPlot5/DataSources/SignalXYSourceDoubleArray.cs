﻿using System;

namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray : ISignalXYSource, IDataSource, IGetNearest
{
    readonly double[] Xs;
    readonly double[] Ys;
    public int Count => Xs.Length;

    public bool Rotated { get; set; } = false;

    public double XOffset { get; set; } = 0;
    public double YOffset { get; set; } = 0;
    public double YScale { get; set; } = 1;
    public double XScale { get; set; } = 1;


    public int MinimumIndex { get; set; } = 0;
    public int MaximumIndex { get; set; }

    bool IDataSource.PreferCoordinates => false;
    int IDataSource.Length => Xs.Length;
    int IDataSource.MinRenderIndex => MinimumIndex;
    int IDataSource.MaxRenderIndex => MaximumIndex;

    public SignalXYSourceDoubleArray(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");
        }

        Xs = xs;
        Ys = ys;
        MaximumIndex = xs.Length - 1;
    }

    public AxisLimits GetAxisLimits()
    {
        if (Xs.Length == 0)
            return AxisLimits.NoLimits;

        double xMin = Xs[MinimumIndex] * XScale + XOffset;
        double xMax = Xs[MaximumIndex] * XScale + XOffset;

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

    private Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointX(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointX(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        if (visibleRange.IsValid && (Xs[dataIndexFirst] > Xs[dataIndexLast]))
            throw new InvalidDataException("Xs must contain only ascending values. " +
                $"The value at index {dataIndexFirst} ({Xs[dataIndexFirst]}) is greater than the value at index {dataIndexLast} ({Xs[dataIndexLast]})");

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = visibleRange.Length <= 0
            ? []
            : Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
            .Select(pxColumn => GetColumnPixelsX(pxColumn, visibleRange, rp, axes))
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

    private Pixel[] GetPixelsToDrawVertically(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointY(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointY(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        if (visibleRange.IsValid && (Xs[dataIndexFirst] > Xs[dataIndexLast]))
            throw new InvalidDataException("Xs must contain only ascending values. " +
                $"The value at index {dataIndexFirst} ({Xs[dataIndexFirst]}) is greater than the value at index {dataIndexLast} ({Xs[dataIndexLast]})");

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = visibleRange.Length <= 0
            ? []
            : Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Height))
            .Select(pxRow => GetColumnPixelsY(pxRow, visibleRange, rp, axes))
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
    private CoordinateRange GetRangeY(int index1, int index2)
    {
        double min = Ys[index1];
        double max = Ys[index1];

        var minIndex = Math.Min(index1, index2);
        var maxIndex = Math.Max(index1, index2);

        for (int i = minIndex; i <= maxIndex; i++)
        {
            min = Math.Min(Ys[i], min);
            max = Math.Max(Ys[i], max);
        }

        return new CoordinateRange(min * YScale + YOffset, max * YScale + YOffset);
    }

    /// <summary>
    /// Get the index associated with the given X position
    /// </summary>
    private int GetIndex(double x)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return GetIndex(x, range);
    }

    /// <summary>
    /// Get the index associated with the given X position limited to the given range
    /// </summary>
    private int GetIndex(double x, IndexRange indexRange)
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
    private IEnumerable<Pixel> GetColumnPixelsX(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float xPixel = pixelColumnIndex + rp.DataRect.Left;
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + unitsPerPixelX * pixelColumnIndex;
        double end = start + unitsPerPixelX;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelX * .01;
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
        yield return new Pixel(xPixel, axes.GetPixelY(Ys[firstIndex] * YScale + YOffset)); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(firstIndex, lastIndex); //YOffset is added in GetRangeY
            if (Ys[firstIndex] > Ys[lastIndex])
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
            yield return new Pixel(xPixel, axes.GetPixelY(Ys[lastIndex] * YScale + YOffset)); // exit
        }
    }

    /// <summary>
    /// Given a pixel column, return the pixels to render its line.
    /// If the column contains no data, no pixels are returned.
    /// If the column contains one point, return that one pixel.
    /// If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetColumnPixelsY(int rowColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        // here rowColumnIndex will count upwards from the bottom, but pixels are measured from the top of the plot
        float yPixel = rp.DataRect.Bottom - rowColumnIndex;
        double unitsPerPixelY = axes.YAxis.Height / rp.DataRect.Height;
        double start = axes.YAxis.Min + unitsPerPixelY * rowColumnIndex;
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
        yield return new Pixel(axes.GetPixelX(Ys[firstIndex] * YScale + YOffset), yPixel); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(firstIndex, lastIndex);
            if (Ys[firstIndex] > Ys[lastIndex])
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
            yield return new Pixel(axes.GetPixelX(Ys[lastIndex] * YScale + YOffset), yPixel); // exit
        }
    }

    /// <summary>
    /// If data is off to the screen to the left, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointX(IAxes axes)
    {
        if (Xs.Length == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Min : axes.XAxis.Max); // if axis is reversed first index will on the right limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeX = axes.GetPixelX(Xs[firstPointIndex - 1] * XScale + XOffset);
            float beforeY = axes.GetPixelY(Ys[firstPointIndex - 1] * YScale + YOffset);
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
        if (Xs.Length == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Min : axes.YAxis.Max); // if axis is reversed first index will on the top limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeY = axes.GetPixelY(Xs[firstPointIndex - 1] * XScale + XOffset);
            float beforeX = axes.GetPixelX(Ys[firstPointIndex - 1] * YScale + YOffset);
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
        if (Xs.Length == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Max : axes.XAxis.Min); // if axis is reversed last index will on the left limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            float afterX = axes.GetPixelX(Xs[lastPointIndex] * XScale + XOffset);
            float afterY = axes.GetPixelY(Ys[lastPointIndex] * YScale + YOffset);
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
        if (Xs.Length == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Max : axes.YAxis.Min); // if axis is reversed last index will on the bottom limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            float afterY = axes.GetPixelY(Xs[lastPointIndex] * XScale + XOffset);
            float afterX = axes.GetPixelX(Ys[lastPointIndex] * YScale + YOffset);
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
        int index = Array.BinarySearch(Xs, indexRange.Min, indexRange.Length, (x - XOffset) / XScale);

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

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation)
    {
        return Rotated
            ? GetIndex(mouseLocation.Y)
            : GetIndex(mouseLocation.X);
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

}
