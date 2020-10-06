using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Drawing
{
    /* 
     * This object is only for passing plot dimensions into the render functions of plottables.
     * It should hold no logic other than coordinate space conversion to/from pixel space.
     */

    /* TODO: When the rendering system is refactored to render onto a single bitmap (with no separate
     *       figure and data bitmaps) collapse this class to only track width and height of a single image.
     */
    public class PlotDimensions
    {
        readonly float Width;
        readonly float Height;

        readonly float DataWidth;
        readonly float DataHeight;

        readonly float DataOffsetX;
        readonly float DataOffsetY;

        readonly double YMin;
        readonly double YMax;
        readonly double XMin;
        readonly double XMax;

        readonly double PxPerUnitX;
        readonly double PxPerUnitY;
        readonly double UnitsPerPxX;
        readonly double UnitsPerPxY;

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight, DataOffsetX, DataOffsetY) = (dataSize.Width, dataSize.Height, dataOffset.X, dataOffset.Y);
            (XMin, XMax, YMin, YMax) = (axisLimits.x1, axisLimits.x2, axisLimits.y1, axisLimits.y2);
            PxPerUnitX = DataWidth / (XMax - XMin);
            PxPerUnitY = DataHeight / (YMax - YMin);
            UnitsPerPxX = (XMax - XMin) / DataWidth;
            UnitsPerPxY = (YMax - YMin) / DataHeight;
        }

        public float GetPixelX(double position, int axisIndex = 0, bool drawingOnDataBitmap = true)
        {
            if (axisIndex != 0)
                throw new NotImplementedException("multiple X axes are not yet supported");

            double unitsFromMin = position - XMin;
            double pxFromMin = unitsFromMin * PxPerUnitX;
            double pixel = drawingOnDataBitmap ? pxFromMin : DataOffsetX + pxFromMin;
            return (float)pixel;
        }

        public float GetPixelY(double position, int axisIndex = 0, bool drawingOnDataBitmap = true)
        {
            if (axisIndex != 0)
                throw new NotImplementedException("multiple Y axes are not yet supported");

            double unitsFromMax = YMax - position; // intentionally inverted
            double pxFromMax = unitsFromMax * PxPerUnitY;
            double pixel = drawingOnDataBitmap ? pxFromMax : DataOffsetY + pxFromMax;
            return (float)pixel;
        }
    }
}
