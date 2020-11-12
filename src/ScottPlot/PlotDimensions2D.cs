using System.Drawing;

namespace ScottPlot
{
    /* This class is a DTO intended to pass axis and figure dimensions
     * for a single X/Y plane into a render function.
     */
    public class PlotDimensions2D
    {
        private readonly PlotDimensions1D XAxis;
        private readonly PlotDimensions1D YAxis;

        // plot dimensions
        public float Width => XAxis.FigureSize;
        public float Height => YAxis.FigureSize;
        public float DataWidth => XAxis.DataSize;
        public float DataHeight => YAxis.DataSize;
        public float DataOffsetX => XAxis.DataOffset;
        public float DataOffsetY => YAxis.DataOffset;

        // axis limits
        public double XMin => XAxis.Min;
        public double XMax => XAxis.Max;
        public double YMin => YAxis.Min;
        public double YMax => YAxis.Max;
        public double XSpan => XAxis.Span;
        public double YSpan => YAxis.Span;
        public double XCenter => XAxis.Center;
        public double YCenter => YAxis.Center;

        // pixel/coordinate conversions
        public double PxPerUnitX => XAxis.PxPerUnit;
        public double PxPerUnitY => YAxis.PxPerUnit;
        public double UnitsPerPxX => XAxis.UnitsPerPx;
        public double UnitsPerPxY => YAxis.UnitsPerPx;
        public float GetPixelX(double position) => XAxis.GetPixel(position);
        public float GetPixelY(double position) => YAxis.GetPixel(position);
        public double GetCoordinateX(float pixel) => XAxis.GetUnit(pixel);
        public double GetCoordinateY(float pixel) => YAxis.GetUnit(pixel);

        public PlotDimensions2D(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            XAxis = new PlotDimensions1D(figureSize.Width, dataSize.Width, dataOffset.X, axisLimits.XMin, axisLimits.XMax, false);
            YAxis = new PlotDimensions1D(figureSize.Height, dataSize.Height, dataOffset.Y, axisLimits.YMin, axisLimits.YMax, true);
        }

        public override string ToString() =>
            $"Dimensions for figure ({Width}x{Height}), " +
            $"data area ({DataWidth}x{DataHeight}), " +
            $"and axes ({XMin}, {XMax}, {YMin}, {YMax})";
    }
}
