using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// A DTO for passing plot and axis dimensions into render functions
    /// </summary>
    public class PlotDimensions
    {
        public readonly float Width;
        public readonly float Height;
        public readonly float DataWidth;
        public readonly float DataHeight;
        public readonly float DataOffsetX;
        public readonly float DataOffsetY;

        public readonly double YMin;
        public readonly double YMax;
        public readonly double XMin;
        public readonly double XMax;

        public double XSpan => XMax - XMin;
        public double YSpan => YMax - YMin;
        public double XCenter => (XMax + XMin) / 2;
        public double YCenter => (YMax + YMin) / 2;

        public double PxPerUnitX => DataWidth / XSpan;
        public double PxPerUnitY => DataHeight / YSpan;
        public double UnitsPerPxX => XSpan / DataWidth;
        public double UnitsPerPxY => YSpan / DataHeight;

        public double[] LimitsArray => new double[] { XMin, XMax, YMin, YMax };
        public (double xMin, double xMax, double yMin, double yMax) LimitsTuple => (XMin, XMax, YMin, YMax);

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight, DataOffsetX, DataOffsetY) = (dataSize.Width, dataSize.Height, dataOffset.X, dataOffset.Y);
            (XMin, XMax, YMin, YMax) = (axisLimits.XMin, axisLimits.XMax, axisLimits.YMin, axisLimits.YMax);
        }

        public override string ToString() =>
            $"Dimensions for figure ({Width}x{Height}), " +
            $"data area ({DataWidth}x{DataHeight}), " +
            $"and axes ({XMin}, {XMax}, {YMin}, {YMax})";

        public float GetPixelX(double position, int axisIndex = 0)
        {
            double unitsFromMin = position - XMin;
            double pxFromMin = unitsFromMin * PxPerUnitX;
            double pixel = DataOffsetX + pxFromMin;
            return (float)pixel;
        }

        public float GetPixelY(double position, int axisIndex = 0)
        {
            double unitsFromMax = YMax - position; // intentionally inverted
            double pxFromMax = unitsFromMax * PxPerUnitY;
            double pixel = DataOffsetY + pxFromMax;
            return (float)pixel;
        }

        public double GetCoordinateX(float pixel, int axisIndex = 0)
        {
            return (pixel - DataOffsetX) / PxPerUnitX + XMin;
        }

        public double GetCoordinateY(float pixel, int axisIndex = 0)
        {
            return DataHeight - ((pixel - YMin) * PxPerUnitY);
        }
    }
}
