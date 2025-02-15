using ScottPlot.Collections;
using System.Runtime.CompilerServices;

namespace ScottPlot;

public static class DataSourceUtilities
{
    /// <summary>
    /// Check if a collection is in ascending order
    /// </summary>
    /// <typeparam name="T">the type to evaluate</typeparam>
    /// <param name="values">The values</param>
    /// <param name="comparer">The comparer to use. If not specified, uses Comparer{T}.Default (which is preferred)</param>
    /// <returns>True if the collection is ascending (and can therefore be used with BinarySearch and GetClosest). Otherwise false.</returns>
    public static bool IsAscending<T>(this IEnumerable<T> values, IComparer<T>? comparer)
    {
        using var enu = values?.GetEnumerator() ?? throw new ArgumentNullException(nameof(values));
        if (enu.MoveNext() == false)
            return false;

        comparer ??= GenericComparer<T>.Default;
        T prev = enu.Current;
        while (enu.MoveNext())
        {
            if (comparer.Compare(enu.Current, prev) < 0) // current >= prev = OK, current < prev Not OK
                return false;

            prev = enu.Current;
        }
        return true;
    }

    #region < Binary Search | GetClosestIndex >

    /// <inheritdoc cref="Array.BinarySearch{T}(T[], int, int, T, IComparer{T})"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int BinarySearch<T, TList>(this TList sortedList, int index, int length, T value, IComparer<T>? comparer)
        where TList : IEnumerable<T>
    {
        if (sortedList is null) throw new ArgumentNullException(nameof(sortedList));
        comparer ??= BinarySearchComparer.GetComparer<T>();

        return sortedList switch
        {
            List<T> listT => listT.BinarySearch(index, length, value, comparer),
            T[] arrayT => Array.BinarySearch(arrayT, index, length, value, comparer),
            CircularBuffer<T> circularBufferT => circularBufferT.BinarySearch(index, length, value, comparer),
            _ => throw new NotSupportedException($"unsupported {nameof(IList<T>)}: {sortedList.GetType().Name}")
        };
    }

    /// <inheritdoc cref="GetClosestIndex{TValue, TList}(TList, TValue, IndexRange, IComparer{TValue}?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int GetClosestIndex(double[] sortedList, double value, IndexRange indexRange)
    {
        int index = Array.BinarySearch(sortedList, indexRange.Min, indexRange.Length, value);
        if (index < 0) index = ~index;
        return index > indexRange.Max ? indexRange.Max : index;
    }

