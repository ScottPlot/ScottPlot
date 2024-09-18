using System.Runtime.CompilerServices;

namespace ScottPlot
{
    internal static class DataSourceUtilities
    {
        [MethodImpl(NumericConversion.ImplOptions)]
        public static int CalculateRenderIndexCount(this IDataSource dataSource)
            => Math.Min(dataSource.Length - 1, dataSource.MaxRenderIndex) - dataSource.MinRenderIndex + 1;

        [MethodImpl(NumericConversion.ImplOptions)]
        public static int CalculateRenderIndexCount(int XsLength, int maxRenderIndex, int minRenderIndex)
            => Math.Min(XsLength - 1, maxRenderIndex) - minRenderIndex + 1;


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
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

            var renderIndexCount = dataSource.CalculateRenderIndexCount();
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

        public static DataPoint GetNearestX(this IDataSource dataSource, Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis = null)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));

            _ = renderInfo.TryGetPxPerUnitX(xAxis, out double pxPerUnitX);

            var renderIndexCount = dataSource.CalculateRenderIndexCount();
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
