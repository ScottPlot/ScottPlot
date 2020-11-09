using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

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
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }

        public double YMin { get; private set; }
        public double YMax { get; private set; }
        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double XSpan => XMax - XMin;
        public double YSpan => YMax - YMin;
        public double XCenter => (XMax + XMin) / 2;
        public double YCenter => (YMax + YMin) / 2;

        public double PxPerUnitX => DataWidth / XSpan;
        public double PxPerUnitY => DataHeight / YSpan;
        public double UnitsPerPxX => XSpan / DataWidth;
        public double UnitsPerPxY => YSpan / DataHeight;

        public PlotDimensions()
        {
            ResetAxes();
        }

        public PlotDimensions(SizeF figureSize, SizeF dataSize, PointF dataOffset, AxisLimits2D axisLimits)
        {
            (Width, Height) = (figureSize.Width, figureSize.Height);
            (DataWidth, DataHeight, DataOffsetX, DataOffsetY) = (dataSize.Width, dataSize.Height, dataOffset.X, dataOffset.Y);
            (XMin, XMax, YMin, YMax) = (axisLimits.x1, axisLimits.x2, axisLimits.y1, axisLimits.y2);
            MakeRational();
        }

        private void MakeRational()
        {
            XMin = double.IsNaN(XMin) ? -10 : XMin;
            XMax = double.IsNaN(XMax) ? 10 : XMax;
            YMin = double.IsNaN(YMin) ? -10 : YMin;
            YMax = double.IsNaN(YMax) ? 10 : YMax;

            if (XMin == XMax)
                (XMin, XMax) = (XMin - .5, XMax + .5);
            if (YMin == YMax)
                (YMin, YMax) = (YMin - .5, YMax + .5);
        }

        public void ResetAxes() =>
            (XMin, XMax, YMin, YMax) = (-10, 10, -10, 10);

        public void Resize(float width, float height) => (Width, Height) = (width, height);

        public void ResizeDataWithPadding(float padLeft, float padRight, float padBottom, float padTop)
        {
            DataOffsetX = padLeft;
            DataOffsetY = padTop;
            DataWidth = Width - padLeft - padRight;
            DataHeight = Height - padTop - padBottom;
        }

        public void SetAxis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            XMin = x1 ?? XMin;
            XMax = x2 ?? XMax;
            YMin = y1 ?? YMin;
            YMax = y2 ?? YMax;
            MakeRational();
        }

        public void Pan(double xUnits, double yUnits)
        {
            XMin += xUnits;
            XMax += xUnits;
            YMin += yUnits;
            YMax += yUnits;
        }

        public void PanPx(double pxX, double pxY) =>
            Pan(pxX * UnitsPerPxX, pxY * UnitsPerPxY);

        public void Zoom(double fracX, double fracY)
        {
            ZoomX(fracX);
            ZoomY(fracY);
        }

        public void ZoomX(double frac = 1, double? zoomTo = null)
        {
            zoomTo = zoomTo ?? XCenter;
            double spanLeft = (double)zoomTo - XMin;
            double spanRight = XMax - (double)zoomTo;
            XMin = (double)zoomTo - spanLeft / frac;
            XMax = (double)zoomTo + spanRight / frac;
        }

        public void ZoomY(double frac = 1, double? zoomTo = null)
        {
            zoomTo = zoomTo ?? YCenter;
            double spanLeft = (double)zoomTo - YMin;
            double spanRight = YMax - (double)zoomTo;
            YMin = (double)zoomTo - spanLeft / frac;
            YMax = (double)zoomTo + spanRight / frac;
        }

        public float GetPixelX(double position, int axisIndex = 0)
        {
            if (axisIndex != 0)
                throw new NotImplementedException("multiple X axes are not yet supported");

            double unitsFromMin = position - XMin;
            double pxFromMin = unitsFromMin * PxPerUnitX;
            double pixel = DataOffsetX + pxFromMin;
            return (float)pixel;
        }

        public float GetPixelY(double position, int axisIndex = 0)
        {
            if (axisIndex != 0)
                throw new NotImplementedException("multiple Y axes are not yet supported");

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
