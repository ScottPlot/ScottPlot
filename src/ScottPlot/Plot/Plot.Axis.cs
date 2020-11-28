/* Code here is for adjusting axis limits and converting between pixel and coordinate units */

using System;

namespace ScottPlot
{
    public partial class Plot
    {
        #region Axis access and creation

        /// <summary>
        /// Axis on the bottom edge of the plot
        /// </summary>
        public Renderable.Axis XAxis => settings.XAxis;

        /// <summary>
        /// Axis on the top edge of the plot
        /// </summary>
        public Renderable.Axis XAxis2 => settings.XAxis2;

        /// <summary>
        /// Axis on the left edge of the plot
        /// </summary>
        public Renderable.Axis YAxis => settings.YAxis;

        /// <summary>
        /// Axis on the right edge of the plot
        /// </summary>
        public Renderable.Axis YAxis2 => settings.YAxis2;

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

        #endregion

        #region coordinate/pixel conversions

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public (double x, double y) GetCoordinate(float xPixel, float yPixel) =>
            (settings.XAxis.Dims.GetUnit(xPixel), settings.YAxis.Dims.GetUnit(yPixel));

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double GetCoordinateX(float xPixel) => settings.XAxis.Dims.GetUnit(xPixel);

        /// <summary>
        /// Retrun the coordinate (in plot space) for the given pixel
        /// </summary>
        public double GetCoordinateY(float yPixel) => settings.YAxis.Dims.GetUnit(yPixel);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public (float xPixel, float yPixel) GetPixel(double x, double y) =>
            (settings.XAxis.Dims.GetPixel(x), settings.YAxis.Dims.GetPixel(y));

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float GetPixelX(double x) => settings.XAxis.Dims.GetPixel(x);

        /// <summary>
        /// Retrun the pixel location of the given coordinate (in plot space)
        /// </summary>
        public float GetPixelY(double y) => settings.YAxis.Dims.GetPixel(y);

        #endregion

        #region axis limits: get and set

        /// <summary>
        /// Get limits for the given axes
        /// </summary>
        public AxisLimits GetAxisLimits(int xAxisIndex = 0, int yAxisIndex = 0)
        {
            (double xMin, double xMax) = settings.GetXAxis(xAxisIndex).Dims.RationalLimits();
            (double yMin, double yMax) = settings.GetYAxis(yAxisIndex).Dims.RationalLimits();
            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        /// <summary>
        /// Set limits for the given axes
        /// </summary>
        public void SetAxisLimits(
            double? xMin = null, double? xMax = null,
            double? yMin = null, double? yMax = null,
            int xAxisIndex = 0, int yAxisIndex = 0)
        {
            bool notAllAxesDefined = xMin is null || xMax is null || yMin is null || yMax is null;
            if (notAllAxesDefined)
                settings.AxisAutoUnsetAxes();
            settings.AxisSet(xMin, xMax, yMin, yMax, xAxisIndex, yAxisIndex);
        }

        /// <summary>
        /// Set limits for the given axes
        /// </summary>
        public void SetAxisLimits(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0) =>
            settings.AxisSet(limits, xAxisIndex, yAxisIndex);

        /// <summary>
        /// Set boundaries of the primary axes (you cannot zoom, pan, or set axis limits beyond these boundaries)
        /// </summary>
        public void SetAxisBoundaries(
            double xMin = double.NegativeInfinity, double xMax = double.PositiveInfinity,
            double yMin = double.NegativeInfinity, double yMax = double.PositiveInfinity)
        {
            settings.XAxis.Dims.SetBounds(xMin, xMax);
            settings.YAxis.Dims.SetBounds(yMin, yMax);
        }

        #endregion

        #region axis limits: fit to plottable data

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAuto(double horizontalMargin = .05, double verticalMargin = .1) =>
            settings.AxisAuto(horizontalMargin, verticalMargin);

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAutoX(double margin = .05)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            AxisLimits originalLimits = GetAxisLimits();
            AxisAuto(horizontalMargin: margin);
            SetAxisLimits(yMin: originalLimits.YMin, yMax: originalLimits.YMax);
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data (with a little extra margin)
        /// </summary>
        public void AxisAutoY(double margin = .1)
        {
            if (settings.AllAxesHaveBeenSet == false)
                AxisAuto();

            AxisLimits originalLimits = GetAxisLimits();
            AxisAuto(horizontalMargin: margin);
            SetAxisLimits(xMin: originalLimits.XMin, xMax: originalLimits.XMax);
        }

        #endregion

        #region axis limits: scaling

        /// <summary>
        /// Adjust axis limits to achieve a certain pixel scale (units per pixel)
        /// </summary>
        public void AxisScale(double? unitsPerPixelX = null, double? unitsPerPixelY = null)
        {
            if (unitsPerPixelX != null)
            {
                double spanX = unitsPerPixelX.Value * settings.DataWidth;
                SetAxisLimits(xMin: settings.XAxis.Dims.Center - spanX / 2, xMax: settings.XAxis.Dims.Center + spanX / 2);
            }

            if (unitsPerPixelY != null)
            {
                double spanY = unitsPerPixelY.Value * settings.DataHeight;
                SetAxisLimits(xMin: settings.YAxis.Dims.Center - spanY / 2, xMax: settings.YAxis.Dims.Center + spanY / 2);
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
        /// Lock X and Y axis scales together (so squares cannot be distorted)
        /// </summary>
        public void AxisEqualScale(bool lockScales = true) => settings.AxisEqualScale = lockScales;

        #endregion

        #region axis limits: pan and zoom

        /// <summary>
        /// Zoom by a fraction (zoom in if fraction > 1)
        /// </summary>
        public void AxisZoom(
            double xFrac = 1, double yFrac = 1,
            double? zoomToX = null, double? zoomToY = null,
            int xAxisIndex = 0, int yAxisIndex = 0)
        {
            var xAxis = settings.GetXAxis(xAxisIndex);
            var yAxis = settings.GetYAxis(yAxisIndex);

            if (xAxis.Dims.HasBeenSet == false || yAxis.Dims.HasBeenSet == false)
                settings.AxisAuto();

            xAxis.Dims.Zoom(xFrac, zoomToX ?? xAxis.Dims.Center);
            yAxis.Dims.Zoom(yFrac, zoomToY ?? yAxis.Dims.Center);
        }

        /// <summary>
        /// Pan by a delta defined in coordinates
        /// </summary>
        public void AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.AllAxesHaveBeenSet)
                settings.AxisAuto();
            settings.XAxis.Dims.Pan(dx);
            settings.XAxis.Dims.Pan(dy);
        }

        #endregion

        #region obsolete

        [Obsolete("Use SetAxisLimits() and GetAxisLimits()")]
        public AxisLimits AxisLimits(int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateX()")]
        public double CoordinateFromPixelX(float pixelX) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateY()")]
        public double CoordinateFromPixelY(float pixelY) => throw new NotImplementedException();

        [Obsolete("use GetPixelX()")]
        public float CoordinateToPixelX(double locationX) => throw new NotImplementedException();

        [Obsolete("use GetPixelY()")]
        public float CoordinateToPixelY(double locationY) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public AxisLimits Axis(
            double? x1 = null, double? x2 = null, 
            double? y1 = null, double? y2 = null,
            int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void Axis(double[] axisLimits, int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void Axis(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use AxisEqualScale()", true)]
        public bool EqualAxis;

        #endregion
    }
}
