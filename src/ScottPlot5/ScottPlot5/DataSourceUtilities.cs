using ScottPlot.Collections;
using ScottPlot.DataSources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ScottPlot
{
    public sealed class BinarySearchComparer : IComparer<Coordinates>, IComparer<double>, IComparer<RootedCoordinateVector>, IComparer<RootedPixelVector>
    {
        public static readonly BinarySearchComparer Instance = new BinarySearchComparer();

        public int Compare(Coordinates a, Coordinates b)
        {
            return a.X.CompareTo(b.X);
        }

        public int Compare(double a, double b)
        {
            return a.CompareTo(b);
        }

        public int Compare(RootedPixelVector x, RootedPixelVector y)
        {
            return  x.Point.X.CompareTo(y.Point.X);
        }

        public int Compare(RootedCoordinateVector x, RootedCoordinateVector y)
        {
            return x.Point.X.CompareTo(y.Point.X);
        }
    }

    public sealed class GenericDoubleComparer<T> : IComparer<T>
    {
        public static readonly GenericDoubleComparer<T> Instance = new GenericDoubleComparer<T>();

#pragma warning disable CS8767 // This error is due to checking is IComparable is implemented when T is not specified as a struct -- Maybe restrict generic data sources to 'where T : struct, IComparable<T>' ?
        public int Compare(T a, T b)
        {
            if (a is IComparable<T> cA)
            {
                return cA.CompareTo(b); // JIT should optimize this call after detecting that T always implements IComparable
            }
            else
            {
                return NumericConversion.GenericToDouble(ref a).CompareTo(NumericConversion.GenericToDouble(ref b));
            }
        }
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    }

    public static class DataSourceUtilities
    {
        public static bool IsAscending<T>(this IEnumerable<T> values, IComparer<T> comparer)
        {
            bool isAscending = true;
            T prev = values.First();
            var enu = values.GetEnumerator();
            while (isAscending && enu.MoveNext())
            {
                isAscending = comparer.Compare(prev, enu.Current) <= 0;
                prev = enu.Current;
            }
            return isAscending;
        }

        #region < Binary Search | GetClosestIndex >

        /// <inheritdoc cref="Array.BinarySearch{T}(T[], int, int, T, IComparer{T})"/>
        [MethodImpl(NumericConversion.ImplOptions)]
        public static int BinarySearch<T>(this IList<T> sortedList, int index, int count, T value, IComparer<T> comparer)
        {
            return sortedList switch
            {
                List<T> listT => listT.BinarySearch(index, count, value, comparer),
                T[] arrayT => Array.BinarySearch(arrayT, index, count, value, comparer),
                CircularBuffer<T> circularBufferT => circularBufferT.BinarySearch(index, count, value, comparer),
                _ => throw new NotSupportedException($"unsupported {nameof(IList<T>)}: {sortedList.GetType().Name}")
            };
        }

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int GetClosestIndex(double[] sortedList, double value, IndexRange indexRange)
        {
            int index = Array.BinarySearch(sortedList, indexRange.Min, indexRange.Length, value);
            if (index < 0) index = ~index;
            return index > indexRange.Max ? indexRange.Max : index;
        }

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int GetClosestIndex(List<double> sortedList, double value, IndexRange indexRange)
        {
            int index = sortedList.BinarySearch(indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
            if (index < 0) index = ~index;
            return index > indexRange.Max ? indexRange.Max : index;
        }

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int GetClosestIndex(Coordinates[] sortedList, Coordinates value, IndexRange indexRange)
        {
            int index = Array.BinarySearch(sortedList, indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
            if (index < 0) index = ~index;
            return index > indexRange.Max ? indexRange.Max : index;
        }

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int GetClosestIndex(List<Coordinates> sortedList, Coordinates value, IndexRange indexRange)
        {
            int index = sortedList.BinarySearch(indexRange.Min, indexRange.Length, value, BinarySearchComparer.Instance);
            if (index < 0) index = ~index;
            return index > indexRange.Max ? indexRange.Max : index;
        }

        public static int GetClosestIndex<T>(this T[] sortedList, T value, IndexRange indexRange)
        {
            int index = Array.BinarySearch(sortedList, indexRange.Min, indexRange.Length, value, GenericDoubleComparer<T>.Instance);
            if (index < 0) index = ~index;
            return index > indexRange.Max ? indexRange.Max : index;
        }

        public static int GetClosestIndex<TValue, TList>(this TList sortedList, TValue value, IndexRange indexRange, IComparer<TValue> comparer)
            where TList : IEnumerable<TValue> // expects IList & IReadOnlyList
        {
            comparer ??= GenericDoubleComparer<TValue>.Instance;
            int index = sortedList switch
            {
                List<TValue> list => list.BinarySearch(indexRange.Min, indexRange.Length, value, comparer),
                TValue[] arr => Array.BinarySearch(arr, indexRange.Min, indexRange.Length, value, comparer),
                CircularBuffer<TValue> circBuffer => circBuffer.BinarySearch(indexRange.Min, indexRange.Length, value, comparer),
                _ => throw new NotSupportedException($"unsupported {nameof(TList)}: {sortedList.GetType().Name}")
            };
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

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int GetRenderIndexCount(this IDataSource dataSource)
            => Math.Min(dataSource.Length - 1, dataSource.MaxRenderIndex) - dataSource.MinRenderIndex + 1;

        /// <summary> Creates a new <see cref="IndexRange"/>  </summary>
        [MethodImpl(NumericConversion.ImplOptions)]
        public static IndexRange GetRenderIndexRange(this IDataSource dataSource) 
            => new IndexRange(Math.Max(0, dataSource.MinRenderIndex), Math.Min(dataSource.Length - 1, dataSource.MaxRenderIndex));

        #region < ScaleXY >

        [MethodImpl(NumericConversion.ImplOptions)]
        public static double ScaleXY(double point, double scalingFactor, double offset)
            => point * scalingFactor + offset;

        [MethodImpl(NumericConversion.ImplOptions)]
        public static double ScaleXY<T>(T point, double scalingFactor, double offset)
            => NumericConversion.GenericToDouble(ref point) * scalingFactor + offset;

        [MethodImpl(NumericConversion.ImplOptions)]
        public static double ScaleXY<T>(T[] collection, int index, double scalingFactor, double offset)
            => NumericConversion.GenericToDouble(collection, index) * scalingFactor + offset;

        [MethodImpl(NumericConversion.ImplOptions)]
        public static double ScaleXY<T>(IReadOnlyList<T> collection, int index, double scalingFactor, double offset)
            => NumericConversion.GenericToDouble(collection, index) * scalingFactor + offset;

        #endregion

        [MethodImpl(NumericConversion.ImplOptions)]
        public static Coordinates ScaleCoordinate(Coordinates coordinate, double xScalingFactor, double xOffset, double yScalingFactor, double yOffset)
        {
            return new Coordinates(
                x: ScaleXY(coordinate.X, xScalingFactor, xOffset), 
                y: ScaleXY(coordinate.Y, yScalingFactor, yOffset)
                );
        }


        [MethodImpl(NumericConversion.ImplOptions)]
        public static Coordinates UnScaleCoordinate(Coordinates coordinate, double xScalingFactor, double xOffset, double yScalingFactor, double yOffset)
        {
            return new Coordinates(
                x: (coordinate.X - xOffset) / xScalingFactor,
                y: (coordinate.Y - yOffset) / yScalingFactor
                );
        }

        [MethodImpl(NumericConversion.ImplOptions)]
        public static Coordinates UnScaleCoordinate(Coordinates pixelCoordinate, RenderDetails renderInfo, double xScalingFactor = 1, double xOffset = 0, double yScalingFactor = 1, double yOffset = 0, IXAxis? xaxis = null, IYAxis? yaxis = null)
        {
            renderInfo.TryGetPixelPerUnitX(xaxis, out var pixelScaleX);
            renderInfo.TryGetPixelPerUnitY(yaxis, out var pixelScaleY);

            return new Coordinates(
                x: UnScale(pixelCoordinate.X, pixelScaleX, xScalingFactor, xOffset),
                y: UnScale(pixelCoordinate.Y, pixelScaleY, yScalingFactor, yOffset)
                ); ;

            static double UnScale(double value,  double pixelScaling, double axisScaling, double offset)
            {
                return ((value / pixelScaling) - offset) / axisScaling;
            }
        }

        /// <summary>
        /// Get the nearest datapoint from the <paramref name="dataSource"/>, based on the <paramref name="mouseLocation"/> and the <paramref name="renderInfo"/>
        /// </summary>
        /// <remarks>This is the original way to locate the nearest DataPoint from the collection, and is safe for unsorted collections (such as Scatter)</remarks>
        /// <param name="dataSource">The data source</param>
        /// <param name="mouseLocation">the mouse coordinates from the plot. <see cref="Plot.GetCoordinates(Pixel, IXAxis?, IYAxis?)"/></param>
        /// <param name="renderInfo"><see cref="Plot.LastRender"/></param>
        /// <param name="maxDistance">The maximum distance to search</param>
        /// <param name="xAxis">The X-Axis of assigned to the datasource. If not specified, uses the bottom axis.</param>
        /// <param name="yAxis">The X-Axis of assigned to the datasource. If not specified, uses the left axis.</param>
        /// <returns>
        /// If match found : returns a datapoint that represents the (X,Y) values (not scaled or offset) 
        /// <br/>If no match found : returns <see cref="DataPoint.None"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataPoint GetNearest(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

            var renderIndexCount = dataSource.GetRenderIndexCount();
            var minRenderIndex = dataSource.MinRenderIndex;

            double maxDistanceSquared = maxDistance * maxDistance;
            double closestDistanceSquared = double.PositiveInfinity;

            int closestIndex = 0;

            _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);
            _ = renderInfo.TryGetPixelPerUnitY(yAxis, out double pxPerUnitY);

            bool preferCoordinates = dataSource.PreferCoordinates;

            for (int i2 = 0; i2 < renderIndexCount; i2++)
            {
                int i = minRenderIndex + i2;

                (double X, double Y) = preferCoordinates ?
                    dataSource.GetCoordinateScaled(i).Deconstruct() :
                    (dataSource.GetXScaled(i), dataSource.GetYScaled(i));

                double dX = Math.Abs(X - mouseLocation.X) * pxPerUnitX;
                double dY = Math.Abs(Y - mouseLocation.Y) * pxPerUnitY;
                double distanceSquared = dX * dX + dY * dY;

                if (distanceSquared <= closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    closestIndex = i;
                }
            }

            if (closestDistanceSquared <= maxDistanceSquared)
            {
                if (preferCoordinates)
                {
                    var coord = dataSource.GetCoordinate(closestIndex);
                    return new DataPoint(coord.X, coord.Y, closestIndex);
                }
                return new DataPoint(dataSource.GetX(closestIndex), dataSource.GetY(closestIndex), closestIndex);
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

            var renderIndexCount = dataSource.GetRenderIndexCount();
            var minRenderIndex = dataSource.MinRenderIndex;

            double closestDistance = double.PositiveInfinity;

            int closestIndex = 0;

            for (int i2 = 0; i2 < renderIndexCount; i2++)
            {
                int i = minRenderIndex + i2;
                var x = dataSource.GetXScaled(i);
                double dX = Math.Abs(x - mouseLocation.X) * pxPerUnitX;
                if (dX <= closestDistance)
                {
                    closestDistance = dX;
                    closestIndex = i;
                }
            }

            if (closestDistance <= maxDistance)
            {
                if (dataSource.PreferCoordinates)
                {
                    Coordinates closestCoord = dataSource.GetCoordinate(closestIndex);
                    return new DataPoint(closestCoord.X, closestCoord.Y, closestIndex);
                }
                return new DataPoint(dataSource.GetX(closestIndex), dataSource.GetY(closestIndex), closestIndex);
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

            while (NextPoint != -1)
            {
                (double x, double y) = preferCoordinates ?
                    dataSource.GetCoordinateScaled(NextPoint).Deconstruct() :
                    (dataSource.GetXScaled(NextPoint), dataSource.GetYScaled(NextPoint));

                double dx = Math.Abs(x - mouseLocation.X) * pxPerUnitX;
                double dy = Math.Abs(y - mouseLocation.Y) * pxPerUnitY;
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
                    var coord = dataSource.GetCoordinate(closestIndex);
                    return new DataPoint(coord.X, coord.Y, closestIndex);
                }
                return new DataPoint(dataSource.GetX(closestIndex), dataSource.GetY(closestIndex), closestIndex);
            }
            return DataPoint.None;
        }

        /// <summary>
        /// Get the nearest datapoint from the <paramref name="dataSource"/>, based on the <paramref name="mouseLocation"/> X location and the <paramref name="renderInfo"/>
        /// </summary>
        /// <inheritdoc cref="GetNearestFast"/>
        public static DataPoint GetNearestXFast(IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
        {
            int closestIndex = dataSource.GetXClosestIndex(mouseLocation);

            _ = renderInfo.TryGetPixelPerUnitX(xAxis, out double pxPerUnitX);

            int searchedLeft = closestIndex;
            int searchedRight = closestIndex;
            int NextPoint = closestIndex;
            double closestDistance = double.PositiveInfinity;
            double closestDistanceSquared = closestDistance;

            while (NextPoint != -1)
            {
                double x = dataSource.GetXScaled(NextPoint);
                double dx = Math.Abs(x - mouseLocation.X) * pxPerUnitX;

                if (dx < closestDistance)
                {
                    closestIndex = NextPoint;
                    closestDistance = dx;
                    closestDistanceSquared = dx * dx;
                }

                NextPoint = GetNextPointNearestSearch(dataSource, mouseLocation.X, searchedLeft, searchedRight, closestDistanceSquared, pxPerUnitX);

                if (NextPoint < searchedLeft)
                    searchedLeft = NextPoint;
                else
                    searchedRight = NextPoint;
            }

            if (closestDistance <= maxDistance)
            {
                if (dataSource.PreferCoordinates)
                {
                    Coordinates closestCoord = dataSource.GetCoordinate(closestIndex);
                    return new DataPoint(closestCoord.X, closestCoord.Y, closestIndex);
                }
                return new DataPoint(dataSource.GetX(closestIndex), dataSource.GetY(closestIndex), closestIndex);
            }
            return DataPoint.None;
        }

        private static int GetNextPointNearestSearch(IDataSource dataSource, double locationX, int searchedLeft, int searchedRight, double maxDistanceSquared, double PxPerUnitX)
        {
            int leftCandidate = searchedLeft - 1;
            int rightCandidate = searchedRight + 1;

            if (leftCandidate < 0 && rightCandidate >= dataSource.Length)
            {
                return -1;
            }
            else if (leftCandidate < 0)
            {
                double distance = (dataSource.GetXScaled(rightCandidate) - locationX) * PxPerUnitX;
                if (distance * distance > maxDistanceSquared)
                    return -1;

                return rightCandidate;
            }
            else if (rightCandidate >= dataSource.Length)
            {
                double distance = (dataSource.GetXScaled(leftCandidate) - locationX) * PxPerUnitX;
                if (distance * distance > maxDistanceSquared)
                    return -1;

                return leftCandidate;
            }
            else
            {

                double leftCandidateDistance = (dataSource.GetXScaled(leftCandidate) - locationX) * PxPerUnitX;
                double rightCandidateDistance = (dataSource.GetXScaled(rightCandidate) - locationX) * PxPerUnitX;

                double minDistanceSquared = Math.Min(leftCandidateDistance * leftCandidateDistance, rightCandidateDistance * rightCandidateDistance);

                if (minDistanceSquared > maxDistanceSquared)
                    return -1;

                return Math.Abs(leftCandidateDistance) < Math.Abs(rightCandidateDistance) ? leftCandidate : rightCandidate;
            }
        }
    }
}