    /// <inheritdoc cref="GetClosestIndex{TValue, TList}(TList, TValue, IndexRange, IComparer{TValue}?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int GetClosestIndex(List<double> sortedList, double value, IndexRange indexRange)
    {
        if (sortedList is null) throw new ArgumentNullException(nameof(sortedList));
        int index = sortedList.BinarySearch(indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
        if (index < 0) index = ~index;
        return index > indexRange.Max ? indexRange.Max : index;
    }

    /// <inheritdoc cref="GetClosestIndex{TValue, TList}(TList, TValue, IndexRange, IComparer{TValue}?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int GetClosestIndex(Coordinates[] sortedList, Coordinates value, IndexRange indexRange)
    {
        int index = Array.BinarySearch(sortedList, indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
        if (index < 0) index = ~index;
        return index > indexRange.Max ? indexRange.Max : index;
    }

    /// <inheritdoc cref="GetClosestIndex{TValue, TList}(TList, TValue, IndexRange, IComparer{TValue}?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int GetClosestIndex(List<Coordinates> sortedList, Coordinates value, IndexRange indexRange)
    {
        if (sortedList is null) throw new ArgumentNullException(nameof(sortedList));
        int index = sortedList.BinarySearch(indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
        if (index < 0) index = ~index;
        return index > indexRange.Max ? indexRange.Max : index;
    }

    /// <summary>
    /// Gets the closest index to a specified <paramref name="value"/> from the <paramref name="sortedList"/>
    /// </summary>
    /// <typeparam name="TValue">The type of values in the collection</typeparam>
    /// <typeparam name="TList">Must be an object that implements IList or IReadOnlyList. List{T} | T[] are preferred.</typeparam>
    /// <param name="sortedList">The collection to search. Collection must be in ascending order.</param>
    /// <param name="value">The value to search for in the collection</param>
    /// <param name="indexRange">Provides details about the range of indexes to search</param>
    /// <param name="comparer">A comparer used to determine equality - recommend default comparer or <see cref="GenericComparer{T}.Default"/></param>
    /// <returns>The index of the item that is closest to the <paramref name="value"/></returns>
    public static int GetClosestIndex<TValue, TList>(this TList sortedList, TValue value, IndexRange indexRange, IComparer<TValue>? comparer)
        where TList : IEnumerable<TValue> // expects IList & IReadOnlyList
    {
        int index = BinarySearch(sortedList, indexRange.Min, indexRange.Length, value, comparer);

        // If x is not exactly matched to any value in Xs, BinarySearch returns a negative number. We can bitwise negation to obtain the position where x would be inserted (i.e., the next highest index).
        // If x is below the min Xs, BinarySearch returns -1. Here, bitwise negation returns 0 (i.e., x would be inserted at the first index of the array).
        // If x is above the max Xs, BinarySearch returns -maxIndex. Bitwise negation of this value returns maxIndex + 1 (i.e., the position after the last index). However, this index is beyond the array bounds, so we return the final index instead.
        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return index > indexRange.Max ? indexRange.Max : index;
    }

    #endregion

    /// <summary>
    /// Calculates the maximum count of indexes available to render in an <see cref="IDataSource"/>'s collection
    /// </summary>
    /// <param name="dataSource">The datasource</param>
    /// <returns>The number of indexes that should be rendered </returns>
    /// <remarks>Example : <br/>MaxIndex = Xs.Length - 1 <br/>MinIndex = 0, <br/>returns Xs.Length </remarks>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static int GetRenderIndexCount(this IDataSource dataSource)
        => Math.Max(0, Math.Min(dataSource.Length - 1, dataSource.MaxRenderIndex) - Math.Max(0, dataSource.MinRenderIndex) + 1);

    /// <summary>
    /// Creates a new <see cref="IndexRange"/> 
    /// </summary>
    /// <param name="dataSource">The DataSource</param>
    /// <returns>an IndexRange representing a the minimum and maximum indexes to render</returns>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static IndexRange GetRenderIndexRange(this IDataSource dataSource)
    {
        if (dataSource is null || dataSource.Length == 0) return IndexRange.None;
        int min = Math.Min(dataSource.Length - 1, dataSource.MinRenderIndex);
        int max = Math.Min(dataSource.Length - 1, dataSource.MaxRenderIndex);
        if (min > max || min < 0 || max < 0) return IndexRange.None;
        return new IndexRange(min, max);
    }

    #region < ScaleXY >

    /// <summary>
    /// Scale the <paramref name="value"/> by applying the <paramref name="scalingFactor"/> and the <paramref name="offset"/>
    /// </summary>
    /// <param name="value">the value to scale</param>
    /// <param name="scalingFactor">the scaling factor to apply.</param>
    /// <param name="offset">the offset to apply</param>
    /// <returns>value * <paramref name="scalingFactor"/> + <paramref name="offset"/></returns>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static double ScaleXY(double value, double scalingFactor, double offset)
        => value * scalingFactor + offset;

    /// <inheritdoc cref="ScaleXY(double, double, double)"/>
    /// <typeparam name="T">The type of value - will be converted to a double via <see cref="NumericConversion.GenericToDouble{T}(ref T)"/></typeparam>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static double ScaleXY<T>(T value, double scalingFactor, double offset)
        => NumericConversion.GenericToDouble(ref value) * scalingFactor + offset;

    /// <summary>
    /// Scale a value from the collection by the <paramref name="scalingFactor"/> and <paramref name="offset"/>
    /// </summary>
    /// <inheritdoc cref="ScaleXY(double, double, double)"/>
    /// <param name="collection">the collection to select a value from</param>
    /// <param name="index">the index of the value within the collection</param>
    /// <param name="scalingFactor"/> <param name="offset"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static double ScaleXY<T>(T[] collection, int index, double scalingFactor, double offset)
        => NumericConversion.GenericToDouble(collection, index) * scalingFactor + offset;

    /// <inheritdoc cref="ScaleXY{T}(T[], int, double, double)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static double ScaleXY<T>(IReadOnlyList<T> collection, int index, double scalingFactor, double offset)
        => NumericConversion.GenericToDouble(collection, index) * scalingFactor + offset;

    /// <summary>
    /// Scale the <paramref name="value"/> by applying the <paramref name="scalingFactor"/> and the <paramref name="offset"/>
    /// </summary>
    /// <param name="value">the value to unscale</param>
    /// <param name="scalingFactor">the scaling factor to remove.</param>
    /// <param name="offset">the offset to remove</param>
    /// <returns>(<paramref name="value"> - <paramref name="offset"/>) / <paramref name="scalingFactor"/> </returns>
    public static double UnscaleXY(double value, double scalingFactor, double offset)
        => (value - offset) / scalingFactor;

    #endregion

    /// <summary>
    /// Scale a coordinate by applying the offsets and the scaling factors.
    /// </summary>
    /// <inheritdoc cref="UnScaleCoordinate(Coordinates, RenderDetails, double, double, double, double, IXAxis?, IYAxis?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static Coordinates ScaleCoordinate(Coordinates coordinate, double xScalingFactor, double xOffset, double yScalingFactor, double yOffset)
    {
        return new Coordinates(
            x: ScaleXY(coordinate.X, xScalingFactor, xOffset),
            y: ScaleXY(coordinate.Y, yScalingFactor, yOffset)
            );
    }


    /// <inheritdoc cref="UnScaleCoordinate(Coordinates, RenderDetails, double, double, double, double, IXAxis?, IYAxis?)"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static Coordinates UnScaleCoordinate(Coordinates coordinate, double xScalingFactor, double xOffset, double yScalingFactor, double yOffset)
    {
        return new Coordinates(
            x: (coordinate.X - xOffset) / xScalingFactor,
            y: (coordinate.Y - yOffset) / yScalingFactor
            );
    }


    /// <summary>
    /// Unscale a coordinate
    /// </summary>
    /// <param name="pixelCoordinate">the coordinate to unscale</param>
    /// <param name="renderInfo">RenderDetails used to unscale the coordinate by the pixels/axis</param>
    /// <param name="xScalingFactor">The plottable's X Scaling factor</param>
    /// <param name="xOffset">The plottable's X Offset</param>
    /// <param name="yScalingFactor">The plottable's Y Scaling factor</param>
    /// <param name="yOffset">The plottable's Y OFfset</param>
    /// <param name="xaxis">The X-Axis to used to get the pixel scaling from the <paramref name="renderInfo"/>. If not found uses the default value from the renderInfo.</param>
    /// <param name="yaxis">The Y-Axis to used to get the pixel scaling from the <paramref name="renderInfo"/>. If not found uses the default value from the renderInfo.</param>
    /// <returns>A new <see cref="Coordinates"/> value</returns>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static Coordinates UnScaleCoordinate(Coordinates pixelCoordinate, RenderDetails renderInfo, double xScalingFactor = 1, double xOffset = 0, double yScalingFactor = 1, double yOffset = 0, IXAxis? xaxis = null, IYAxis? yaxis = null)
    {
        renderInfo.TryGetPixelPerUnitX(xaxis, out var pixelScaleX);
        renderInfo.TryGetPixelPerUnitY(yaxis, out var pixelScaleY);

        return new Coordinates(
            x: UnScale(pixelCoordinate.X, pixelScaleX, xScalingFactor, xOffset),
            y: UnScale(pixelCoordinate.Y, pixelScaleY, yScalingFactor, yOffset)
            ); ;

        static double UnScale(double value, double pixelScaling, double axisScaling, double offset)
        {
            return ((value / pixelScaling) - offset) / axisScaling;
        }
    }

    /// <remarks> Checks <see cref="IDataSource.IsSorted"/> and decides to call the Fast or Original method accordingly</remarks>
    /// <inheritdoc cref="GetNearestFast"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static DataPoint GetNearestSmart(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        if (dataSource.IsSorted()) return GetNearestFast(dataSource, mouseLocation, renderInfo, maxDistance, xAxis, yAxis);
        return GetNearest(dataSource, mouseLocation, renderInfo, maxDistance, xAxis, yAxis);
    }


    /// <remarks> Checks <see cref="IDataSource.IsSorted"/> and decides to call the Fast or Original method accordingly</remarks>
    /// <inheritdoc cref="GetNearestXFast"/>
    [MethodImpl(NumericConversion.ImplOptions)]
    public static DataPoint GetNearestXSmart(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
    {
        if (dataSource.IsSorted()) return GetNearestXFast(dataSource, mouseLocation, renderInfo, maxDistance, xAxis);
        return GetNearestX(dataSource, mouseLocation, renderInfo, maxDistance, xAxis);
    }

    /// <summary>
    /// Get the nearest datapoint on the plot from the <paramref name="dataSource"/>, based on the <paramref name="mouseLocation"/> and the <paramref name="renderInfo"/>
    /// </summary>
    /// <remarks>This is the original way to locate the nearest DataPoint from the collection, and is safe for unsorted collections (such as Scatter)</remarks>
    /// <param name="dataSource">The data source</param>
    /// <param name="mouseLocation">the mouse coordinates from the plot. <see cref="Plot.GetCoordinates(Pixel, IXAxis?, IYAxis?)"/></param>
    /// <param name="renderInfo"><see cref="Plot.LastRender"/></param>
    /// <param name="maxDistance">The maximum distance to search</param>
    /// <param name="xAxis">The X-Axis of assigned to the datasource. If not specified, uses the bottom axis.</param>
    /// <param name="yAxis">The X-Axis of assigned to the datasource. If not specified, uses the left axis.</param>
    /// <returns>
    /// If match found : returns a datapoint that represents the closest (X,Y) coordinate on the plot, and the index that can be used to get the values from the DataSource.
    /// <br/>If no match found : returns <see cref="DataPoint.None"/>
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DataPoint GetNearest(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;
        int closestIndex = 0;

        _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);
        _ = renderInfo.TryGetPixelPerUnitY(yAxis, out double pxPerUnitY);

        int indexLength = Math.Max(0, dataSource.MinRenderIndex) + dataSource.GetRenderIndexCount();
        bool preferCoordinates = dataSource.PreferCoordinates;
        double X; double Y;

        for (int i = Math.Max(0, dataSource.MinRenderIndex); i < indexLength; i++)
        {
            if (preferCoordinates)
            {
                (X, Y) = dataSource.GetCoordinateScaled(i).Deconstruct();
            }
            else
            {
                X = dataSource.GetXScaled(i);
                Y = dataSource.GetYScaled(i);
            }

            double dX = (X - mouseLocation.X) * pxPerUnitX;
            double dY = (Y - mouseLocation.Y) * pxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestIndex = i;
            }
        }

        if (closestDistanceSquared < maxDistanceSquared)
        {
            if (preferCoordinates)
            {
                return new DataPoint(dataSource.GetCoordinateScaled(closestIndex), closestIndex);
            }
            return new DataPoint(dataSource.GetXScaled(closestIndex), dataSource.GetYScaled(closestIndex), closestIndex);
        }
        return DataPoint.None;
    }

    /// <summary>
    /// Get the nearest datapoint from the <paramref name="dataSource"/>, based on the <paramref name="mouseLocation"/> X location and the <paramref name="renderInfo"/>
    /// </summary>
    /// <inheritdoc cref="GetNearest"/>
    public static DataPoint GetNearestX(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
    {
        if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

        _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);

        var indexLength = Math.Max(0, dataSource.MinRenderIndex) + dataSource.GetRenderIndexCount();
        double closestDistance = double.PositiveInfinity;

        int closestIndex = 0;

        for (int i = Math.Max(0, dataSource.MinRenderIndex); i < indexLength; i++)
        {
            double dX = Math.Abs(dataSource.GetXScaled(i) - mouseLocation.X) * pxPerUnitX;
            if (dX < closestDistance)
            {
                closestDistance = dX;
                closestIndex = i;
            }
        }

        if (closestDistance <= maxDistance)
        {
            if (dataSource.PreferCoordinates)
            {
                return new DataPoint(dataSource.GetCoordinateScaled(closestIndex), closestIndex);
            }
            return new DataPoint(dataSource.GetXScaled(closestIndex), dataSource.GetYScaled(closestIndex), closestIndex);
        }
        return DataPoint.None;
    }

    /// <remarks>This is a faster way to locate the nearest DataPoint from the collection, but it requires the collection to be sorted in ascending order. ( Signal | SignalXY )</remarks>
    /// <inheritdoc cref="GetNearest"/>
    public static DataPoint GetNearestFast(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        // To-DO : GetXCLosestIndex accepts the mouse coordinate, which means its fully scalled and offset. This must be accounted for in the IDataSource.GetXClosestIndex
        int closestIndex = dataSource.GetXClosestIndex(mouseLocation);

        _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);
        _ = renderInfo.TryGetPixelPerUnitY(yAxis, out double pxPerUnitY);


        int searchedLeft = closestIndex;
        int searchedRight = closestIndex;
        int NextPoint = closestIndex;
        double maxDistanceSquared = maxDistance * maxDistance;
        double maxDistanceToSearch = maxDistanceSquared;

        double closestDistanceSquared = double.PositiveInfinity;

        bool preferCoordinates = dataSource.PreferCoordinates;
        double x; double y;
        while (NextPoint != -1)
        {
            if (preferCoordinates)
            {
                (x, y) = dataSource.GetCoordinateScaled(NextPoint).Deconstruct();
            }
            else
            {
                x = dataSource.GetXScaled(NextPoint);
                y = dataSource.GetYScaled(NextPoint);
            }

            double dx = (x - mouseLocation.X) * pxPerUnitX;
            double dy = (y - mouseLocation.Y) * pxPerUnitY;
            double distanceSquared = dx * dx + dy * dy;

            if (distanceSquared < maxDistanceToSearch)
            {
                maxDistanceToSearch = distanceSquared;
                closestIndex = NextPoint;
                closestDistanceSquared = distanceSquared;
            }

            NextPoint = GetNextPointNearestSearch(dataSource, mouseLocation.X, searchedLeft, searchedRight, maxDistanceToSearch, pxPerUnitX);

            if (NextPoint < searchedLeft)
                searchedLeft = NextPoint;
            else
                searchedRight = NextPoint;
        };

        if (closestDistanceSquared <= maxDistanceSquared)
        {
            if (preferCoordinates)
            {
                return new DataPoint(dataSource.GetCoordinateScaled(closestIndex), closestIndex);
            }
            return new DataPoint(dataSource.GetXScaled(closestIndex), dataSource.GetYScaled(closestIndex), closestIndex);
        }
        return DataPoint.None;
    }

