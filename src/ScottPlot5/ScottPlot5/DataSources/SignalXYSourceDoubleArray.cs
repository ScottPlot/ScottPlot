﻿using System;

namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray : ISignalXYSource
{
    readonly double[] Positions;
    readonly double[] Amplitudes;
    public int Count => Positions.Length;

    /// <summary>
    /// With Rotated = False, SignalXY has a horizontal Position axis and a vertical Amplitude axis
    /// With Rotated = True, SignalXY has a vertical Position axis and a horizontal Amplitude axis
    /// </summary>
    public bool Rotated { get; set; } = false;

    public double AmplitudeOffset { get; set; } = 0;
    public double AmplitudeScale { get; set; } = 1;
    public double PositionOffset { get; set; } = 0;
    public double PositionScale { get; set; } = 1;

    //Obselete properties handled for backward compatibility
    public double XOffset
    {
        get => PositionOffset;
        set => PositionOffset = value;
    }
    public double YOffset
    {
        get => AmplitudeOffset;
        set => AmplitudeOffset = value;
    }
    public double YScale
    {
        get => AmplitudeScale;
        set => AmplitudeScale = value;
    }
    public double XScale
    {
        get => PositionScale;
        set => PositionScale = value;
    }

    public int MinimumIndex { get; set; } = 0;
    public int MaximumIndex { get; set; }

    public SignalXYSourceDoubleArray(double[] positions, double[] amplitudes)
    {
        if (positions.Length != amplitudes.Length)
        {
            throw new ArgumentException($"{nameof(positions)} and {nameof(amplitudes)} must have equal length");
        }

        Positions = positions;
        Amplitudes = amplitudes;
        MaximumIndex = positions.Length - 1;
    }

    public AxisLimits GetAxisLimits()
    {
        if (Positions.Length == 0)
            return AxisLimits.NoLimits;

        double positionMin = Positions[MinimumIndex] * PositionScale + PositionOffset;
        double positionMax = Positions[MaximumIndex] * PositionScale + PositionOffset;

        CoordinateRange positionRange = new(positionMin, positionMax);
        CoordinateRange amplitudeRange = GetAmplitudeRange(MinimumIndex, MaximumIndex);
        return Rotated
            ? new AxisLimits(amplitudeRange, positionRange)
            : new AxisLimits(positionRange, amplitudeRange);
    }

    public Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        return Rotated
            ? GetPixelsToDrawVertically(rp, axes, connectStyle)
            : GetPixelsToDrawHorizontally(rp, axes, connectStyle);
    }

    private Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        if (Rotated)
        {
            throw new Exception($"GetPixelsToDrawHorizontally method should only be used when rendering SignalXY with {nameof(Rotated)} = False");
        }

        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointX(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointX(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
            .Select(pxColumn => GetColumnPixels(pxColumn, visibleRange, rp, axes))
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
        if (!Rotated)
        {
            throw new Exception($"GetPixelsToDrawVertically method should only be used when rendering SignalXY with {nameof(Rotated)} = True");
        }

        // determine the range of data in view
        (Pixel[] PointBefore, int dataIndexFirst) = GetFirstPointY(axes);
        (Pixel[] PointAfter, int dataIndexLast) = GetLastPointY(axes);
        IndexRange visibleRange = new(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> VisiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Height))
            .Select(pxRow => GetRowPixels(pxRow, visibleRange, rp, axes))
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
    /// Return the range covered by amplitude data between the given indices (inclusive)
    /// </summary>
    private CoordinateRange GetAmplitudeRange(int index1, int index2)
    {
        double min = Amplitudes[index1];
        double max = Amplitudes[index1];

        var minIndex = Math.Min(index1, index2);
        var maxIndex = Math.Max(index1, index2);

        for (int i = minIndex; i <= maxIndex; i++)
        {
            min = Math.Min(Amplitudes[i], min);
            max = Math.Max(Amplitudes[i], max);
        }

        return new CoordinateRange(min * AmplitudeScale + AmplitudeOffset, max * AmplitudeScale + AmplitudeOffset);
    }

    /// <summary>
    /// Get the index associated with the given position
    /// </summary>
    private int GetPositionIndex(double position)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return GetPositionIndex(position, range);
    }

    /// <summary>
    /// Get the index associated with the given position limited to the given range
    /// </summary>
    private int GetPositionIndex(double position, IndexRange indexRange)
    {
        var (_, index) = SearchPositionIndex(position, indexRange);
        return index;
    }

    /// <summary>
    /// Given a pixel column, return the pixels to render its line.
    /// If the column contains no data, no pixels are returned.
    /// If the column contains one point, return that one pixel.
    /// If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetColumnPixels(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float xPixel = pixelColumnIndex + rp.DataRect.Left;
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + unitsPerPixelX * pixelColumnIndex;
        double end = start + unitsPerPixelX;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelX * .01;
        end += overlap;

        var (startIndex, _) = SearchPositionIndex(start, rng);
        var (endIndex, _) = SearchPositionIndex(end, rng);

        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;


        yield return new Pixel(xPixel, axes.GetPixelY(Amplitudes[firstIndex] * AmplitudeScale + AmplitudeOffset)); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetAmplitudeRange(firstIndex, lastIndex); //AmplitudeOffset is added in GetAmplitudeRange
            if (Amplitudes[firstIndex] > Amplitudes[lastIndex])
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
            yield return new Pixel(xPixel, axes.GetPixelY(Amplitudes[lastIndex] * AmplitudeScale + AmplitudeOffset)); // exit
        }
    }

    /// <summary>
    /// Given a pixel row, return the pixels to render its line.
    /// If the row contains no data, no pixels are returned.
    /// If the row contains one point, return that one pixel.
    /// If the row contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetRowPixels(int pixelRowIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        // here pixelRowIndex will count upwards from the bottom, but pixels are measured from the top of the plot
        float yPixel = rp.DataRect.Bottom - pixelRowIndex;

        double unitsPerPixelY = axes.YAxis.Height / rp.DataRect.Height;
        double start = axes.YAxis.Min + unitsPerPixelY * pixelRowIndex;
        double end = start + unitsPerPixelY;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelY * .01;
        end += overlap;

        var (startIndex, _) = SearchPositionIndex(start, rng);
        var (endIndex, _) = SearchPositionIndex(end, rng);

        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;

        yield return new Pixel(axes.GetPixelX(Amplitudes[firstIndex] * AmplitudeScale + AmplitudeOffset), yPixel); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange xRange = GetAmplitudeRange(firstIndex, lastIndex);
            if (Amplitudes[firstIndex] > Amplitudes[lastIndex])
            { //signal amplitude is decreasing, so we'll return the maximum before the minimum
                yield return new Pixel(axes.GetPixelX(xRange.Max), yPixel); // max
                yield return new Pixel(axes.GetPixelX(xRange.Min), yPixel); // min
            }
            else
            { //signal amplitude is increasing, so we'll return the minimum before the maximum
                yield return new Pixel(axes.GetPixelX(xRange.Min), yPixel); // min
                yield return new Pixel(axes.GetPixelX(xRange.Max), yPixel); // max
            }
        }

        if (pointsInRange > 1)
        {
            yield return new Pixel(axes.GetPixelX(Amplitudes[lastIndex] * AmplitudeScale + AmplitudeOffset), yPixel); // exit
        }
    }

    /// <summary>
    /// If Position axis is horiztonal (Rotated = False) and data is off to the screen to the left, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointX(IAxes axes)
    {
        if (Rotated)
        {
            throw new Exception($"GetFirstPointX method should only be used when rendering SignalXY with {nameof(Rotated)} = False");
        }

        if (Positions.Length == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchPositionIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Min : axes.XAxis.Max); // if axis is reversed first index will on the right limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeX = axes.GetPixelX(Positions[firstPointIndex - 1] * PositionScale + PositionOffset);
            float beforeY = axes.GetPixelY(Amplitudes[firstPointIndex - 1] * AmplitudeScale + AmplitudeOffset);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], firstPointIndex);
        }
        else
        {
            return ([], MinimumIndex);
        }
    }

    /// <summary>
    /// If Position axis is vertical (Rotated = True) and data is off to the screen to the bottom, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointY(IAxes axes)
    {
        if (!Rotated)
        {
            throw new Exception($"GetFirstPointY method should only be used when rendering SignalXY with {nameof(Rotated)} = True");
        }

        if (Positions.Length == 1)
            return ([], MinimumIndex);

        var (firstPointIndex, _) = SearchPositionIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Min : axes.YAxis.Max); // if axis is reversed first index will on the top limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeX = axes.GetPixelX(Amplitudes[firstPointIndex - 1] * AmplitudeScale + AmplitudeOffset);
            float beforeY = axes.GetPixelY(Positions[firstPointIndex - 1] * PositionScale + PositionOffset);
            Pixel beforePoint = new(beforeX, beforeY);
            return ([beforePoint], firstPointIndex);
        }
        else
        {
            return ([], MinimumIndex);
        }
    }

    /// <summary>
    /// If Position axis is horizontal (Rotated = False) and data is off to the screen to the right, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointAfter, int lastIndex) GetLastPointX(IAxes axes)
    {
        if (Rotated)
        {
            throw new Exception($"GetLastPointX method should only be used when rendering SignalXY with {nameof(Rotated)} = False");
        }

        if (Positions.Length == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchPositionIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Max : axes.XAxis.Min); // if axis is reversed last index will on the left limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            float afterX = axes.GetPixelX(Positions[lastPointIndex] * PositionScale + PositionOffset);
            float afterY = axes.GetPixelY(Amplitudes[lastPointIndex] * AmplitudeScale + AmplitudeOffset);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], lastPointIndex - 1);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }

    /// <summary>
    /// If Position axis is vertical (Rotated = True) and data is off to the screen to the top, 
    /// returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointAfter, int lastIndex) GetLastPointY(IAxes axes)
    {
        if (!Rotated)
        {
            throw new Exception($"GetLastPointY method should only be used when rendering SignalXY with {nameof(Rotated)} = True");
        }

        if (Positions.Length == 1)
            return ([], MaximumIndex);

        var (lastPointIndex, _) = SearchPositionIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Max : axes.YAxis.Min); // if axis is reversed last index will on the bottom limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            float afterX = axes.GetPixelX(Amplitudes[lastPointIndex] * AmplitudeScale + AmplitudeOffset);
            float afterY = axes.GetPixelY(Positions[lastPointIndex] * PositionScale + PositionOffset);
            Pixel afterPoint = new(afterX, afterY);
            return ([afterPoint], lastPointIndex - 1);
        }
        else
        {
            return ([], MaximumIndex);
        }
    }

    /// <summary>
    /// Search the Position index associated with the given position
    /// </summary>
    private (int SearchedPositionIndex, int LimitedIndex) SearchPositionIndex(double position)
    {
        IndexRange range = new(MinimumIndex, MaximumIndex);
        return SearchPositionIndex(position, range);
    }

    /// <summary>
    /// Search the Position index associated with the given position limited to the given range
    /// SearchedPositionIndex is the index where the searched position would be inserted (could be one index larger than the indexRange)
    /// LimitedIndex is the index is the same as SearchedPositionIndex except that it wont exceed the indexRange
    /// </summary>
    private (int SearchedPositionIndex, int LimitedIndex) SearchPositionIndex(double position, IndexRange indexRange)
    {
        int index = Array.BinarySearch(Positions, indexRange.Min, indexRange.Length, (position - PositionOffset) / PositionScale);

        // If position is not exactly matched to any value in Positions, BinarySearch returns a negative number. We can bitwise negation to obtain the position where position would be inserted (i.e., the next highest index).
        // If position is below the min Positions, BinarySearch returns -1. Here, bitwise negation returns 0 (i.e., position would be inserted at the first index of the array).
        // If position is above the max Positions, BinarySearch returns -maxIndex. Bitwise negation of this value returns maxIndex + 1 (i.e., the position after the last index). However, this index is beyond the array bounds, so we return the final index instead.
        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return (SearchedPositionIndex: index, LimitedIndex: index > indexRange.Max ? indexRange.Max : index);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;


        for (int i = 0; i < Positions.Length; i++)
        {
            double dX = Rotated ?
                (Amplitudes[i] * AmplitudeScale + AmplitudeOffset - mouseLocation.X) * renderInfo.PxPerUnitX :
                (Positions[i] * PositionScale + PositionOffset - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = Rotated ?
                (Positions[i] * PositionScale + PositionOffset - mouseLocation.Y) * renderInfo.PxPerUnitY :
                (Amplitudes[i] * AmplitudeScale + AmplitudeOffset - mouseLocation.Y) * renderInfo.PxPerUnitY;

            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = Rotated ?
                    Amplitudes[i] * AmplitudeScale + AmplitudeOffset :
                    Positions[i] * PositionScale + PositionOffset;
                closestY = Rotated ?
                    Positions[i] * PositionScale + PositionOffset :
                    Amplitudes[i] * AmplitudeScale + AmplitudeOffset;
                    
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }

    public DataPoint GetNearestPosition(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        var MousePosition = Rotated ? mouseLocation.Y : mouseLocation.X;
        int i = GetPositionIndex(MousePosition); // TODO: check the index after too?
        var PxPerPositionUnit = Rotated ? renderInfo.PxPerUnitY : renderInfo.PxPerUnitX;

        double distance = (Positions[i] * PositionScale + PositionOffset - MousePosition) * PxPerPositionUnit;
        var closestX = Rotated ? Amplitudes[i] * AmplitudeScale + AmplitudeOffset : Positions[i] * PositionScale + PositionOffset;
        var closestY = Rotated ? Positions[i] * PositionScale + PositionOffset : Amplitudes[i] * AmplitudeScale + AmplitudeOffset;

        return Math.Abs(distance) <= maxDistance
            ? new DataPoint(closestX, closestY, i)
            : DataPoint.None;
    }
}
