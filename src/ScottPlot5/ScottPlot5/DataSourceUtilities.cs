using ScottPlot.Collections;
using ScottPlot.DataSources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ScottPlot
{
    internal class BinarySearchComparer : IComparer<Coordinates>, IComparer<double>, IComparer<RootedCoordinateVector>, IComparer<RootedPixelVector>
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

    internal class GenericDoubleComparer<T> : IComparer<T>
    {
        public static readonly GenericDoubleComparer<T> Instance = new GenericDoubleComparer<T>();

        public int Compare(T a, T b)
        {
            if (a is IComparable<T> cA)
            {
                return cA.CompareTo(b);
            }
            else
            {
                return NumericConversion.GenericToDouble(ref a).CompareTo(NumericConversion.GenericToDouble(ref b));
            }
        }
    }

    internal static class DataSourceUtilities
    {
        public static bool IsAscending<T>(this IEnumerable<T> values, IComparer<T> comparer)
        {
            bool isAscending = true;
            T prev = values.First();
            var enu = values.GetEnumerator();
            while (isAscending && enu.MoveNext())
            {
                isAscending = comparer.Compare(prev, enu.Current) >= 0;
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

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int CalculateRenderIndexCount(int XsLength, int maxRenderIndex, int minRenderIndex)
            => Math.Min(XsLength - 1, maxRenderIndex) - minRenderIndex + 1;

        /// <summary> Creates a new <see cref="IndexRange"/> using the MinRenderIndex and <see cref="GetRenderIndexCount(IDataSource)"/> </summary>
        [MethodImpl(NumericConversion.ImplOptions)]
        public static IndexRange GetRenderIndexRange(this IDataSource source) 
            => new IndexRange(source.MinRenderIndex, source.MinRenderIndex + source.GetRenderIndexCount());

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

        [MethodImpl(NumericConversion.ImplOptions)]
        public static Coordinates ScaleXY(Coordinates coordinate, double xScalingFactor, double xOffset, double yScalingFactor, double yOffset) => new Coordinates()
        {
            X = ScaleXY(coordinate.X, xScalingFactor, xOffset),
            Y = ScaleXY(coordinate.Y, yScalingFactor, yOffset)
        };

        public static DataPoint GetNearest(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            if (dataSource.IsSorted)
                return GetNearest_Optimized(dataSource, mouseLocation, renderInfo, maxDistance, xAxis, yAxis);
            else
                return GetNearest_New(dataSource, mouseLocation, renderInfo, maxDistance, xAxis, yAxis);
        }

        public static DataPoint GetNearestX(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
        {
            if (dataSource.IsSorted)
                return GetNearestX_Optimized(dataSource, mouseLocation, renderInfo, maxDistance, xAxis);
            else
                return GetNearestX_New(dataSource, mouseLocation, renderInfo, maxDistance, xAxis);
        }

        public static DataPoint GetNearest_New(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

            var renderIndexCount = dataSource.GetRenderIndexCount();
            var minRenderIndex = dataSource.MinRenderIndex;

            double maxDistanceSquared = maxDistance * maxDistance;
            double closestDistanceSquared = double.PositiveInfinity;

            int closestIndex = 0;
            double closestX = double.PositiveInfinity;
            double closestY = double.PositiveInfinity;

            _ = renderInfo.TryGetPxPerUnitX(xAxis, out double pxPerUnitX);
            _ = renderInfo.TryGetPxPerUnitY(yAxis, out double pxPerUnitY);

            for (int i2 = 0; i2 < renderIndexCount; i2++)
            {
                int i = minRenderIndex + i2;

                (double X, double Y) = dataSource.PreferCoordinates ?
                    dataSource.GetCoordinateScaled(i).Deconstruct() :
                    (dataSource.GetXScaled(i), dataSource.GetYScaled(i));

                double dX = (X - mouseLocation.X) * pxPerUnitX;
                double dY = (Y - mouseLocation.Y) * pxPerUnitY;
                double distanceSquared = dX * dX + dY * dY;

                if (distanceSquared <= closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    closestX = X;
                    closestY = Y;
                    closestIndex = i;
                }

            }

            return closestDistanceSquared <= maxDistanceSquared
                    ? new DataPoint(closestX, closestY, closestIndex)
                    : DataPoint.None;
        }

        public static DataPoint GetNearest_Optimized(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {

            int closestIndex = dataSource.GetXClosestIndex(mouseLocation);

            _ = renderInfo.TryGetPxPerUnitX(xAxis, out double pxPerUnitX);
            _ = renderInfo.TryGetPxPerUnitY(yAxis, out double pxPerUnitY);

            
            int searchedLeft = closestIndex;
            int searchedRight = closestIndex;
            int NextPoint = closestIndex;
            double maxDistanceSquared = maxDistance * maxDistance;
            double maxDistanceToSearch = maxDistanceSquared;
            
            double closestDistanceSquared = double.PositiveInfinity;
            double closestX = 0;
            double closestY = 0;

            while (NextPoint != -1)
            {
                (double x, double y) = dataSource.PreferCoordinates ?
                    dataSource.GetCoordinateScaled(closestIndex).Deconstruct() :
                    (dataSource.GetXScaled(closestIndex), dataSource.GetYScaled(closestIndex));

                double dx = (x - mouseLocation.X) * pxPerUnitX;
                double dy = (y - mouseLocation.Y) * pxPerUnitY;
                double distanceSquared = dx * dx + dy * dy;

                if (distanceSquared < maxDistanceToSearch)
                {
                    maxDistanceToSearch = distanceSquared;
                    closestIndex = NextPoint;
                    closestX = x; 
                    closestY = y;
                    closestDistanceSquared = distanceSquared;
                }

                NextPoint = GetNextPointNearestSearch(dataSource, mouseLocation.X, searchedLeft, searchedRight, maxDistanceToSearch, pxPerUnitX);

                if (NextPoint < searchedLeft)
                    searchedLeft = NextPoint;
                else
                    searchedRight = NextPoint;
            };

            return closestDistanceSquared <= maxDistanceSquared
                ? new DataPoint(closestX, closestY, closestIndex)
                : DataPoint.None;
        }

        
        private static int GetNextPointNearestSearch(IDataSource dataSource, double locationXRotated, int searchedLeft, int searchedRight, double maxDistanceSquared, double PxPerUnitXRotated)
        {
            int leftCandidate = searchedLeft - 1;
            int rightCandidate = searchedRight + 1;

            if (leftCandidate < 0 && rightCandidate >= dataSource.Length)
            {
                return -1;
            }
            else if (leftCandidate < 0)
            {
                double distance = (dataSource.GetXScaled(rightCandidate) - locationXRotated) * PxPerUnitXRotated;
                if (distance * distance > maxDistanceSquared)
                    return -1;

                return rightCandidate;
            }
            else if (rightCandidate >= dataSource.Length)
            {
                double distance = (dataSource.GetXScaled(leftCandidate) - locationXRotated) * PxPerUnitXRotated;
                if (distance * distance > maxDistanceSquared)
                    return -1;

                return leftCandidate;
            }
            else
            {

                double leftCandidateDistance = (dataSource.GetXScaled(leftCandidate) - locationXRotated) * PxPerUnitXRotated;
                double rightCandidateDistance = (dataSource.GetXScaled(rightCandidate) - locationXRotated) * PxPerUnitXRotated;

                double minDistanceSquared = Math.Min(leftCandidateDistance * leftCandidateDistance, rightCandidateDistance * rightCandidateDistance);

                if (minDistanceSquared > maxDistanceSquared)
                    return -1;

                return Math.Abs(leftCandidateDistance) < Math.Abs(rightCandidateDistance) ? leftCandidate : rightCandidate;
            }
        }

        public static DataPoint GetNearestX_Optimized(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            int closestIndex = dataSource.GetXClosestIndex(mouseLocation);

            _ = renderInfo.TryGetPxPerUnitX(xAxis, out double pxPerUnitX);

            int searchedLeft = closestIndex;
            int searchedRight = closestIndex;
            int NextPoint = closestIndex;
            double maxDistanceSquared = maxDistance * maxDistance;
            double maxDistanceToSearch = maxDistanceSquared;

            double closestDistanceSquared = double.PositiveInfinity;


            if (dataSource.PreferCoordinates)
            {
                Coordinates closestCoordinate = Coordinates.NaN;
                while (NextPoint != -1)
                {
                    var coord = dataSource.GetCoordinateScaled(closestIndex);

                    double dx = (coord.X - mouseLocation.X) * pxPerUnitX;
                    double distanceSquared = dx * dx;

                    if (distanceSquared < maxDistanceToSearch)
                    {
                        maxDistanceToSearch = distanceSquared;
                        closestIndex = NextPoint;
                        closestCoordinate = coord;
                        closestDistanceSquared = distanceSquared;
                    }

                    NextPoint = GetNextPointNearestSearch(dataSource, mouseLocation.X, searchedLeft, searchedRight, maxDistanceToSearch, pxPerUnitX);

                    if (NextPoint < searchedLeft)
                        searchedLeft = NextPoint;
                    else
                        searchedRight = NextPoint;
                }

                return closestDistanceSquared <= maxDistanceSquared
                    ? new DataPoint(closestCoordinate.X, closestCoordinate.Y, closestIndex)
                    : DataPoint.None;
            }
            else
            {
                double closestX = 0;
                while (NextPoint != -1)
                {
                    double x = dataSource.GetXScaled(closestIndex);

                    double dx = (x - mouseLocation.X) * pxPerUnitX;
                    double distanceSquared = dx * dx;

                    if (distanceSquared < maxDistanceToSearch)
                    {
                        maxDistanceToSearch = distanceSquared;
                        closestIndex = NextPoint;
                        closestX = x;
                        closestDistanceSquared = distanceSquared;
                    }

                    NextPoint = GetNextPointNearestSearch(dataSource, mouseLocation.X, searchedLeft, searchedRight, maxDistanceToSearch, pxPerUnitX);

                    if (NextPoint < searchedLeft)
                        searchedLeft = NextPoint;
                    else
                        searchedRight = NextPoint;
                }

                return closestDistanceSquared <= maxDistanceSquared
                    ? new DataPoint(closestX, dataSource.GetYScaled(closestIndex), closestIndex)
                    : DataPoint.None;
            }
        }

        public static DataPoint GetNearestX_New(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

            _ = renderInfo.TryGetPxPerUnitX(xAxis, out double pxPerUnitX);

            var renderIndexCount = dataSource.GetRenderIndexCount();
            var minRenderIndex = dataSource.MinRenderIndex;

            double closestDistance = double.PositiveInfinity;

            int closestIndex = 0;
            
            if (dataSource.PreferCoordinates)
            {
                // To-DO : check for IScatterSource and optimize with GetScatterPoints?

                Coordinates closestCoord = default;
                for (int i2 = 0; i2 < renderIndexCount; i2++)
                {
                    int i = minRenderIndex + i2;
                    var coord = dataSource.GetCoordinateScaled(i);
                    double dX = Math.Abs(coord.X - mouseLocation.X) * pxPerUnitX;
                    if (dX <= closestDistance)
                    {
                        closestDistance = dX;
                        closestCoord = coord;
                        closestIndex = i;
                    }
                }

                return closestDistance <= maxDistance
                    ? new DataPoint(closestCoord.X, closestCoord.Y, closestIndex)
                    : DataPoint.None;
            }
            else
            {
                double closestX = double.PositiveInfinity;
                for (int i2 = 0; i2 < renderIndexCount; i2++)
                {
                    int i = minRenderIndex + i2;
                    var x = dataSource.GetXScaled(i);
                    double dX = Math.Abs(x - mouseLocation.X) * pxPerUnitX;
                    if (dX <= closestDistance)
                    {
                        closestDistance = dX;
                        closestX = x;
                        closestIndex = i;
                    }
                }

                return closestDistance <= maxDistance
                    ? new DataPoint(closestX, dataSource.GetYScaled(closestIndex), closestIndex)
                    : DataPoint.None;
            }
        }
    }
}