    /// <summary>
    /// Get the nearest datapoint from the <paramref name="dataSource"/>, based on the <paramref name="mouseLocation"/> X location and the <paramref name="renderInfo"/>
    /// </summary>
    /// <inheritdoc cref="GetNearestFast"/>
    public static DataPoint GetNearestXFast(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
    {
        //Nearest X will always be within 1 of the closest index of a sorted collection
        _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);
        int closestIndex = dataSource.GetXClosestIndex(mouseLocation);

        double d1 = Math.Abs(dataSource.GetXScaled(closestIndex) - mouseLocation.X) * pxPerUnitX;
        if (closestIndex > 0)
        {
            double d2 = Math.Abs(dataSource.GetXScaled(closestIndex - 1) - mouseLocation.X) * pxPerUnitX;
            if (d1 <= d2 && d1 <= maxDistance)
            {
                // do nothing - heads to the return statements at bottom
            }
            else if (d2 < maxDistance) // d2 is known closer at this point, check if within range
            {
                closestIndex--;
            }
            else
            {
                return DataPoint.None;
            }
        }
        else if (d1 > maxDistance)
        {
            return DataPoint.None;
        }

        // return closest Index
        if (dataSource.PreferCoordinates)
        {
            return new DataPoint(dataSource.GetCoordinateScaled(closestIndex), closestIndex);
        }
        return new DataPoint(dataSource.GetXScaled(closestIndex), dataSource.GetYScaled(closestIndex), closestIndex);
    }

