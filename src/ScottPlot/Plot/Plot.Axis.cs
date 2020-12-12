/* 
 * This file contains code related to Axes including:
 *   - Unit/Pixel conversions
 *   - Configuring axis limits and boundaries
 *   - Axis labels (XLabel, YLabel, Title, etc)
 *   - Adding multiple axes
 *   - Grid lines
 *   - Tick marks
 *   - Tick labels
 */

using System;
using System.Drawing;

namespace ScottPlot
{
    public partial class Plot
    {
        #region shortcuts: primary axes

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

        #endregion

        #region shortcuts: axis label, tick, and grid

        /// <summary>
        /// Set the label for the vertical axis to the right of the plot (XAxis). 
        /// </summary>
        public void XLabel(string label) => XAxis.Label(label);

        /// <summary>
        /// Set the label for the vertical axis to the right of the plot (YAxis2). 
        /// </summary>
        public void YLabel(string label) => YAxis.Label(label);

        /// <summary>
        /// Set the label for the horizontal axis above the plot (XAxis2). 
        /// </summary>
        public void Title(string label, bool bold = true) => XAxis2.Label(label, bold: bold);

        /// <summary>
        /// Configure color and visibility of the frame that outlines the data area (lines along the edges of the primary axes)
        /// </summary>
        public void Frame(bool? visible = null, Color? color = null, bool? left = null, bool? right = null, bool? bottom = null, bool? top = null)
        {
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };

            foreach (var axis in primaryAxes)
                axis.Line(visible, color);

            YAxis.Line(visible: left);
            YAxis2.Line(visible: right);
            XAxis.Line(visible: bottom);
            XAxis2.Line(visible: top);
        }

