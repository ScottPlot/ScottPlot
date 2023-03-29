/* The Axis module seeks to provide a simple facade to a lot of complex logic.
 * 
 * Axes have many functions:
 *   - Unit/Pixel conversions
 *   - Configuring axis limits and boundaries
 *   - Axis labels (XLabel, YLabel, Title, etc)
 *   - Adding multiple axes
 *   - Grid lines
 *   - Tick marks
 *   - Tick labels
 * 
 */

using ScottPlot.Drawing;
using ScottPlot.Ticks;
using System;
using System.Drawing;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// An Axis stores dimensions (axis limits and pixel/unit conversion methods) and can render
    /// itself including axis label, tick marks, tick labels, and grid lines
    /// </summary>
    public class Axis : IRenderable
    {
        /// <summary>
        /// Axis dimensions and methods for pixel/unit conversions
        /// </summary>
        public readonly AxisDimensions Dims = new AxisDimensions();

        /// <summary>
        /// Plottables with this axis index will use pixel/unit conversions from this axis
        /// </summary>
        public int AxisIndex = 0;

        public bool IsVisible { get; set; } = true;

        private Edge _Edge;
        public Edge Edge
        {
            get => _Edge;
            set
            {
                _Edge = value;
                AxisLine.Edge = value;
                AxisLabel.Edge = value;
                AxisTicks.Edge = value;
                bool isVertical = (value == Edge.Left || value == Edge.Right);
                AxisTicks.TickCollection.Orientation = isVertical ? AxisOrientation.Vertical : AxisOrientation.Horizontal;
                Dims.IsInverted = isVertical;
            }
        }
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;

        /// <summary>
        /// Customization options for the text label displayed on an axis
        /// </summary>
        public readonly AxisLabel AxisLabel = new();

        /// <summary>
        /// Customization options for axis tick marks
        /// </summary>
        public readonly AxisTicks AxisTicks = new();

        /// <summary>
        /// Customization options for the line between an axis and the data area
        /// </summary>
        public readonly AxisLine AxisLine = new();

        /// <summary>
        /// Return configuration objects to allow deep customization of axis settings.
        /// WARNING: This API may not be stable across future versions.
        /// </summary>
        public (AxisLabel, AxisTicks, AxisLine) GetSettings(bool showWarning = true)
        {
            if (showWarning)
            {
                System.Diagnostics.Debug.WriteLine(
                    "WARNING: GetSettings() is only for development and testing. " +
                    "Not all features may be fully implemented. " +
                    "Its API may not be stable across future versions.");
            }

            return (AxisLabel, AxisTicks, AxisLine);
        }

        /// <summary>
        /// Reset axis padding to the default values
        /// </summary>
        public void ResetLayout()
        {
            PixelSizeMinimum = 5;
            PixelSizeMaximum = float.PositiveInfinity;
            PixelSizePadding = 3;
        }

        /// <summary>
        /// Define the minimum and maximum limits for the pixel size of this axis
        /// </summary>
        public void SetSizeLimit(float? min = null, float? max = null, float? pad = null)
        {
            PixelSizeMinimum = min ?? PixelSizeMinimum;
            PixelSizeMaximum = max ?? PixelSizeMaximum;
            PixelSizePadding = pad ?? PixelSizePadding;
        }

        /// <summary>
        /// Size this axis to an exact number of pixels
        /// </summary>
        public void SetSizeLimit(float px) => SetSizeLimit(px, px, 0);

        // private styling variables
        private float PixelSize; // how large this axis is
        public float PixelOffset { get; private set; } // distance from the data area
        private bool Collapsed = false; // true if axes are hidden

        private float PixelSizeMinimum = 5; // also defined in ResetLayout()
        private float PixelSizeMaximum = float.PositiveInfinity; // also defined in ResetLayout()
        private float PixelSizePadding = 3; // also defined in ResetLayout()

        /// <summary>
        /// Define how many pixels away from the data area this axis will be.
        /// TightenLayout() populates this value (based on other PixelSize values) to stack axes beside each other.
        /// </summary>
        public void SetOffset(float pixels) => PixelOffset = pixels;

        /// <summary>
        /// Returns the number of pixels occupied by this axis
        /// </summary>
        public float GetSize()
        {
            if (IsVisible == false || Collapsed)
                return 0;
            else
                return PixelSize + PixelSizePadding;
        }

        public override string ToString() => $"{Edge} axis from {Dims.Min} to {Dims.Max}";

        /// <summary>
        /// Use the latest configuration (size, font settings, axis limits) to determine tick mark positions
        /// </summary>
        public void RecalculateTickPositions(PlotDimensions dims) =>
            AxisTicks.TickCollection.Recalculate(dims, AxisTicks.TickLabelFont);

        /// <summary>
        /// Render all components of this axis onto the given Bitmap
        /// </summary>
        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            AxisLabel.PixelSizePadding = PixelSizePadding;
            AxisTicks.PixelOffset = PixelOffset;
            AxisLabel.PixelOffset = PixelOffset;
            AxisLabel.PixelSize = PixelSize;
            AxisLine.PixelOffset = PixelOffset;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            {
                AxisTicks.Render(dims, bmp, lowQuality);
                AxisLabel.Render(dims, bmp, lowQuality);
                AxisLine.Render(dims, bmp, AxisTicks.SnapPx || lowQuality);
            }
        }

        /// <summary>
        /// DateTime format assumes axis represents DateTime.ToOATime() units and displays tick labels accordingly.
        /// </summary>
        public void DateTimeFormat(bool enable) => AxisTicks.TickCollection.LabelFormat =
            enable
            ? ScottPlot.Ticks.TickLabelFormat.DateTime
            : ScottPlot.Ticks.TickLabelFormat.Numeric;

        /// <summary>
        /// Configure the label of this axis
        /// </summary>
        public string Label(string label = null, Color? color = null, float? size = null, bool? bold = null, string fontName = null)
        {
            AxisLabel.IsVisible = true;
            AxisLabel.ImageLabel = null;
            AxisLabel.Label = label ?? AxisLabel.Label;
            AxisLabel.Font.Color = color ?? AxisLabel.Font.Color;
            AxisLabel.Font.Size = size ?? AxisLabel.Font.Size;
            AxisLabel.Font.Bold = bold ?? AxisLabel.Font.Bold;
            AxisLabel.Font.Name = fontName ?? AxisLabel.Font.Name;
            return AxisLabel.Label;
        }

        /// <summary>
        /// Display a custom image as the axis label instead of text
        /// </summary>
        /// <param name="img">The image to display where the label should go</param>
        /// <param name="padInside">pixels of padding between the inner image edge and the data area</param>
        /// <param name="padOutside">pixels of padding between the outer image edge and the figure edge</param>
        public void ImageLabel(Bitmap img, float padInside = 5, float padOutside = 5)
        {

            IsVisible = true;
            AxisLabel.ImageLabel = img;
            AxisLabel.ImagePaddingToDataArea = padInside;
            AxisLabel.ImagePaddingToFigureEdge = padOutside;
        }

        /// <summary>
        /// Set color of every component of this axis (label, line, tick marks, and tick labels)
        /// </summary>
        public void Color(Color color)
        {
            Label(color: color);
            TickLabelStyle(color: color);
            AxisTicks.MajorTickColor = color;
            AxisTicks.MinorTickColor = color;
            AxisLine.Color = color;
        }

        /// <summary>
        /// Use a custom function to generate tick label strings
        /// </summary>
        public void TickLabelFormat(Func<double, string> tickFormatter)
        {
            AxisTicks.TickCollection.ManualTickFormatter = tickFormatter;

            // delete existing custom tick format strings
            AxisTicks.TickCollection.numericFormatString = null;
            AxisTicks.TickCollection.dateTimeFormatString = null;
            AxisTicks.TickCollection.LabelFormat = ScottPlot.Ticks.TickLabelFormat.Numeric;
        }

        /// <summary>
        /// Manually define the string format to use for translating tick positions to tick labels
        /// </summary>
        public void TickLabelFormat(string format, bool dateTimeFormat)
        {
            // delete existing tick formatter function
            AxisTicks.TickCollection.ManualTickFormatter = null;

            if (dateTimeFormat)
            {
                AxisTicks.TickCollection.dateTimeFormatString = format;
                DateTimeFormat(true);
            }
            else
            {
                AxisTicks.TickCollection.numericFormatString = format;
                DateTimeFormat(false);
            }
        }

        /// <summary>
        /// Manually define the string format to use for translating exponential part of a number to corner label
        /// </summary>
        /// <param name="format"> String.Format(format,exponent)</param>
        public void CornerLabelFormat(string format)
        {
            AxisTicks.TickCollection.CornerLabelFormat = format;
        }

        /// <summary>
        /// Customize string settings for the tick labels
        /// </summary>
        public void TickLabelNotation(
            bool? multiplier = null,
            bool? offset = null,
            bool? exponential = null,
            bool? invertSign = null,
            int? radix = null,
            string prefix = null)
        {
            AxisTicks.TickCollection.useMultiplierNotation = multiplier ?? AxisTicks.TickCollection.useMultiplierNotation;
            AxisTicks.TickCollection.useOffsetNotation = offset ?? AxisTicks.TickCollection.useOffsetNotation;
            AxisTicks.TickCollection.useExponentialNotation = exponential ?? AxisTicks.TickCollection.useExponentialNotation;
            AxisTicks.TickCollection.LabelUsingInvertedSign = invertSign ?? AxisTicks.TickCollection.LabelUsingInvertedSign;
            AxisTicks.TickCollection.radix = radix ?? AxisTicks.TickCollection.radix;
            AxisTicks.TickCollection.prefix = prefix ?? AxisTicks.TickCollection.prefix;
        }

        /// <summary>
        /// Define a manual spacing between major ticks (and major grid lines)
        /// </summary>
        public void ManualTickSpacing(double manualSpacing)
        {
            // TODO: cutt X and Y out of this
            AxisTicks.TickCollection.manualSpacingX = manualSpacing;
            AxisTicks.TickCollection.manualSpacingY = manualSpacing;
        }

        /// <summary>
        /// Define a manual spacing between major ticks (and major grid lines) for axes configured to display using DateTime format
        /// </summary>
        public void ManualTickSpacing(double manualSpacing, DateTimeUnit manualSpacingDateTimeUnit)
        {
            ManualTickSpacing(manualSpacing);
            AxisTicks.TickCollection.manualDateTimeSpacingUnitX = manualSpacingDateTimeUnit;
        }

        /// <summary>
        /// Manually define major tick (and grid) positions and labels
        /// </summary>
        public void ManualTickPositions(double[] positions, string[] labels)
        {
            if (positions.Length != labels.Length)
                throw new ArgumentException($"{nameof(positions)} must have the same length as {nameof(labels)}");

            AxisTicks.TickCollection.manualTickPositions = positions;
            AxisTicks.TickCollection.manualTickLabels = labels;
        }

        /// <summary>
        /// Reset previously defined manual tick positions and revert to default automatic tick placement.
        /// </summary>
        public void AutomaticTickPositions()
        {
            AxisTicks.TickCollection.manualTickPositions = null;
            AxisTicks.TickCollection.manualTickLabels = null;
            AxisTicks.TickCollection.additionalTickPositions = null;
            AxisTicks.TickCollection.additionalTickLabels = null;
        }

        /// <summary>
        /// Reset previously defined manual tick positions and revert to default automatic tick placement.
        /// The provided tick positions and labels will be displayed in addition to the automatic ticks.
        /// </summary>
        public void AutomaticTickPositions(double[] additionalTickPositions, string[] additionalTickLabels)
        {
            if (additionalTickPositions is null)
                throw new ArgumentNullException(nameof(additionalTickPositions));

            if (additionalTickLabels is null)
                throw new ArgumentNullException(nameof(additionalTickLabels));

            if (additionalTickLabels.Length != additionalTickLabels.Length)
                throw new ArgumentException("tick positions and labels must be equal length");

            AxisTicks.TickCollection.manualTickPositions = null;
            AxisTicks.TickCollection.manualTickLabels = null;
            AxisTicks.TickCollection.additionalTickPositions = additionalTickPositions;
            AxisTicks.TickCollection.additionalTickLabels = additionalTickLabels;
        }

        /// <summary>
        /// Ruler mode draws long tick marks and offsets tick labels for a ruler appearance
        /// </summary>
        public void RulerMode(bool enable) => AxisTicks.RulerMode = enable;

        /// <summary>
        /// Enable this to snap major ticks (and grid lines) to the nearest pixel to avoid anti-aliasing artifacts
        /// </summary>
        /// <param name="enable"></param>
        public void PixelSnap(bool enable) => AxisTicks.SnapPx = enable;

        /// <summary>
        /// Apply the same color to major and minor tick marks
        /// </summary>
        public void TickMarkColor(Color color)
        {
            AxisTicks.MajorTickColor = color;
            AxisTicks.MinorTickColor = color;
        }

        /// <summary>
        /// Set colors for major and minor tick marks
        /// </summary>
        public void TickMarkColor(Color majorColor, Color minorColor)
        {
            AxisTicks.MajorTickColor = majorColor;
            AxisTicks.MinorTickColor = minorColor;
        }

        /// <summary>
        /// Control whether tick marks point outward or inward
        /// </summary>
        public void TickMarkDirection(bool outward)
        {
            AxisTicks.TicksExtendOutward = outward;
        }

        /// <summary>
        /// Set the culture to use for unit-to-string tick mark conversion
        /// </summary>
        public void SetCulture(System.Globalization.CultureInfo culture) => AxisTicks.TickCollection.Culture = culture;

        /// <summary>
        /// Manually define culture to use for unit-to-string tick mark conversion
        /// </summary>
        public void SetCulture(
            string shortDatePattern = null, string decimalSeparator = null, string numberGroupSeparator = null,
            int? decimalDigits = null, int? numberNegativePattern = null, int[] numberGroupSizes = null) =>
            AxisTicks.TickCollection.SetCulture(shortDatePattern, decimalSeparator, numberGroupSeparator,
                decimalDigits, numberNegativePattern, numberGroupSizes);

        /// <summary>
        /// Customize styling of the tick labels
        /// </summary>
        public void TickLabelStyle(
            Color? color = null,
            string fontName = null,
            float? fontSize = null,
            bool? fontBold = null,
            float? rotation = null)
        {
            AxisTicks.TickLabelFont.Color = color ?? AxisTicks.TickLabelFont.Color;
            AxisTicks.TickLabelFont.Name = fontName ?? AxisTicks.TickLabelFont.Name;
            AxisTicks.TickLabelFont.Size = fontSize ?? AxisTicks.TickLabelFont.Size;
            AxisTicks.TickLabelFont.Bold = fontBold ?? AxisTicks.TickLabelFont.Bold;
            AxisTicks.TickLabelRotation = rotation ?? AxisTicks.TickLabelRotation;
        }

        /// <summary>
        /// Customize styling of the label (without changing its content)
        /// </summary>
        public void LabelStyle(
            Color? color = null,
            string fontName = null,
            float? fontSize = null,
            float? rotation = null)
        {
            AxisLabel.Font.Color = color ?? AxisLabel.Font.Color;
            AxisLabel.Font.Name = fontName ?? AxisLabel.Font.Name;
            AxisLabel.Font.Size = fontSize ?? AxisLabel.Font.Size;
            AxisLabel.Font.Rotation = rotation ?? AxisLabel.Font.Rotation;
        }

        /// <summary>
        /// Set visibility of all ticks
        /// </summary>
        public void Ticks(bool enable)
        {
            AxisTicks.MajorTickVisible = enable;
            AxisTicks.TickLabelVisible = enable;
            AxisTicks.MinorTickVisible = enable;
        }

        /// <summary>
        /// Set visibility of individual tick components
        /// </summary>
        public void Ticks(bool major, bool minor, bool majorLabels = true)
        {
            AxisTicks.MajorTickVisible = major;
            AxisTicks.TickLabelVisible = majorLabels;
            AxisTicks.MinorTickVisible = minor;
        }

        /// <summary>
        /// This value defines the packing density of tick labels.
        /// A density of 1.0 means labels fit tightly based on measured maximum label size.
        /// Higher densities place more ticks but tick labels may oberlap.
        /// </summary>
        public void TickDensity(double ratio = 1.0)
        {
            AxisTicks.TickCollection.TickDensity = (float)ratio;
        }

        /// <summary>
        /// Define the smallest distance between major ticks, grid lines, and tick labels in coordinate units.
        /// This only works for numeric tick systems (DateTime ticks are not supported).
        /// </summary>
        public void MinimumTickSpacing(double spacing)
        {
            AxisTicks.TickCollection.MinimumTickSpacing = spacing;
        }

        /// <summary>
        /// Sets whether minor ticks are evenly spaced or log-distributed between major tick positions
        /// </summary>
        /// <param name="enable">if true, minor tick marks will be logarithmically distributed</param>
        /// <param name="roundMajorTicks">if true, log-scaled ticks will only show as even powers of ten</param>
        /// <param name="minorTickCount">This many minor ticks will be placed between each major tick</param>
        public void MinorLogScale(bool enable, bool roundMajorTicks = true, int minorTickCount = 10)
        {
            if (enable)
            {
                AxisTicks.TickCollection.MinorTickDistribution = MinorTickDistribution.log;
                AxisTicks.TickCollection.IntegerPositionsOnly = roundMajorTicks;
                AxisTicks.TickCollection.LogScaleMinorTickCount = minorTickCount;
            }
            else
            {
                AxisTicks.TickCollection.MinorTickDistribution = MinorTickDistribution.even;
                AxisTicks.TickCollection.IntegerPositionsOnly = false;
            }
        }

        /// <summary>
        /// Configure the line drawn along the edge of the axis
        /// </summary>
        public void Line(bool? visible = null, Color? color = null, float? width = null)
        {
            AxisLine.IsVisible = visible ?? AxisLine.IsVisible;
            AxisLine.Color = color ?? AxisLine.Color;
            AxisLine.Width = width ?? AxisLine.Width;
        }

        /// <summary>
        /// Set the minimum size and padding of the axis
        /// </summary>
        public void Layout(float? padding = null, float? minimumSize = null, float? maximumSize = null)
        {
            PixelSizePadding = padding ?? PixelSizePadding;
            PixelSizeMinimum = minimumSize ?? PixelSizeMinimum;
            PixelSizeMaximum = maximumSize ?? PixelSizeMaximum;
        }

        /// <summary>
        /// Configure visibility and styling of the major grid
        /// </summary>
        public void MajorGrid(
            bool? enable = null,
            Color? color = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null)
        {
            AxisTicks.MajorGridVisible = enable ?? AxisTicks.MajorGridVisible;
            AxisTicks.MajorGridColor = color ?? AxisTicks.MajorGridColor;
            AxisTicks.MajorGridWidth = lineWidth ?? AxisTicks.MajorGridWidth;
            AxisTicks.MajorGridStyle = lineStyle ?? AxisTicks.MajorGridStyle;
        }

        /// <summary>
        /// Configure visibility and styling of the minor grid
        /// </summary>
        public void MinorGrid(
            bool? enable = null,
            Color? color = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null,
            bool? logScale = null)
        {
            AxisTicks.MinorGridVisible = enable ?? AxisTicks.MinorGridVisible;
            AxisTicks.MinorGridColor = color ?? AxisTicks.MinorGridColor;
            AxisTicks.MinorGridWidth = lineWidth ?? AxisTicks.MinorGridWidth;
            AxisTicks.MinorGridStyle = lineStyle ?? AxisTicks.MinorGridStyle;
            if (logScale.HasValue)
                AxisTicks.TickCollection.MinorTickDistribution = logScale.Value
                    ? MinorTickDistribution.log
                    : MinorTickDistribution.even;
        }

        /// <summary>
        /// Hide this axis by forcing its size to always be zero.
        /// </summary>
        public void Hide(bool hidden = true)
        {
            // NOTE: Don't set this.IsVisible because that will control the grid
            AxisTicks.MajorTickVisible = !hidden;
            AxisTicks.MinorTickVisible = !hidden;
            AxisTicks.TickLabelVisible = !hidden;
            AxisLine.IsVisible = !hidden;
            Collapsed = hidden;
        }

        /// <summary>
        /// Set visibility for major tick grid lines
        /// </summary>
        public void Grid(bool enable) => AxisTicks.MajorGridVisible = enable;

        /// <summary>
        /// Set pixel size based on the latest axis label, tick marks, and tick label
        /// </summary>
        public void RecalculateAxisSize()
        {
            if (Collapsed)
            {
                PixelSize = 0;
                return;
            }

            using (var tickFont = GDI.Font(AxisTicks.TickLabelFont))
            using (var titleFont = GDI.Font(AxisLabel.Font))
            {
                PixelSize = 0;

                if (AxisLabel.IsVisible)
                    PixelSize += AxisLabel.Measure().Height;

                if (AxisTicks.TickLabelVisible)
                {
                    // determine how many pixels the largest tick label occupies
                    float maxHeight = AxisTicks.TickCollection.LargestLabelHeight;
                    float maxWidth = AxisTicks.TickCollection.LargestLabelWidth * 1.2f;

                    // calculate the width and height of the rotated label
                    float largerEdgeLength = Math.Max(maxWidth, maxHeight);
                    float shorterEdgeLength = Math.Min(maxWidth, maxHeight);
                    float differenceInEdgeLengths = largerEdgeLength - shorterEdgeLength;
                    double radians = AxisTicks.TickLabelRotation * Math.PI / 180;
                    double fraction = IsHorizontal ? Math.Sin(radians) : Math.Cos(radians);
                    double rotatedSize = shorterEdgeLength + differenceInEdgeLengths * fraction;

                    // add the rotated label size to the size of this axis
                    PixelSize += (float)rotatedSize;
                }

                if (AxisTicks.MajorTickVisible)
                    PixelSize += AxisTicks.MajorTickLength;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
                PixelSize = Math.Min(PixelSize, PixelSizeMaximum);
                PixelSize += PixelSizePadding;
            }
        }

        /// <summary>
        /// Lock min/max limits so it cannot be changed (until it's unlocked)
        /// </summary>
        public void LockLimits(bool locked = true)
        {
            Dims.LockLimits(locked);
        }

        /// <summary>
        /// Return the ticks displayed in the previous render
        /// </summary>
        public Tick[] GetTicks(double min = double.NegativeInfinity, double max = double.PositiveInfinity)
        {
            return AxisTicks.TickCollection.GetTicks(min, max);
        }

        /// <summary>
        /// Configure how tick label measurement is performed when calculating ideal tick density.
        /// </summary>
        /// <param name="manual"></param>
        public void TickMeasurement(bool manual)
        {
            AxisTicks.TickCollection.MeasureStringManually = manual;
        }
    }
}
