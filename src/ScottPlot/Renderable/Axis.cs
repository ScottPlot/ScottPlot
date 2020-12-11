using ScottPlot.Drawing;
using ScottPlot.Ticks;
using System;
using System.Drawing;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        public readonly AxisDimensions Dims = new AxisDimensions();
        public int AxisIndex = 0;
        private Edge _Edge;
        public Edge Edge
        {
            get => _Edge;
            set
            {
                _Edge = value;
                AxLine.Edge = value;
                AxTitle.Edge = value;
                AxTicks.Edge = value;
                bool isVertical = (value == Edge.Left || value == Edge.Right);
                AxTicks.TickCollection.verticalAxis = isVertical;
                Dims.IsInverted = isVertical;
            }
        }
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;
        public bool IsVisible { get; set; } = true;

        public float PixelOffset; // TightenLayout() populates this value based on other PixelSize values
        public float PixelSize; // RecalculateAxisSize() populates this value
        public float PixelSizeMinimum = 5;
        public float PixelSizeMaximum = float.PositiveInfinity;
        public float PixelSizePadding = 3;

        public readonly AxisTitle AxTitle = new AxisTitle();
        public readonly AxisTicks AxTicks = new AxisTicks();
        public readonly AxisLine AxLine = new AxisLine();

        public override string ToString() => $"{Edge} axis from {Dims.Min} to {Dims.Max}";

        public void RecalculateTickPositions(PlotDimensions dims)
        {
            AxTicks.TickCollection.Recalculate(dims, AxTicks.MajorLabelFont);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            AxTitle.PixelSizePadding = PixelSizePadding;
            AxTicks.PixelOffset = PixelOffset;
            AxTitle.PixelOffset = PixelOffset;
            AxTitle.PixelSize = PixelSize;
            AxLine.PixelOffset = PixelOffset;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            {
                AxTicks.Render(dims, bmp, lowQuality);
                AxTitle.Render(dims, bmp, lowQuality);
                AxLine.Render(dims, bmp, lowQuality);
            }
        }

        /// <summary>
        /// DateTime format assumes axis represents DateTime.ToOATime() units and displays tick labels accordingly.
        /// </summary>
        public void DateTimeFormat(bool enable)
        {
            AxTicks.TickCollection.dateFormat = enable;
        }

        /// <summary>
        /// Configure the label of this axis
        /// </summary>
        public void Label(string label = null, Color? color = null, float? size = null, bool? bold = null, string fontName = null)
        {
            AxTitle.IsVisible = true;
            AxTitle.Label = label ?? AxTitle.Label;
            AxTitle.Font.Color = color ?? AxTitle.Font.Color;
            AxTitle.Font.Size = size ?? AxTitle.Font.Size;
            AxTitle.Font.Bold = bold ?? AxTitle.Font.Bold;
            AxTitle.Font.Name = fontName ?? AxTitle.Font.Name;
        }

        /// <summary>
        /// Set color of every component of this axis (label, line, tick marks, and tick labels)
        /// </summary>
        public void Color(Color color)
        {
            Label(color: color);
            TickLabelStyle(color: color);
            AxTicks.Color = color;
            AxLine.Color = color;
        }

        /// <summary>
        /// Manually define the string format to use for translating tick positions to tick labels
        /// </summary>
        public void TickLabelFormat(string format, bool dateTimeFormat)
        {
            if (dateTimeFormat)
            {
                AxTicks.TickCollection.dateTimeFormatString = format;
                DateTimeFormat(true);
            }
            else
            {
                AxTicks.TickCollection.numericFormatString = format;
                DateTimeFormat(false);
            }
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
            AxTicks.TickCollection.useMultiplierNotation = multiplier ?? AxTicks.TickCollection.useMultiplierNotation;
            AxTicks.TickCollection.useOffsetNotation = offset ?? AxTicks.TickCollection.useOffsetNotation;
            AxTicks.TickCollection.useExponentialNotation = exponential ?? AxTicks.TickCollection.useExponentialNotation;
            AxTicks.TickCollection.invertSign = invertSign ?? AxTicks.TickCollection.invertSign;
            AxTicks.TickCollection.radix = radix ?? AxTicks.TickCollection.radix;
            AxTicks.TickCollection.prefix = prefix ?? AxTicks.TickCollection.prefix;
        }

        /// <summary>
        /// Define a manual spacing between major ticks (and major grid lines)
        /// </summary>
        public void ManualTickSpacing(double manualSpacing)
        {
            // TODO: cutt X and Y out of this
            AxTicks.TickCollection.manualSpacingX = manualSpacing;
            AxTicks.TickCollection.manualSpacingY = manualSpacing;
        }

        /// <summary>
        /// Define a manual spacing between major ticks (and major grid lines) for axes configured to display using DateTime format
        /// </summary>
        public void ManualTickSpacing(double manualSpacing, DateTimeUnit manualSpacingDateTimeUnit)
        {
            ManualTickSpacing(manualSpacing);
            AxTicks.TickCollection.manualDateTimeSpacingUnitX = manualSpacingDateTimeUnit;
        }

        /// <summary>
        /// Manually define major tick (and grid) positions and labels
        /// </summary>
        public void ManualTickPositions(double[] positions, string[] labels)
        {
            AxTicks.TickCollection.manualTickPositions = positions;
            AxTicks.TickCollection.manualTickLabels = labels;
        }

        /// <summary>
        /// Ruler mode draws long tick marks and offsets tick labels for a ruler appearance
        /// </summary>
        public void RulerMode(bool enable) => AxTicks.RulerMode = enable;

        /// <summary>
        /// Enable this to snap major ticks (and grid lines) to the nearest pixel to avoid anti-aliasing artifacts
        /// </summary>
        /// <param name="enable"></param>
        public void PixelSnap(bool enable) => AxTicks.SnapPx = enable;

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
            AxTicks.Color = color ?? AxTicks.Color;
            AxTicks.MajorLabelFont.Name = fontName ?? AxTicks.MajorLabelFont.Name;
            AxTicks.MajorLabelFont.Size = fontSize ?? AxTicks.MajorLabelFont.Size;
            AxTicks.MajorLabelFont.Bold = fontBold ?? AxTicks.MajorLabelFont.Bold;
            AxTicks.Rotation = rotation ?? AxTicks.Rotation;
        }

        /// <summary>
        /// Set visibility of all ticks
        /// </summary>
        public void Ticks(bool enable)
        {
            AxTicks.MajorTickEnable = enable;
            AxTicks.MajorLabelEnable = enable;
            AxTicks.MinorTickEnable = enable;
        }

        /// <summary>
        /// Set visibility of individual tick components
        /// </summary>
        public void Ticks(bool major, bool minor = true, bool majorLabels = true)
        {
            AxTicks.MajorTickEnable = major;
            AxTicks.MajorLabelEnable = major && majorLabels;
            AxTicks.MinorTickEnable = minor;
        }

        /// <summary>
        /// Sets whether minor ticks are evenly spaced or log-distributed between major tick positions
        /// </summary>
        public void MinorLogScale(bool enable) => AxTicks.TickCollection.logScale = enable;

        /// <summary>
        /// Configure the line drawn along the edge of the axis
        /// </summary>
        public void Line(bool? visible = null, Color? color = null, float? width = null)
        {
            AxLine.IsVisible = visible ?? AxLine.IsVisible;
            AxLine.Color = color ?? AxLine.Color;
            AxLine.Width = width ?? AxLine.Width;
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
            AxTicks.MajorGridEnable = enable ?? AxTicks.MajorGridEnable;
            AxTicks.MajorGridColor = color ?? AxTicks.MajorGridColor;
            AxTicks.MajorGridWidth = lineWidth ?? AxTicks.MajorGridWidth;
            AxTicks.MajorGridStyle = lineStyle ?? AxTicks.MajorGridStyle;
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
            AxTicks.MinorGridEnable = enable ?? AxTicks.MinorGridEnable;
            AxTicks.MinorGridColor = color ?? AxTicks.MinorGridColor;
            AxTicks.MinorGridWidth = lineWidth ?? AxTicks.MinorGridWidth;
            AxTicks.MinorGridStyle = lineStyle ?? AxTicks.MinorGridStyle;
            AxTicks.TickCollection.logScale = logScale ?? AxTicks.TickCollection.logScale;
        }

        /// <summary>
        /// Disable all visibility and set size to 0px
        /// </summary>
        public void Hide()
        {
            IsVisible = false;
            PixelSizeMinimum = 0;
            PixelSizeMaximum = 0;
            PixelSizePadding = 0;
        }

        /// <summary>
        /// Set visibility for major tick grid lines
        /// </summary>
        /// <param name="enable"></param>
        public void Grid(bool enable)
        {
            AxTicks.MajorGridEnable = enable;
            AxTicks.MinorTickEnable = enable;
        }

        public void RecalculateAxisSize()
        {
            using (var tickFont = GDI.Font(AxTicks.MajorLabelFont))
            using (var titleFont = GDI.Font(AxTitle.Font))
            {
                PixelSize = 0;

                if (AxTitle.IsVisible)
                    PixelSize += GDI.MeasureString(AxTitle.Label, AxTitle.Font).Height;

                if (AxTicks.MajorLabelEnable)
                    PixelSize += IsHorizontal ? AxTicks.TickCollection.maxLabelHeight : AxTicks.TickCollection.maxLabelWidth * 1.2f;

                if (AxTicks.MajorTickEnable)
                    PixelSize += AxTicks.MajorTickLength;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
                PixelSize = Math.Min(PixelSize, PixelSizeMaximum);
                PixelSize += PixelSizePadding;
            }
        }
    }
}
