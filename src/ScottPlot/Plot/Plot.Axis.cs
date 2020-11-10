/* Code here is for adjusting axis limits and converting between pixel and coordinate units */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot
{
    public partial class Plot
    {
        public (double xMin, double xMax, double yMin, double yMax) AxisLimits()
        {
            (double xMin, double xMax) = settings.XAxis.Dims.RationalLimits();
            (double yMin, double yMax) = settings.YAxis.Dims.RationalLimits();
            return (xMin, xMax, yMin, yMax);
        }

        /// <summary>
        /// Optionally set axis limits and return the latest limits
        /// </summary>
        public double[] Axis(
            double? x1 = null,
            double? x2 = null,
            double? y1 = null,
            double? y2 = null
            )
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull && !settings.AxesHaveBeenSet)
                settings.AxisAuto();

            settings.AxisSet(x1, x2, y1, y2);
            settings.AxesHaveBeenSet = true;
            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Set axis limits [xMin, xMax, yMin, yMax]
        /// </summary>
        public double[] Axis(double[] axisLimits)
        {
            if ((axisLimits == null) || (axisLimits.Length != 4))
                throw new ArgumentException("axis limits must contain 4 elements");
            Axis(axisLimits[0], axisLimits[1], axisLimits[2], axisLimits[3]);
            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Set axis bounds (bounds restrict axis limits)
        /// </summary>
        public void AxisBounds(
            double minX = double.NegativeInfinity,
            double maxX = double.PositiveInfinity,
            double minY = double.NegativeInfinity,
            double maxY = double.PositiveInfinity)
        {
            // TODO: support bounds
            /*
            settings.axes.x.boundMin = minX;
            settings.axes.x.boundMax = maxX;
            settings.axes.y.boundMin = minY;
            settings.axes.y.boundMax = maxY;
            */
        }

        /// <summary>
        /// Adjust axis limits to achieve a certain pixel scale (units per pixel)
        /// </summary>
        public double[] AxisScale(double? unitsPerPixelX = null, double? unitsPerPixelY = null)
        {
            if (unitsPerPixelX != null)
            {
                double spanX = unitsPerPixelX.Value * settings.DataWidth;
                Axis(x1: settings.XAxis.Dims.Center - spanX / 2, x2: settings.XAxis.Dims.Center + spanX / 2);
            }

            if (unitsPerPixelY != null)
            {
                double spanY = unitsPerPixelY.Value * settings.DataHeight;
                Axis(y1: settings.YAxis.Dims.Center - spanY / 2, y2: settings.YAxis.Dims.Center + spanY / 2);
            }

            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Zoom one axis to ensure its scale (units per pixel) matches the other axis
        /// </summary>
        public double[] AxisEqual(bool preserveY = true)
        {
            if (preserveY)
                AxisScale(unitsPerPixelX: settings.YAxis.Dims.UnitsPerPx);
            else
                AxisScale(unitsPerPixelY: settings.XAxis.Dims.UnitsPerPx);
            return settings.AxisLimitsArray();
        }

        public void AxisLockScalesTogether(bool enable)
        {
            // TODO: support this
            /*
            settings.axes.equalAxes = enable;*/
        }

        [Obsolete("call AxisLockScalesTogether()", true)]
        public bool EqualAxis;

        // Keep this in the Plot module to assist discoverability
        [Obsolete("use AxisAuto() to fit axis limits to the data", true)]
        public double[] AutoAxis() => AxisAuto();

        // Keep this in the Plot module to assist discoverability
        [Obsolete("use AxisAuto() to fit axis limits to the data", true)]
        public double[] AutoScale() => AxisAuto();

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public double[] AxisAuto(
            double horizontalMargin = .05,
            double verticalMargin = .1,
            bool xExpandOnly = false,
            bool yExpandOnly = false)
        {
            settings.AxisAuto(horizontalMargin, verticalMargin, xExpandOnly, yExpandOnly);
            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public double[] AxisAutoX(double margin = .05, bool expandOnly = false)
        {
            if (settings.AxesHaveBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(horizontalMargin: margin, xExpandOnly: expandOnly);
            return Axis(newLimits[0], newLimits[1], originalLimits[2], originalLimits[3]);
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public double[] AxisAutoY(double margin = .1, bool expandOnly = false)
        {
            if (settings.AxesHaveBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(verticalMargin: margin, yExpandOnly: expandOnly);
            return Axis(originalLimits[0], originalLimits[1], newLimits[2], newLimits[3]);
        }

        /// <summary>
        /// Adjust axis limits to simulate zooming
        /// </summary>
        public double[] AxisZoom(double xFrac = 1, double yFrac = 1, double? zoomToX = null, double? zoomToY = null)
        {
            if (!settings.AxesHaveBeenSet)
                settings.AxisAuto();

            if (zoomToX is null)
                zoomToX = settings.XAxis.Dims.Center;

            if (zoomToY is null)
                zoomToY = settings.YAxis.Dims.Center;

            settings.XAxis.Dims.Zoom(xFrac, zoomToX);
            settings.YAxis.Dims.Zoom(yFrac, zoomToY);
            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Adjust axis limits to simulate panning
        /// </summary>
        public double[] AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.AxesHaveBeenSet)
                settings.AxisAuto();
            settings.XAxis.Dims.Pan(dx);
            settings.XAxis.Dims.Pan(dy);
            return settings.AxisLimitsArray();
        }

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double CoordinateFromPixelX(double pixelX) => settings.GetLocationX(pixelX);

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double CoordinateFromPixelY(double pixelY) => settings.GetLocationY(pixelY);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float CoordinateToPixelX(double locationX) => (float)(settings.GetPixelX(locationX) + settings.DataOffsetX);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float CoordinateToPixelY(double locationY) => (float)(settings.GetPixelX(locationY) + settings.DataOffsetY);

        [Obsolete("use X/Y methods", true)]
        public PointF CoordinateToPixel(double locationX, double locationY)
        {
            PointF pixelLocation = settings.GetPixel(locationX, locationY);
            pixelLocation.X += settings.DataOffsetX;
            pixelLocation.Y += settings.DataOffsetY;
            return pixelLocation;
        }

        [Obsolete("use X/Y methods", true)]
        public PointF CoordinateToPixel(PointF location) => CoordinateToPixel(location.X, location.Y);

        /// <summary>
        /// Set this plot's axis limits to match those of the given plot
        /// </summary>
        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).AxesHaveBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.AxesHaveBeenSet)
                AxisAuto();

            double x1 = horizontal ? sourcePlot.settings.XAxis.Dims.Min : settings.XAxis.Dims.Min;
            double x2 = horizontal ? sourcePlot.settings.XAxis.Dims.Max : settings.XAxis.Dims.Max;
            double y1 = vertical ? sourcePlot.settings.YAxis.Dims.Min : settings.YAxis.Dims.Min;
            double y2 = vertical ? sourcePlot.settings.YAxis.Dims.Max : settings.YAxis.Dims.Max;
            settings.AxisSet(x1, x2, y1, y2);
        }
    }
}
