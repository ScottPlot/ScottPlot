using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        /// <summary>
        /// Customize styling options for the bottom axis (XAxis)
        /// </summary>
        public void XLabel(string label = null, Color? color = null, string fontName = null, float? fontSize = null, bool? bold = null)
        {
            XAxis.Title.Label = label;
            XAxis.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? XAxis.Title.Font.Name : fontName;
            XAxis.Title.Font.Size = fontSize ?? XAxis.Title.Font.Size;
            XAxis.Configure(color: color);
            XAxis.Title.Font.Bold = bold ?? XAxis.Title.Font.Bold;
        }

        /// <summary>
        /// Customize styling options for the left axis (YAxis)
        /// </summary>
        public void YLabel(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null)
        {
            YAxis.Title.Label = label;
            YAxis.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? YAxis.Title.Font.Name : fontName;
            YAxis.Title.Font.Size = fontSize ?? YAxis.Title.Font.Size;
            YAxis.Configure(color: color);
            YAxis.Title.Font.Bold = bold ?? YAxis.Title.Font.Bold;
        }

        /// <summary>
        /// Customize styling options for the right axis (YAxis2)
        /// </summary>
        public void YLabel2(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null)
        {
            YAxis2.Title.Label = label;
            YAxis2.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? YAxis2.Title.Font.Name : fontName;
            YAxis2.Title.Font.Size = fontSize ?? YAxis2.Title.Font.Size;
            YAxis2.Configure(color: color);
            YAxis2.Title.Font.Bold = bold ?? YAxis2.Title.Font.Bold;
            YAxis2.Title.IsVisible = true;
            YAxis2.Ticks.MajorTickEnable = true;
            YAxis2.Ticks.MinorTickEnable = true;
            YAxis2.Ticks.MajorLabelEnable = true;
        }

        /// <summary>
        /// Customize styling options for the top axis (XAxis2)
        /// </summary>
        public void XLabel2(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null)
        {
            XAxis2.Title.Label = label;
            XAxis2.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? XAxis2.Title.Font.Name : fontName;
            XAxis2.Title.Font.Size = fontSize ?? XAxis2.Title.Font.Size;
            XAxis2.Configure(color: color);
            XAxis2.Title.Font.Bold = bold ?? XAxis2.Title.Font.Bold;
            XAxis2.Title.IsVisible = true;
            XAxis2.Ticks.MajorTickEnable = true;
            XAxis2.Ticks.MinorTickEnable = true;
            XAxis2.Ticks.MajorLabelEnable = true;
        }

        /// <summary>
        /// Customize styling options for title (which is just the axis label for the top axis, XAxis2)
        /// </summary>
        public void Title(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null)
        {
            XAxis2.Title.Label = label;
            XAxis2.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? XAxis2.Title.Font.Name : fontName;
            XAxis2.Title.Font.Size = fontSize ?? XAxis2.Title.Font.Size;
            XAxis2.Title.Font.Color = color ?? XAxis2.Title.Font.Color;
            XAxis2.Title.Font.Bold = bold ?? XAxis2.Title.Font.Bold;
        }

        /// <summary>
        /// Control color and visibility of the plot area outlines (frame)
        /// </summary>
        public void Frame(bool? drawFrame = true, System.Drawing.Color? frameColor = null,
            bool? left = true, bool? right = true, bool? bottom = true, bool? top = true)
        {
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };
            foreach (var axis in primaryAxes)
            {
                axis.Line.IsVisible = drawFrame ?? axis.Line.IsVisible;
                axis.Line.Color = frameColor ?? axis.Line.Color;
            }

            YAxis.Line.IsVisible = left ?? YAxis.Line.IsVisible;
            YAxis2.Line.IsVisible = right ?? YAxis2.Line.IsVisible;
            XAxis.Line.IsVisible = bottom ?? XAxis.Line.IsVisible;
            XAxis2.Line.IsVisible = top ?? XAxis2.Line.IsVisible;
        }

        /// <summary>
        /// Disable visibility of all axes and set their size and padding to zero so the data area covers the whole figure
        /// </summary>
        public void LayoutFrameless()
        {
            foreach (var axis in settings.Axes)
            {
                axis.IsVisible = false;
                axis.PixelSizeMinimum = 0;
                axis.PixelSizeMaximum = 0;
                axis.PixelSizePadding = 0;
            }
        }

        /// <summary>
        /// Customize styling options for the primary X and Y major grid lines
        /// </summary>
        public void Grid(
            bool? enable = null,
            Color? color = null,
            double? xSpacing = null,
            double? ySpacing = null,
            bool? enableHorizontal = null,
            bool? enableVertical = null,
            Ticks.DateTimeUnit? xSpacingDateTimeUnit = null,
            Ticks.DateTimeUnit? ySpacingDateTimeUnit = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null,
            bool? snapToNearestPixel = null)
        {
            XAxis.Ticks.MajorGridEnable = enable ?? XAxis.Ticks.MajorGridEnable;
            YAxis.Ticks.MajorGridEnable = enable ?? YAxis.Ticks.MajorGridEnable;
            XAxis.Ticks.MajorGridEnable = enableHorizontal ?? XAxis.Ticks.MajorGridEnable;
            YAxis.Ticks.MajorGridEnable = enableVertical ?? YAxis.Ticks.MajorGridEnable;

            XAxis.Ticks.MajorGridColor = color ?? XAxis.Ticks.MajorGridColor;
            YAxis.Ticks.MajorGridColor = color ?? YAxis.Ticks.MajorGridColor;

            XAxis.Ticks.TickCollection.manualSpacingX = xSpacing ?? XAxis.Ticks.TickCollection.manualSpacingX;
            YAxis.Ticks.TickCollection.manualSpacingY = ySpacing ?? YAxis.Ticks.TickCollection.manualSpacingY;
            XAxis.Ticks.TickCollection.manualDateTimeSpacingUnitX = xSpacingDateTimeUnit ?? XAxis.Ticks.TickCollection.manualDateTimeSpacingUnitX;
            YAxis.Ticks.TickCollection.manualDateTimeSpacingUnitY = ySpacingDateTimeUnit ?? YAxis.Ticks.TickCollection.manualDateTimeSpacingUnitY;

            XAxis.Ticks.MajorGridWidth = lineWidth ?? XAxis.Ticks.MajorGridWidth;
            YAxis.Ticks.MajorGridWidth = lineWidth ?? YAxis.Ticks.MajorGridWidth;

            XAxis.Ticks.MajorGridStyle = lineStyle ?? XAxis.Ticks.MajorGridStyle;
            YAxis.Ticks.MajorGridStyle = lineStyle ?? YAxis.Ticks.MajorGridStyle;

            XAxis.Ticks.SnapPx = snapToNearestPixel ?? XAxis.Ticks.SnapPx;
            YAxis.Ticks.SnapPx = snapToNearestPixel ?? YAxis.Ticks.SnapPx;
        }

        /// <summary>
        /// Set padding around the data area by defining the minimum size and padding for all axes
        /// </summary>
        public void Layout(float? left = null, float? right = null, float? bottom = null, float? top = null, float? padding = 5)
        {
            foreach (var axis in settings.Axes)
            {
                if (axis.Edge == Renderable.Edge.Left) axis.PixelSizeMinimum = left ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Right) axis.PixelSizeMinimum = right ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Bottom) axis.PixelSizeMinimum = bottom ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Top) axis.PixelSizeMinimum = top ?? axis.PixelSizeMinimum;
                axis.PixelSizePadding = padding ?? axis.PixelSizePadding;
            }
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
                YAxis.PixelSize = sourceSettings.YAxis.PixelSize;
                YAxis2.PixelSize = sourceSettings.YAxis2.PixelSize;
            }

            if (vertical)
            {
                XAxis.PixelSize = sourceSettings.XAxis.PixelSize;
                XAxis2.PixelSize = sourceSettings.XAxis2.PixelSize;
            }
        }

        /// <summary>
        /// Manually define X axis tick labels
        /// </summary>
        public void XTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            XTicks(DataGen.Consecutive(labels.Length), labels);
        }

        /// <summary>
        /// Manually define X axis tick positions and labels
        /// </summary>
        public void XTicks(double[] positions = null, string[] labels = null)
        {
            settings.XAxis.Ticks.TickCollection.manualTickPositions = positions;
            settings.XAxis.Ticks.TickCollection.manualTickLabels = labels;
        }

        /// <summary>
        /// Manually define Y axis tick labels
        /// </summary>
        public void YTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            YTicks(DataGen.Consecutive(labels.Length), labels);
        }

        /// <summary>
        /// Manually define Y axis tick positions and labels
        /// </summary>
        public void YTicks(double[] positions = null, string[] labels = null)
        {
            settings.YAxis.Ticks.TickCollection.manualTickPositions = positions;
            settings.YAxis.Ticks.TickCollection.manualTickLabels = labels;
        }

        /// <summary>
        /// Configure the style and behavior of X and Y ticks
        /// </summary>
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
            double? xTickRotation = null,
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
            )
        {
            settings.XAxis.Configure(showMajorTicks: displayTicksX);
            settings.XAxis.Configure(showMinorTicks: displayTicksX);
            settings.XAxis.Configure(showLabels: displayTicksX);
            settings.YAxis.Configure(showMajorTicks: displayTicksY);
            settings.YAxis.Configure(showMinorTicks: displayTicksY);
            settings.YAxis.Configure(showLabels: displayTicksY);

            settings.XAxis.Configure(showMinorTicks: displayTicksXminor);
            settings.YAxis.Configure(showMinorTicks: displayTicksYminor);
            settings.XAxis.Configure(showLabels: displayTickLabelsX);
            settings.YAxis.Configure(showLabels: displayTickLabelsY);

            settings.XAxis.Configure(color: color);
            settings.YAxis.Configure(color: color);

            settings.XAxis.Configure(useMultiplierNotation: useMultiplierNotation);
            settings.YAxis.Configure(useMultiplierNotation: useMultiplierNotation);
            settings.XAxis.Configure(useOffsetNotation: useOffsetNotation);
            settings.YAxis.Configure(useOffsetNotation: useOffsetNotation);
            settings.XAxis.Configure(useExponentialNotation: useExponentialNotation);
            settings.YAxis.Configure(useExponentialNotation: useExponentialNotation);

            settings.XAxis.Configure(dateTime: dateTimeX);
            settings.YAxis.Configure(dateTime: dateTimeY);

            settings.XAxis.Configure(rulerMode: rulerModeX);
            settings.YAxis.Configure(rulerMode: rulerModeY);
            settings.XAxis.Configure(invertSign: invertSignX);
            settings.YAxis.Configure(invertSign: invertSignY);

            settings.XAxis.Configure(fontName: fontName);
            settings.YAxis.Configure(fontName: fontName);
            settings.XAxis.Configure(fontSize: fontSize);
            settings.YAxis.Configure(fontSize: fontSize);

            settings.XAxis.Configure(rotation: xTickRotation);

            settings.XAxis.Configure(logScale: logScaleX);
            settings.YAxis.Configure(logScale: logScaleY);

            settings.XAxis.Configure(numericFormatString: numericFormatStringX);
            settings.YAxis.Configure(numericFormatString: numericFormatStringY);

            settings.XAxis.Configure(snapToNearestPixel: snapToNearestPixel);
            settings.YAxis.Configure(snapToNearestPixel: snapToNearestPixel);

            settings.XAxis.Configure(radix: baseX);
            settings.YAxis.Configure(radix: baseY);

            settings.XAxis.Configure(prefix: prefixX);
            settings.YAxis.Configure(prefix: prefixY);

            settings.XAxis.Configure(dateTimeFormatString: dateTimeFormatStringX);
            settings.YAxis.Configure(dateTimeFormatString: dateTimeFormatStringY);
        }
    }
}
