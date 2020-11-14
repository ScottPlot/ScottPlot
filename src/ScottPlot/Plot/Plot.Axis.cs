/* Code here is for adjusting axis limits and converting between pixel and coordinate units */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            double? y2 = null,
            int xAxisIndex = 0,
            int yAxisIndex = 0
            )
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull)
            {
                // auto-axis before assigning the rest of the values
                var xAxis = settings.GetXAxis(xAxisIndex);
                var yAxis = settings.GetYAxis(yAxisIndex);
                if (xAxis.Dims.HasBeenSet == false || yAxis.Dims.HasBeenSet == false)
                    AxisAuto(xAxisIndex, yAxisIndex);
            }

            settings.AxisSet(x1, x2, y1, y2, xAxisIndex, yAxisIndex);
            return settings.AxisLimitsArray(xAxisIndex, yAxisIndex);
        }

        /// <summary>
        /// Set axis limits [xMin, xMax, yMin, yMax]
        /// </summary>
        public double[] Axis(double[] axisLimits, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            if ((axisLimits == null) || (axisLimits.Length != 4))
                throw new ArgumentException("axis limits must contain 4 elements");
            Axis(axisLimits[0], axisLimits[1], axisLimits[2], axisLimits[3], xAxisIndex, yAxisIndex);
            return settings.AxisLimitsArray(xAxisIndex, yAxisIndex);
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

            // TODO: respect axis indexes
            return settings.AxisLimitsArray(0, 0);
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
            // TODO: respect axis indexes
            return settings.AxisLimitsArray(0, 0);
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
            double verticalMargin = .1)
        {
            settings.AxisAuto(horizontalMargin, verticalMargin);
            return settings.AxisLimitsArray(0, 0);
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public double[] AxisAutoX(double margin = .05, bool expandOnly = false)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(horizontalMargin: margin);
            return Axis(newLimits[0], newLimits[1], originalLimits[2], originalLimits[3]);
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public double[] AxisAutoY(double margin = .1)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(verticalMargin: margin);
            return Axis(originalLimits[0], originalLimits[1], newLimits[2], newLimits[3]);
        }

        /// <summary>
        /// Adjust axis limits to simulate zooming
        /// </summary>
        public double[] AxisZoom(
            double xFrac = 1,
            double yFrac = 1,
            double? zoomToX = null,
            double? zoomToY = null,
            int xAxisIndex = 0,
            int yAxisIndex = 0)
        {
            var xAxis = settings.GetXAxis(xAxisIndex);
            var yAxis = settings.GetYAxis(yAxisIndex);

            if (xAxis.Dims.HasBeenSet == false || yAxis.Dims.HasBeenSet == false)
                settings.AxisAuto();

            xAxis.Dims.Zoom(xFrac, zoomToX ?? xAxis.Dims.Center);
            yAxis.Dims.Zoom(yFrac, zoomToY ?? yAxis.Dims.Center);

            return settings.AxisLimitsArray(xAxisIndex, yAxisIndex);
        }

        /// <summary>
        /// Adjust axis limits to simulate panning
        /// </summary>
        public double[] AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.AllAxesHaveBeenSet)
                settings.AxisAuto();
            settings.XAxis.Dims.Pan(dx);
            settings.XAxis.Dims.Pan(dy);

            // TODO: support axis index
            return settings.AxisLimitsArray(0, 0);
        }

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double CoordinateFromPixelX(float pixelX) => settings.XAxis.Dims.GetUnit(pixelX);

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double CoordinateFromPixelY(float pixelY) => settings.YAxis.Dims.GetUnit(pixelY);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float CoordinateToPixelX(double locationX) => settings.XAxis.Dims.GetPixel(locationX);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float CoordinateToPixelY(double locationY) => settings.YAxis.Dims.GetPixel(locationY);

        /// <summary>
        /// Set this plot's axis limits to match those of the given plot
        /// </summary>
        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).AllAxesHaveBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.AllAxesHaveBeenSet)
                AxisAuto();

            double x1 = horizontal ? sourcePlot.settings.XAxis.Dims.Min : settings.XAxis.Dims.Min;
            double x2 = horizontal ? sourcePlot.settings.XAxis.Dims.Max : settings.XAxis.Dims.Max;
            double y1 = vertical ? sourcePlot.settings.YAxis.Dims.Min : settings.YAxis.Dims.Min;
            double y2 = vertical ? sourcePlot.settings.YAxis.Dims.Max : settings.YAxis.Dims.Max;
            settings.AxisSet(x1, x2, y1, y2, 0, 0);
        }
    }
}
