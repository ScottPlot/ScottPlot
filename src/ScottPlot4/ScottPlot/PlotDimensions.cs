using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// PlotDimensions supplies figure dimensions and pixel/coordinate lookup methods for a single 2D plane
    /// </summary>
    public readonly struct PlotDimensions
    {
        // figure dimensions (pixel units)
        public readonly float Width;
        public readonly float Height;
        public readonly float FigureWidth => Width;
        public readonly float FigureHeight => Height;

        // data area dimensions (pixel units)
        public readonly float DataWidth;
        public readonly float DataHeight;
        public readonly float DataOffsetX;
        public readonly float DataOffsetY;

        // rendering options
        public readonly double ScaleFactor;

        // axis limits (coordinate units)
        public readonly double XMin;
        public readonly double XMax;
        public readonly double YMin;
        public readonly double YMax;
        public readonly double XSpan;
        public readonly double YSpan;
        public readonly double XCenter;
        public readonly double YCenter;
        public readonly AxisLimits AxisLimits;

        // pixel/coordinate conversions
        public readonly double PxPerUnitX;
        public readonly double PxPerUnitY;
        public readonly double UnitsPerPxX;
        public readonly double UnitsPerPxY;

        // Reverse axis direction
        public readonly bool IsReverseX;
        public readonly bool IsReverseY;

        public Pixel GetPixel(Coordinate coordinate) => new(GetPixelX(coordinate.X), GetPixelY(coordinate.Y));
        public float GetPixelX(double position)
        {
            return IsReverseX
                ? (float)(DataOffsetX + ((XMax - position) * PxPerUnitX))
                : (float)(DataOffsetX + ((position - XMin) * PxPerUnitX));
        }
        public float GetPixelY(double position)
        {
            return IsReverseY
               ? (float)(DataOffsetY + ((position - YMin) * PxPerUnitY))
               : (float)(DataOffsetY + ((YMax - position) * PxPerUnitY));
        }
        public Coordinate GetCoordinate(Pixel pixel) => new(GetCoordinateX(pixel.X), GetCoordinateY(pixel.Y));
        public Coordinate GetCoordinate(float xPixel, float yPixel) => new(GetCoordinateX(xPixel), GetCoordinateY(yPixel));
        public double GetCoordinateX(float pixel)
        {
            return IsReverseX
                ? XMax - ((pixel - DataOffsetX) / PxPerUnitX)
                : (pixel - DataOffsetX) / PxPerUnitX + XMin;

        }
        public double GetCoordinateY(float pixel)
        {
            return IsReverseY
                ? (pixel - DataOffsetY) / PxPerUnitY + YMin
                : YMax - (pixel - DataOffsetY) / PxPerUnitY;
        }
        public RectangleF GetDataRect() => new(DataOffsetX, DataOffsetY, DataWidth, DataHeight);
        public RectangleF GetRect(CoordinateRect rect)
        {
            float left = IsReverseX ? GetPixelX(rect.XMax) : GetPixelX(rect.XMin);
            float right = IsReverseX ? GetPixelX(rect.XMin) : GetPixelX(rect.XMax);
            float top = IsReverseY ? GetPixelY(rect.YMin) : GetPixelY(rect.YMax);
            float bottom = IsReverseY ? GetPixelY(rect.YMax) : GetPixelY(rect.YMin);
            float width = right - left;
            float height = bottom - top;
            return new RectangleF(left, top, width, height);
        }

        public PlotDimensions()
        {

        }

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits axisLimits, double scaleFactor, bool is_reverse_x = false, bool is_reverse_y = false)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight) = (dataSize.Width, dataSize.Height);
            (DataOffsetX, DataOffsetY) = (dataOffset.X, dataOffset.Y);
            AxisLimits = axisLimits;
            (XMin, XMax, YMin, YMax) = (axisLimits.XMin, axisLimits.XMax, axisLimits.YMin, axisLimits.YMax);
            (XSpan, YSpan) = (XMax - XMin, YMax - YMin);
            (XCenter, YCenter) = ((XMin + XMax) / 2, (YMin + YMax) / 2);
            (PxPerUnitX, PxPerUnitY) = (DataWidth / XSpan, DataHeight / YSpan);
            (UnitsPerPxX, UnitsPerPxY) = (XSpan / DataWidth, YSpan / DataHeight);
            ScaleFactor = scaleFactor;

            IsReverseX = is_reverse_x;
            IsReverseY = is_reverse_y;
        }

        public override string ToString() =>
            $"Dimensions for figure ({Width}x{Height}), " +
            $"data area ({DataWidth}x{DataHeight}), " +
            $"and axes ({XMin}, {XMax}, {YMin}, {YMax})";
    }
}
