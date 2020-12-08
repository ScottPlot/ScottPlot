using System;
using System.Drawing;

namespace ScottPlot
{
    partial class Plot
    {
        /// <summary>
        /// Customize styling options for the bottom axis (XAxis) label
        /// </summary>
        public void XLabel(string label = null, Color? color = null, string fontName = null, float? fontSize = null, bool? bold = null) =>
            XAxis.ConfigureAxisLabel(true, label, color, fontSize, bold, fontName);

        /// <summary>
        /// Customize styling options for the left axis (YAxis) label
        /// </summary>
        public void YLabel(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null) =>
            YAxis.ConfigureAxisLabel(true, label, color, fontSize, bold, fontName);

        /// <summary>
        /// Customize styling options for the right axis (YAxis2) label
        /// </summary>
        public void YLabel2(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null) =>
            YAxis2.ConfigureAxisLabel(true, label, color, fontSize, bold, fontName);

        /// <summary>
        /// Customize styling options for the top axis (XAxis2) label
        /// </summary>
        public void XLabel2(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null) =>
            XAxis2.ConfigureAxisLabel(true, label, color, fontSize, bold, fontName);

        /// <summary>
        /// Customize styling options for title (which is just the axis label for the top axis, XAxis2)
        /// </summary>
        public void Title(string label = null, string fontName = null, float? fontSize = null, Color? color = null, bool? bold = null) =>
            XAxis2.ConfigureAxisLabel(true, label, color, fontSize, bold, fontName);

        /// <summary>
        /// Configure color and visibility of the frame that outlines the data area (lines along the edges of the primary axes)
        /// </summary>
        public void Frame(bool? visible = true, Color? color = null, bool? left = true, bool? right = true, bool? bottom = true, bool? top = true)
        {
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };
            foreach (var axis in primaryAxes)
                axis.ConfigureLine(visible, color);

            YAxis.ConfigureLine(visible: left);
            YAxis2.ConfigureLine(visible: right);
            XAxis.ConfigureLine(visible: bottom);
            XAxis2.ConfigureLine(visible: top);
        }