    private static int GetNextPointNearestSearch(IDataSource dataSource, double locationX, int searchedLeft, int searchedRight, double maxDistanceSquared, double PxPerUnitX)
    {
        int leftCandidate = searchedLeft - 1;
        int rightCandidate = searchedRight + 1;

        if (leftCandidate < 0 && rightCandidate >= dataSource.Length)
        {
            return -1;
        }

        if (leftCandidate < 0)
        {
            double distance = (dataSource.GetXScaled(rightCandidate) - locationX) * PxPerUnitX;
            if (distance * distance > maxDistanceSquared)
                return -1;

            return rightCandidate;
        }

        if (rightCandidate >= dataSource.Length)
        {
            double distance = (dataSource.GetXScaled(leftCandidate) - locationX) * PxPerUnitX;
            if (distance * distance > maxDistanceSquared)
                return -1;

            return leftCandidate;
        }

        double leftCandidateDistance = (dataSource.GetXScaled(leftCandidate) - locationX) * PxPerUnitX;
        double rightCandidateDistance = (dataSource.GetXScaled(rightCandidate) - locationX) * PxPerUnitX;

        double minDistanceSquared = Math.Min(leftCandidateDistance * leftCandidateDistance, rightCandidateDistance * rightCandidateDistance);
        if (minDistanceSquared > maxDistanceSquared)
            return -1;

        return Math.Abs(leftCandidateDistance) < Math.Abs(rightCandidateDistance) ? leftCandidate : rightCandidate;
    }
}
