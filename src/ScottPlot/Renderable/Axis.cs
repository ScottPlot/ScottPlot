using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

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
                Line.Edge = value;
                Title.Edge = value;
                Ticks.Edge = value;
                bool isVertical = (value == Edge.Left || value == Edge.Right);
                Ticks.TickCollection.verticalAxis = isVertical;
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

        public readonly AxisTitle Title = new AxisTitle();
        public readonly AxisTicks Ticks = new AxisTicks();
        public readonly AxisLine Line = new AxisLine();

        // shortcuts allow components to be customized without having to reach in too far
        public string Label { get => Title.Label; set => ConfigureAxisLabel(visible: value != null, label: value); }
        public Color Color { get => Title.Font.Color; set => Configure(color: value); }
        public bool DateTime { get => Ticks.TickCollection.dateFormat; set => Ticks.TickCollection.dateFormat = value; }
        public bool Grid { get => Ticks.MajorGridEnable; set => Ticks.MajorGridEnable = value; }

        public override string ToString() => $"{Edge} axis from {Dims.Min} to {Dims.Max}";

        public void RecalculateTickPositions(PlotDimensions dims)
        {
            Ticks.TickCollection.Recalculate(dims, Ticks.MajorLabelFont);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            Title.PixelSizePadding = PixelSizePadding;
            Ticks.PixelOffset = PixelOffset;
            Title.PixelOffset = PixelOffset;
            Title.PixelSize = PixelSize;
            Line.PixelOffset = PixelOffset;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            {
                Ticks.Render(dims, bmp, lowQuality);
                Title.Render(dims, bmp, lowQuality);
                Line.Render(dims, bmp, lowQuality);
            }
        }

        /// <summary>
        /// Customize the axis label visibility, content, and styling
        /// </summary>
        public void ConfigureAxisLabel(
            bool? visible = null,
            string label = null,
            Color? color = null,
            float? fontSize = null,
            bool? fontBold = null,
            string fontName = null)
        {
            Title.IsVisible = visible ?? Title.IsVisible;
            Title.Label = label ?? Title.Label;
            Title.Font.Color = color ?? Title.Font.Color;
            Title.Font.Size = fontSize ?? Title.Font.Size;
            Title.Font.Bold = fontBold ?? Title.Font.Bold;
            Title.Font.Name = fontName ?? Title.Font.Name;
        }

        /// <summary>
        /// Customize string settings for the tick labels
        /// </summary>
        public void ConfigureTickLabelNotation(
            bool? dateTime = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? invertSign = null,
            string customFormatStringNumeric = null,
            string customFormatStringDateTime = null,
            int? radix = null,
            string prefix = null)
        {
            Ticks.TickCollection.useMultiplierNotation = useMultiplierNotation ?? Ticks.TickCollection.useMultiplierNotation;
            Ticks.TickCollection.useOffsetNotation = useOffsetNotation ?? Ticks.TickCollection.useOffsetNotation;
            Ticks.TickCollection.useExponentialNotation = useExponentialNotation ?? Ticks.TickCollection.useExponentialNotation;
            Ticks.TickCollection.dateFormat = dateTime ?? Ticks.TickCollection.dateFormat;
            Ticks.TickCollection.invertSign = invertSign ?? Ticks.TickCollection.invertSign;
            Ticks.TickCollection.numericFormatString = customFormatStringNumeric ?? Ticks.TickCollection.numericFormatString;
            Ticks.TickCollection.dateTimeFormatString = customFormatStringDateTime ?? Ticks.TickCollection.dateTimeFormatString;
            Ticks.TickCollection.radix = radix ?? Ticks.TickCollection.radix;
            Ticks.TickCollection.prefix = prefix ?? Ticks.TickCollection.prefix;
        }

        /// <summary>
        /// Customize styling of the tick labels
        /// </summary>
        public void ConfigureTickLabelStyle(
            Color? color = null,
            bool? rulerMode = null,
            string fontName = null,
            float? fontSize = null,
            bool? fontBold = null,
            float? rotation = null,
            bool? snapToNearestPixel = null,
            bool? logScale = null)
        {
            Ticks.Color = color ?? Ticks.Color;
            Ticks.RulerMode = rulerMode ?? Ticks.RulerMode;
            Ticks.MajorLabelFont.Name = fontName ?? Ticks.MajorLabelFont.Name;
            Ticks.MajorLabelFont.Size = fontSize ?? Ticks.MajorLabelFont.Size;
            Ticks.MajorLabelFont.Bold = fontBold ?? Ticks.MajorLabelFont.Bold;
            Ticks.Rotation = rotation ?? Ticks.Rotation;
            Ticks.SnapPx = snapToNearestPixel ?? Ticks.SnapPx;
            Ticks.TickCollection.logScale = logScale ?? Ticks.TickCollection.logScale;
        }

        /// <summary>
        /// Configure tick visibility and positioning
        /// </summary>
        public void ConfigureTicks(
            bool? majorTickMarks = null,
            bool? majorTickLabels = null,
            bool? minorTickMarks = null,
            double? manualSpacing = null,
            Ticks.DateTimeUnit? manualSpacingDateTimeUnit = null,
            double[] definedPositions = null,
            string[] definedLabels = null)
        {
            Ticks.MajorTickEnable = majorTickMarks ?? Ticks.MajorTickEnable;
            Ticks.MajorLabelEnable = majorTickLabels ?? Ticks.MajorLabelEnable;
            Ticks.MinorTickEnable = minorTickMarks ?? Ticks.MinorTickEnable;
            Ticks.TickCollection.manualSpacingX = manualSpacing ?? Ticks.TickCollection.manualSpacingX;
            Ticks.TickCollection.manualDateTimeSpacingUnitX = manualSpacingDateTimeUnit ?? Ticks.TickCollection.manualDateTimeSpacingUnitX;
            Ticks.TickCollection.manualTickPositions = definedPositions;
            Ticks.TickCollection.manualTickLabels = definedLabels;
        }

        /// <summary>
        /// Configure the line drawn along the edge of the axis
        /// </summary>
        public void ConfigureLine(bool? visible = null, Color? color = null, float? width = null)
        {
            Line.IsVisible = visible ?? Line.IsVisible;
            Line.Color = color ?? Line.Color;
            Line.Width = width ?? Line.Width;
        }

        /// <summary>
        /// Set the minimum size and padding of the axis
        /// </summary>
        public void ConfigureLayout(float? padding = null, float? minimumSize = null, float? maximumSize = null)
        {
            PixelSizePadding = padding ?? PixelSizePadding;
            PixelSizeMinimum = minimumSize ?? PixelSizeMinimum;
            PixelSizeMaximum = maximumSize ?? PixelSizeMaximum;
        }

        /// <summary>
        /// Configure visibility and styling of the major grid
        /// </summary>
        public void ConfigureMajorGrid(
            bool? enable = null,
            Color? color = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null)
        {
            Ticks.MajorGridEnable = enable ?? Ticks.MajorGridEnable;
            Ticks.MajorGridColor = color ?? Ticks.MajorGridColor;
            Ticks.MajorGridWidth = lineWidth ?? Ticks.MajorGridWidth;
            Ticks.MajorGridStyle = lineStyle ?? Ticks.MajorGridStyle;
        }

        public void ConfigureMinorGrid(
            bool? enable = null,
            Color? color = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null)
        {
            Ticks.MinorGridEnable = enable ?? Ticks.MinorGridEnable;
            Ticks.MinorGridColor = color ?? Ticks.MinorGridColor;
            Ticks.MinorGridWidth = lineWidth ?? Ticks.MinorGridWidth;
            Ticks.MinorGridStyle = lineStyle ?? Ticks.MinorGridStyle;
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
        /// High-level configuration for axis label, tick labels, and all tick lines
        /// </summary>
        public void Configure(Color? color = null, bool? ticks = null, bool ?grid = null)
        {
            if (color.HasValue)
            {
                ConfigureAxisLabel(color: color);
                ConfigureTickLabelStyle(color: color);
                Ticks.Color = color.Value;
                Line.Color = color.Value;
            }

            if (ticks.HasValue)
            {
                Ticks.MajorTickEnable = ticks.Value;
                Ticks.MinorTickEnable = ticks.Value;
                Ticks.MajorLabelEnable = ticks.Value;
            }

            if (grid.HasValue)
            {
                Ticks.MajorGridEnable = grid.Value;
                Ticks.MinorTickEnable = grid.Value;
            }
        }

        public void RecalculateAxisSize()
        {
            using (var tickFont = GDI.Font(Ticks.MajorLabelFont))
            using (var titleFont = GDI.Font(Title.Font))
            {
                PixelSize = 0;

                if (Title.IsVisible)
                    PixelSize += GDI.MeasureString(Title.Label, Title.Font).Height;

                if (Ticks.MajorLabelEnable)
                    PixelSize += IsHorizontal ? Ticks.TickCollection.maxLabelHeight : Ticks.TickCollection.maxLabelWidth * 1.2f;

                if (Ticks.MajorTickEnable)
                    PixelSize += Ticks.MajorTickLength;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
                PixelSize = Math.Min(PixelSize, PixelSizeMaximum);
                PixelSize += PixelSizePadding;
            }
        }
    }
}
