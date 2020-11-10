using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// A DTO for passing plot and axis dimensions into render functions
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

        // axis limits with methods to perform pixel/unit conversions
        private readonly List<AxisInfo> XAxes = new List<AxisInfo>();
        private readonly List<AxisInfo> YAxes = new List<AxisInfo>();

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight, DataOffsetX, DataOffsetY) = (dataSize.Width, dataSize.Height, dataOffset.X, dataOffset.Y);

            AddAxisX(axisLimits.XMin, axisLimits.XMax);
            AddAxisY(axisLimits.YMin, axisLimits.YMax);
        }

        public void AddAxisX(double xMin, double xMax) =>
            XAxes.Add(new AxisInfo(Width, DataWidth, DataOffsetX, xMin, xMax, false));

        private void AddAxisY(double yMin, double yMax) =>
            YAxes.Add(new AxisInfo(Height, DataHeight, DataOffsetY, yMin, yMax, true));

        public override string ToString() =>
            $"Dimensions for figure ({Width}x{Height}), " +
            $"data area ({DataWidth}x{DataHeight}), " +
            $"and axes ({XMin}, {XMax}, {YMin}, {YMax})";

        // hard-coded shortcuts for primary axis fields
        public double XMin => XAxes[0].Min;
        public double XMax => XAxes[0].Max;
        public double YMin => YAxes[0].Min;
        public double YMax => YAxes[0].Max;
        public double XSpan => XAxes[0].Min;
        public double YSpan => YAxes[0].Max;
        public double XCenter => XAxes[0].Center;
        public double YCenter => YAxes[0].Center;
        public double PxPerUnitX => XAxes[0].PxPerUnit;
        public double PxPerUnitY => YAxes[0].PxPerUnit;
        public double UnitsPerPxX => XAxes[0].UnitsPerPx;
        public double UnitsPerPxY => YAxes[0].UnitsPerPx;
        public double[] LimitsArray => new double[] { XMin, XMax, YMin, YMax };
        public (double xMin, double xMax, double yMin, double yMax) LimitsTuple => (XMin, XMax, YMin, YMax);

        // public methods for pixel/unit conversions
        public float GetPixelX(double position, int axisIndex = 0) => XAxes[axisIndex].GetPixel(position);
        public float GetPixelY(double position, int axisIndex = 0) => YAxes[axisIndex].GetPixel(position);
        public double GetCoordinateX(float pixel, int axisIndex = 0) => XAxes[axisIndex].GetUnit(pixel);
        public double GetCoordinateY(float pixel, int axisIndex = 0) => YAxes[axisIndex].GetUnit(pixel);
    }
}