        /// <summary>
        /// Set size of the primary axes to zero so the data area covers the whole figure
        /// </summary>
        public void Frameless()
        {
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };
            foreach (var axis in primaryAxes)
                axis.Hide();
        }

        /// <summary>
        /// Customize basic options for the primary X and Y axes. 
        /// Call XAxis and YAxis methods to further customize individual axes.
        /// </summary>
        public void Grid(bool? enable = null, Color? color = null, LineStyle? lineStyle = null)
        {
            if (enable.HasValue)
            {
                XAxis.Grid(enable.Value);
                YAxis.Grid(enable.Value);
            }

            XAxis.MajorGrid(color: color, lineStyle: lineStyle);
            YAxis.MajorGrid(color: color, lineStyle: lineStyle);
        }

        /// <summary>
        /// Set padding around the data area by defining the minimum size and padding for all axes
        /// </summary>
        public void Layout(float? left = null, float? right = null, float? bottom = null, float? top = null, float? padding = 5)
        {
            YAxis.Layout(padding, left);
            YAxis2.Layout(padding, right);
            XAxis.Layout(padding, bottom);
            XAxis2.Layout(padding, top);
        }

        /// <summary>
        /// Adjust this axis layout based on the layout of a source plot
        /// </summary>
        public void MatchLayout(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).AllAxesHaveBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.AllAxesHaveBeenSet)
                AxisAuto();

            var sourceSettings = sourcePlot.GetSettings(false);

            if (horizontal)
            {
                YAxis.SetSize(sourceSettings.YAxis.GetSize());
                YAxis2.SetSize(sourceSettings.YAxis2.GetSize());
            }

            if (vertical)
            {
                XAxis.SetSize(sourceSettings.XAxis.GetSize());
                XAxis2.SetSize(sourceSettings.XAxis2.GetSize());
            }
        }

        /// <summary>
        /// Manually define X axis tick labels
        /// </summary>
        public void XTicks(string[] labels) => XTicks(DataGen.Consecutive(labels.Length), labels);

        /// <summary>
        /// Manually define X axis tick positions and labels
        /// </summary>
        public void XTicks(double[] positions = null, string[] labels = null) =>
            XAxis.ManualTickPositions(positions, labels);

        /// <summary>
        /// Manually define Y axis tick labels
        /// </summary>
        public void YTicks(string[] labels) => YTicks(DataGen.Consecutive(labels.Length), labels);

        /// <summary>
        /// Manually define Y axis tick positions and labels
        /// </summary>
        public void YTicks(double[] positions = null, string[] labels = null) =>
            YAxis.ManualTickPositions(positions, labels);

        /// <summary>
        /// Set the culture to use for number-to-string converstion for tick labels of all axes
        /// </summary>
        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            foreach (var axis in settings.Axes)
                axis.SetCulture(culture);
        }

        /// <summary>
        /// Set the culture to use for number-to-string converstion for tick labels of all axes
        /// </summary>
        /// <param name="shortDatePattern"></param>
        /// <param name="decimalSeparator">Separates the decimal digits</param>
        /// <param name="numberGroupSeparator">Separates large numbers ito groups of digits for readability</param>
        /// <param name="decimalDigits">Number of digits after the numberDecimalSeparator</param>
        /// <param name="numberNegativePattern"></param>
        /// <param name="numberGroupSizes">Sizes of decimal groups which are separated by the numberGroupSeparator</param>
        public void SetCulture(string shortDatePattern = null, string decimalSeparator = null, string numberGroupSeparator = null,
            int? decimalDigits = null, int? numberNegativePattern = null, int[] numberGroupSizes = null)
        {
            foreach (var axis in settings.Axes)
                axis.SetCulture(shortDatePattern, decimalSeparator, numberGroupSeparator, decimalDigits, numberNegativePattern, numberGroupSizes);
        }

        #endregion

        #region Axis creation

        /// <summary>
        /// Create and return an additional axis
        /// </summary>
        public Renderable.Axis AddAxis(Renderable.Edge edge, int axisIndex, string title = null, System.Drawing.Color? color = null)
        {
            if (axisIndex <= 1)
                throw new ArgumentException("The default axes already occupy indexes 0 and 1. Additional axes require higher indexes.");

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

            if (color.HasValue)
                axis.Color(color.Value);

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
        /// Set limits for the primary X axis
        /// </summary>
        public void SetAxisLimitsX(double xMin, double xMax) => SetAxisLimits(xMin, xMax, null, null);

        /// <summary>
        /// Set limits for the primary Y axis
        /// </summary>
        public void SetAxisLimitsY(double yMin, double yMax) => SetAxisLimits(null, null, yMin, yMax);

        /// <summary>
        /// Set limits for the given axes
        /// </summary>
        public void SetAxisLimits(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0) =>
            settings.AxisSet(limits, xAxisIndex, yAxisIndex);

        /// <summary>
        /// Set limits of the view for the primary axes (you cannot zoom, pan, or set axis limits beyond these boundaries)
        /// </summary>
        public void SetViewLimits(
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
            settings.AxisAutoAll(horizontalMargin, verticalMargin);

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
        /// Lock X and Y axis scales (units per pixel) together to protect symmetry of circles and squares
        /// </summary>
        public void AxisScaleLock(bool enable)
        {
            settings.AxisAutoUnsetAxes();
            settings.AxisEqualScale = enable;
            settings.LayoutAuto();
            settings.EnforceEqualAxisScales();
        }

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
                settings.AxisAutoAll();

            xAxis.Dims.Zoom(xFrac, zoomToX ?? xAxis.Dims.Center);
            yAxis.Dims.Zoom(yFrac, zoomToY ?? yAxis.Dims.Center);
        }

        /// <summary>
        /// Pan by a delta defined in coordinates
        /// </summary>
        public void AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.AllAxesHaveBeenSet)
                settings.AxisAutoAll();
            settings.XAxis.Dims.Pan(dx);
            settings.XAxis.Dims.Pan(dy);
        }

        #endregion

        #region obsolete

        [Obsolete("Use SetAxisLimits() and GetAxisLimits()", true)]
        public AxisLimits AxisLimits(int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateX()", true)]
        public double CoordinateFromPixelX(float pixelX) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateY()", true)]
        public double CoordinateFromPixelY(float pixelY) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateX()", true)]
        public double CoordinateFromPixelX(double pixelX) => throw new NotImplementedException();

        [Obsolete("use GetCoordinateY()", true)]
        public double CoordinateFromPixelY(double pixelY) => throw new NotImplementedException();

        [Obsolete("use GetCoordinate(), GetCoordinateX() or GetCoordinateY()", true)]
        public System.Drawing.PointF CoordinateFromPixel(int pixelX, int pixelY) => throw new NotImplementedException();

        [Obsolete("use GetCoordinate(), GetCoordinateX() or GetCoordinateY()", true)]
        public System.Drawing.PointF CoordinateFromPixel(float pixelX, float pixelY) => throw new NotImplementedException();

        [Obsolete("use GetCoordinate(), GetCoordinateX() or GetCoordinateY()", true)]
        public System.Drawing.PointF CoordinateFromPixel(double pixelX, double pixelY) => throw new NotImplementedException();

        [Obsolete("use GetCoordinate(), GetCoordinateX() or GetCoordinateY()", true)]
        public System.Drawing.PointF CoordinateFromPixel(System.Drawing.Point pixel) => throw new NotImplementedException();

        [Obsolete("use GetCoordinate(), GetCoordinateX() or GetCoordinateY()", true)]
        public System.Drawing.PointF CoordinateFromPixel(System.Drawing.PointF pixel) => throw new NotImplementedException();

        [Obsolete("use GetPixel, GetPixelX(), or GetPixelY()", true)]
        public System.Drawing.PointF CoordinateToPixel(System.Drawing.PointF location) => throw new NotImplementedException();

        [Obsolete("use GetPixel, GetPixelX(), or GetPixelY()", true)]
        public System.Drawing.PointF CoordinateToPixel(double locationX, double locationY) => throw new NotImplementedException();

        [Obsolete("use GetPixelX()", true)]
        public float CoordinateToPixelX(double locationX) => throw new NotImplementedException();

        [Obsolete("use GetPixelY()", true)]
        public float CoordinateToPixelY(double locationY) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public AxisLimits Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
            => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void Axis(double[] axisLimits, int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public double[] Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null, double? _ = null) => null;

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public double[] Axis(double[] axisLimits) => null;

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true) => throw new NotImplementedException();

        [Obsolete("use GetAxisLimits() and SetAxisLimits()", true)]
        public void Axis(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0) => throw new NotImplementedException();

        [Obsolete("use AxisEqualScale()", true)]
        public bool EqualAxis;

        [Obsolete("Use AxisAuto()", true)]
        public double[] AutoAxis() => null;

        [Obsolete("Use AxisAuto()", true)]
        public double[] AutoScale() => null;


        [Obsolete("Individual axes (e.g., XAxis and YAxis) have their own tick configuration methods", true)]
        public void Ticks(
            bool? displayTicksX = null,
            bool? displayTicksY = null,
            bool? displayTicksXminor = null,
            bool? displayTicksYminor = null,
            bool? displayTickLabelsX = null,
            bool? displayTickLabelsY = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTimeX = null,
            bool? dateTimeY = null,
            bool? rulerModeX = null,
            bool? rulerModeY = null,
            bool? invertSignX = null,
            bool? invertSignY = null,
            string fontName = null,
            float? fontSize = null,
            float? xTickRotation = null,
            bool? logScaleX = null,
            bool? logScaleY = null,
            string numericFormatStringX = null,
            string numericFormatStringY = null,
            bool? snapToNearestPixel = null,
            int? baseX = null,
            int? baseY = null,
            string prefixX = null,
            string prefixY = null,
            string dateTimeFormatStringX = null,
            string dateTimeFormatStringY = null
            ) => throw new NotImplementedException();

        #endregion
    }
}