        /// <summary>
        /// Set size of the primary axes to zero so the data area covers the whole figure
        /// </summary>
        public void LayoutFrameless()
        {
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };
            foreach (var axis in primaryAxes)
                axis.Hide();
        }

        /// <summary>
        /// Disable grid line visibility for the primary X and Y axes
        /// </summary>
        public void DisableGrid()
        {
            XAxis.Configure(grid: false);
            YAxis.Configure(grid: false);
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
            XAxis.ConfigureMajorGrid(enable, color, lineWidth, lineStyle);
            YAxis.ConfigureMajorGrid(enable, color, lineWidth, lineStyle);

            XAxis.ConfigureTickLabelStyle(snapToNearestPixel: snapToNearestPixel);
            YAxis.ConfigureTickLabelStyle(snapToNearestPixel: snapToNearestPixel);

            XAxis.ConfigureMajorGrid(enable: enableVertical);
            YAxis.ConfigureMajorGrid(enable: enableHorizontal);

            XAxis.ConfigureTicks(manualSpacing: xSpacing);
            YAxis.ConfigureTicks(manualSpacing: ySpacing);
            XAxis.ConfigureTicks(manualSpacingDateTimeUnit: xSpacingDateTimeUnit);
            YAxis.ConfigureTicks(manualSpacingDateTimeUnit: ySpacingDateTimeUnit);
        }

        /// <summary>
        /// Set padding around the data area by defining the minimum size and padding for all axes
        /// </summary>
        public void Layout(float? left = null, float? right = null, float? bottom = null, float? top = null, float? padding = 5)
        {
            YAxis.ConfigureLayout(padding, left);
            YAxis2.ConfigureLayout(padding, right);
            XAxis.ConfigureLayout(padding, bottom);
            XAxis2.ConfigureLayout(padding, top);
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
        public void XTicks(string[] labels) => XTicks(DataGen.Consecutive(labels.Length), labels);

        /// <summary>
        /// Manually define X axis tick positions and labels
        /// </summary>
        public void XTicks(double[] positions = null, string[] labels = null) =>
            XAxis.ConfigureTicks(definedPositions: positions, definedLabels: labels);

        /// <summary>
        /// Manually define Y axis tick labels
        /// </summary>
        public void YTicks(string[] labels) => YTicks(DataGen.Consecutive(labels.Length), labels);

        /// <summary>
        /// Manually define Y axis tick positions and labels
        /// </summary>
        public void YTicks(double[] positions = null, string[] labels = null) =>
            YAxis.ConfigureTicks(definedPositions: positions, definedLabels: labels);

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
            )
        {
            // PRIMARY TICK COMPONENTS
            settings.XAxis.ConfigureTicks(majorTickMarks: displayTicksX);
            settings.YAxis.ConfigureTicks(majorTickMarks: displayTicksY);
            settings.XAxis.ConfigureTicks(minorTickMarks: displayTicksXminor);
            settings.YAxis.ConfigureTicks(minorTickMarks: displayTicksYminor);
            settings.XAxis.ConfigureTicks(majorTickLabels: displayTickLabelsX);
            settings.YAxis.ConfigureTicks(majorTickLabels: displayTickLabelsY);

            // AXIS LABEL
            settings.XAxis.ConfigureTickLabelStyle(fontName: fontName);
            settings.YAxis.ConfigureTickLabelStyle(fontName: fontName);
            settings.XAxis.ConfigureTickLabelStyle(fontSize: fontSize);
            settings.YAxis.ConfigureTickLabelStyle(fontSize: fontSize);

            // TICK LABEL NOTATION
            settings.XAxis.ConfigureTickLabelNotation(useMultiplierNotation: useMultiplierNotation);
            settings.YAxis.ConfigureTickLabelNotation(useMultiplierNotation: useMultiplierNotation);
            settings.XAxis.ConfigureTickLabelNotation(useOffsetNotation: useOffsetNotation);
            settings.YAxis.ConfigureTickLabelNotation(useOffsetNotation: useOffsetNotation);
            settings.XAxis.ConfigureTickLabelNotation(useExponentialNotation: useExponentialNotation);
            settings.YAxis.ConfigureTickLabelNotation(useExponentialNotation: useExponentialNotation);
            settings.XAxis.ConfigureTickLabelNotation(dateTime: dateTimeX);
            settings.YAxis.ConfigureTickLabelNotation(dateTime: dateTimeY);
            settings.XAxis.ConfigureTickLabelNotation(invertSign: invertSignX);
            settings.YAxis.ConfigureTickLabelNotation(invertSign: invertSignY);
            settings.XAxis.ConfigureTickLabelNotation(customFormatStringNumeric: numericFormatStringX);
            settings.YAxis.ConfigureTickLabelNotation(customFormatStringNumeric: numericFormatStringY);
            settings.XAxis.ConfigureTickLabelNotation(customFormatStringDateTime: dateTimeFormatStringX);
            settings.YAxis.ConfigureTickLabelNotation(customFormatStringDateTime: dateTimeFormatStringY);
            settings.XAxis.ConfigureTickLabelNotation(radix: baseX);
            settings.YAxis.ConfigureTickLabelNotation(radix: baseY);
            settings.XAxis.ConfigureTickLabelNotation(prefix: prefixX);
            settings.YAxis.ConfigureTickLabelNotation(prefix: prefixY);

            // TICK STYLING
            settings.XAxis.ConfigureTickLabelStyle(rulerMode: rulerModeX);
            settings.YAxis.ConfigureTickLabelStyle(rulerMode: rulerModeY);
            settings.XAxis.ConfigureTickLabelStyle(rotation: xTickRotation);
            settings.XAxis.ConfigureTickLabelStyle(color: color);
            settings.YAxis.ConfigureTickLabelStyle(color: color);
            settings.XAxis.ConfigureTickLabelStyle(logScale: logScaleX);
            settings.YAxis.ConfigureTickLabelStyle(logScale: logScaleY);
            settings.XAxis.ConfigureTickLabelStyle(snapToNearestPixel: snapToNearestPixel);
            settings.YAxis.ConfigureTickLabelStyle(snapToNearestPixel: snapToNearestPixel);
        }
    }
}
