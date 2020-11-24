using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// PlotDimensions supplies figure dimensions and pixel/coordinate lookup methods for a single 2D plane
    /// </summary>
    public class PlotDimensions
    {
        // plot dimensions
        public readonly float Width;
        public readonly float Height;
        public readonly float DataWidth;
        public readonly float DataHeight;
        public readonly float DataOffsetX;
        public readonly float DataOffsetY;

        // axis limits
        public readonly double XMin;
        public readonly double XMax;
        public readonly double YMin;
        public readonly double YMax;
        public readonly double XSpan;
        public readonly double YSpan;
        public readonly double XCenter;
        public readonly double YCenter;

        // pixel/coordinate conversions
        public readonly double PxPerUnitX;
        public readonly double PxPerUnitY;
        public readonly double UnitsPerPxX;
        public readonly double UnitsPerPxY;

        public float GetPixelX(double position) => (float)(DataOffsetX + ((position - XMin) * PxPerUnitX));
        public float GetPixelY(double position) => (float)(DataOffsetY + ((YMax - position) * PxPerUnitY));
        public double GetCoordinateX(float pixel) => (pixel - DataOffsetX) / PxPerUnitX + XMin;
        public double GetCoordinateY(float pixel) => DataHeight - ((pixel - YMin) * PxPerUnitY);

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset,
            (double xMin, double xMax, double yMin, double yMax) axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight) = (dataSize.Width, dataSize.Height);
            (DataOffsetX, DataOffsetY) = (dataOffset.X, dataOffset.Y);
            (XMin, XMax, YMin, YMax) = (axisLimits.xMin, axisLimits.xMax, axisLimits.yMin, axisLimits.yMax);
            (XSpan, YSpan) = (XMax - XMin, YMax - YMin);
            (XCenter, YCenter) = ((XMin + XMax) / 2, (YMin + YMax) / 2);
            (PxPerUnitX, PxPerUnitY) = (DataWidth / XSpan, DataHeight / YSpan);
            (UnitsPerPxX, UnitsPerPxY) = (XSpan / DataWidth, YSpan / DataHeight);
        }

        public override string ToString() =>
            $"Dimensions for figure ({Width}x{Height}), " +
            $"data area ({DataWidth}x{DataHeight}), " +
            $"and axes ({XMin}, {XMax}, {YMin}, {YMax})";
    }
}
