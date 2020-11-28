/* Code here is for adjusting axis limits and converting between pixel and coordinate units */

using System;

namespace ScottPlot
{
    public partial class Plot
    {
        Renderable.Axis XAxis => settings.XAxis;
        Renderable.Axis XAxis2 => settings.XAxis2;
        Renderable.Axis YAxis => settings.YAxis;
        Renderable.Axis YAxis2 => settings.YAxis2;

        public AxisLimits AxisLimits(int xAxisIndex = 0, int yAxisIndex = 0)
        {
            (double xMin, double xMax) = settings.GetXAxis(xAxisIndex).Dims.RationalLimits();
            (double yMin, double yMax) = settings.GetYAxis(yAxisIndex).Dims.RationalLimits();
            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        /// <summary>
        /// Optionally set axis limits and return the latest limits
        /// </summary>
        public void Axis(
            double? x1 = null,
            double? x2 = null,
            double? y1 = null,
            double? y2 = null,
            int xAxisIndex = 0,
            int yAxisIndex = 0
            )
        {
            // TODO: make overload that accepts xMin, xMax, yMin, yMax arguments and mark this one obsolete
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
        }

        /// <summary>
        /// Set axis limits [xMin, xMax, yMin, yMax]
        /// </summary>
        [Obsolete("use one of the other overloads to set axis limits")]
        public void Axis(double[] axisLimits, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            if ((axisLimits == null) || (axisLimits.Length != 4))
                throw new ArgumentException("axis limits must contain 4 elements");
            Axis(axisLimits[0], axisLimits[1], axisLimits[2], axisLimits[3], xAxisIndex, yAxisIndex);
        }

        /// <summary>
        /// Set axis limits [xMin, xMax, yMin, yMax]
        /// </summary>
        [Obsolete("use one of the other overloads to set axis limits")]
        public void Axis(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0) =>
            Axis(limits.XMin, limits.XMax, limits.YMin, limits.YMax, xAxisIndex, yAxisIndex);

        /// <summary>
        /// Set axis bounds (bounds restrict axis limits)
        /// </summary>
        public void AxisBounds(
            double minX = double.NegativeInfinity,
            double maxX = double.PositiveInfinity,
            double minY = double.NegativeInfinity,
            double maxY = double.PositiveInfinity)
        {
            settings.XAxis.Dims.SetBounds(minX, maxX);
            settings.YAxis.Dims.SetBounds(minY, maxY);
        }

        /// <summary>
        /// Adjust axis limits to achieve a certain pixel scale (units per pixel)
        /// </summary>
        public void AxisScale(double? unitsPerPixelX = null, double? unitsPerPixelY = null)
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
        }

        /// <summary>
        /// Zoom one axis to ensure its scale (units per pixel) matches the other axis
        /// </summary>
        public void AxisEqual(bool preserveY = true)
        {
            if (preserveY)
                AxisScale(unitsPerPixelX: settings.YAxis.Dims.UnitsPerPx);
            else
                AxisScale(unitsPerPixelY: settings.XAxis.Dims.UnitsPerPx);
        }

        /// <summary>
        /// Lock the primary X and Y scale (units per pixel) together. When enabled, squares cannot be distorted.
        /// </summary>
        public void AxisEqualScale(bool enable = true) => settings.AxisEqualScale = enable;

        [Obsolete("call AxisEqualScale()", true)]
        public bool EqualAxis;

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAuto(double horizontalMargin = .05, double verticalMargin = .1) =>
            settings.AxisAuto(horizontalMargin, verticalMargin);

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAutoX(double margin = .05, bool expandOnly = false)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            AxisLimits originalLimits = AxisLimits();
            AxisAuto(horizontalMargin: margin);
            Axis(y1: originalLimits.YMin, y2: originalLimits.YMax);
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAutoY(double margin = .1)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            AxisLimits originalLimits = AxisLimits();
            AxisAuto(horizontalMargin: margin);
            Axis(x1: originalLimits.XMin, x2: originalLimits.XMax);
        }

        /// <summary>
        /// Adjust axis limits to simulate zooming
        /// </summary>
        public void AxisZoom(
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
        }

        /// <summary>
        /// Adjust axis limits to simulate panning
        /// </summary>
        public void AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.AllAxesHaveBeenSet)
                settings.AxisAuto();
            settings.XAxis.Dims.Pan(dx);
            settings.XAxis.Dims.Pan(dy);
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
            double x1 = horizontal ? sourcePlot.settings.XAxis.Dims.Min : settings.XAxis.Dims.Min;
            double x2 = horizontal ? sourcePlot.settings.XAxis.Dims.Max : settings.XAxis.Dims.Max;
            double y1 = vertical ? sourcePlot.settings.YAxis.Dims.Min : settings.YAxis.Dims.Min;
            double y2 = vertical ? sourcePlot.settings.YAxis.Dims.Max : settings.YAxis.Dims.Max;
            settings.AxisSet(x1, x2, y1, y2, 0, 0);
        }

        /// <summary>
        /// Return the first horizontal axis with the specified index
        /// </summary>
        public Renderable.Axis GetXAxis(int xAxisIndex) =>
            settings.GetXAxis(xAxisIndex);

        /// <summary>
        /// Return the first vertical axis with the specified index
        /// </summary>
        public Renderable.Axis GetYAxis(int yAxisIndex) =>
            settings.GetYAxis(yAxisIndex);

        /// <summary>
        /// Create and return an additional axis
        /// </summary>
        public Renderable.Axis AddAxis(Renderable.Edge edge, int axisIndex, string title, System.Drawing.Color? color = null)
        {
            Renderable.Axis axis;

            if (edge == Renderable.Edge.Left)
                axis = new Renderable.AdditionalLeftAxis(axisIndex, title);
            else if (edge == Renderable.Edge.Right)
                axis = new Renderable.AdditionalRightAxis(axisIndex, title);
            else if (edge == Renderable.Edge.Bottom)
                axis = new Renderable.AdditionalBottomAxis(axisIndex, title);
            else if (edge == Renderable.Edge.Top)
                axis = new Renderable.AdditionalTopAxis(axisIndex, title);
            else
                throw new NotImplementedException("unsupported edge");

            axis.Title.Label = title;
            axis.Configure(color: color);

            settings.Axes.Add(axis);
            return axis;
        }
    }
}
