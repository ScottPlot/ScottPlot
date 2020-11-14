using System.Drawing;

namespace ScottPlot
{
    /* This class is a DTO intended to pass axis and figure dimensions
     * for a single X/Y plane into a render function.
     */
    public class PlotDimensions2D
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

        public PlotDimensions2D(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight) = (dataSize.Width, dataSize.Height);
            (DataOffsetX, DataOffsetY) = (dataOffset.X, dataOffset.Y);
            (XMin, XMax, YMin, YMax) = (axisLimits.XMin, axisLimits.XMax, axisLimits.YMin, axisLimits.YMax);
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
