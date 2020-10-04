/* Code here is for adjusting axis limits and converting between pixel and coordinate units */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {

        public double[] Axis(
            double? x1 = null,
            double? x2 = null,
            double? y1 = null,
            double? y2 = null
            )
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull && !settings.axes.hasBeenSet)
                settings.AxisAuto();

            settings.axes.Set(x1, x2, y1, y2);
            return settings.axes.limits;
        }

        public double[] Axis(double[] axisLimits)
        {
            if ((axisLimits == null) || (axisLimits.Length != 4))
                throw new ArgumentException("axis limits must contain 4 elements");
            Axis(axisLimits[0], axisLimits[1], axisLimits[2], axisLimits[3]);
            return settings.axes.limits;
        }

        public void AxisBounds(
            double minX = double.NegativeInfinity,
            double maxX = double.PositiveInfinity,
            double minY = double.NegativeInfinity,
            double maxY = double.PositiveInfinity)
        {
            settings.axes.x.boundMin = minX;
            settings.axes.x.boundMax = maxX;
            settings.axes.y.boundMin = minY;
            settings.axes.y.boundMax = maxY;
        }

        public double[] AxisScale(double? unitsPerPixelX = null, double? unitsPerPixelY = null)
        {
            if (unitsPerPixelX != null)
            {
                double spanX = unitsPerPixelX.Value * settings.dataSize.Width;
                Axis(x1: settings.axes.x.center - spanX / 2, x2: settings.axes.x.center + spanX / 2);
            }

            if (unitsPerPixelY != null)
            {
                double spanY = unitsPerPixelY.Value * settings.dataSize.Height;
                Axis(y1: settings.axes.y.center - spanY / 2, y2: settings.axes.y.center + spanY / 2);
            }

            return settings.axes.limits;
        }

        public double[] AxisEqual(bool preserveY = true)
        {
            if (preserveY)
                AxisScale(unitsPerPixelX: settings.yAxisUnitsPerPixel);
            else
                AxisScale(unitsPerPixelY: settings.xAxisUnitsPerPixel);
            return settings.axes.limits;
        }

        public bool EqualAxis
        {
            get => settings.axes.equalAxes;
            set
            {
                settings.axes.equalAxes = value;
                if (value)
                    settings.AxisAuto();
            }
        }

        // Keep this in the Plot module to assist discoverability
        [Obsolete("use AxisAuto() to fit axis limits to the data", true)]
        public double[] AutoAxis() => AxisAuto();

        // Keep this in the Plot module to assist discoverability
        [Obsolete("use AxisAuto() to fit axis limits to the data", true)]
        public double[] AutoScale() => AxisAuto();

        public double[] AxisAuto(
            double horizontalMargin = .05,
            double verticalMargin = .1,
            bool xExpandOnly = false,
            bool yExpandOnly = false,
            bool tightenLayout = true
            )
        {
            settings.AxisAuto(horizontalMargin, verticalMargin, xExpandOnly, yExpandOnly);
            if (tightenLayout)
                TightenLayout();
            else
                settings.layout.tighteningOccurred = true;
            return settings.axes.limits;
        }

        public double[] AxisAutoX(
            double margin = .05,
            bool expandOnly = false
            )
        {
            if (settings.axes.hasBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(horizontalMargin: margin, xExpandOnly: expandOnly);
            return Axis(newLimits[0], newLimits[1], originalLimits[2], originalLimits[3]);
        }

        public double[] AxisAutoY(
            double margin = .1,
            bool expandOnly = false
            )
        {
            if (settings.axes.hasBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(verticalMargin: margin, yExpandOnly: expandOnly);
            return Axis(originalLimits[0], originalLimits[1], newLimits[2], newLimits[3]);
        }

        public double[] AxisZoom(
            double xFrac = 1,
            double yFrac = 1,
            double? zoomToX = null,
            double? zoomToY = null
            )
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();

            if (zoomToX is null)
                zoomToX = settings.axes.x.center;

            if (zoomToY is null)
                zoomToY = settings.axes.y.center;

            settings.axes.Zoom(xFrac, yFrac, zoomToX, zoomToY);
            return settings.axes.limits;
        }

        public double[] AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            settings.axes.x.Pan(dx);
            settings.axes.y.Pan(dy);
            return settings.axes.limits;
        }

        public double CoordinateFromPixelX(double pixelX)
        {
            return settings.GetLocationX(pixelX);
        }

        public double CoordinateFromPixelY(double pixelY)
        {
            return settings.GetLocationY(pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(int pixelX, int pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(float pixelX, float pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(double pixelX, double pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(Point pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(PointF pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        public PointF CoordinateToPixel(double locationX, double locationY)
        {
            PointF pixelLocation = settings.GetPixel(locationX, locationY);
            pixelLocation.X += settings.dataOrigin.X;
            pixelLocation.Y += settings.dataOrigin.Y;
            return pixelLocation;
        }

        public PointF CoordinateToPixel(PointF location)
        {
            return CoordinateToPixel(location.X, location.Y);
        }

        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).axes.hasBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.axes.hasBeenSet)
                AxisAuto();

            if (horizontal)
            {
                settings.axes.x.min = sourcePlot.settings.axes.x.min;
                settings.axes.x.max = sourcePlot.settings.axes.x.max;
            }
            if (vertical)
            {
                settings.axes.y.min = sourcePlot.settings.axes.y.min;
                settings.axes.y.max = sourcePlot.settings.axes.y.max;
            }
            TightenLayout();
        }
    }
}
