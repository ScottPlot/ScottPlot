using System;
using System.Drawing;

namespace ScottPlot
{
    partial class Plot
    {
        /// <summary>
        /// Set the label for the vertical axis to the right of the plot (XAxis). 
        /// </summary>
        public void XLabel(string label) => XAxis.SetLabel(label);

        /// <summary>
        /// Set the label for the vertical axis to the right of the plot (YAxis2). 
        /// </summary>
        public void YLabel(string label) => YAxis.SetLabel(label);

        /// <summary>
        /// Set the label for the horizontal axis above the plot (XAxis2). 
        /// </summary>
        public void Title(string label, bool bold = true) => XAxis2.SetLabel(label, bold: bold);

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
            XAxis.SetTickPositions(positions, labels);

        /// <summary>
        /// Manually define Y axis tick labels
        /// </summary>
        public void YTicks(string[] labels) => YTicks(DataGen.Consecutive(labels.Length), labels);

        /// <summary>
        /// Manually define Y axis tick positions and labels
        /// </summary>
        public void YTicks(double[] positions = null, string[] labels = null) =>
            YAxis.SetTickPositions(positions, labels);

        /// <summary>
        /// Configure the style and behavior of X and Y ticks
        /// </summary>
        [Obsolete("Do not use this method! Call methods of individual axes (e.g., XAxis and YAxis)", true)]
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
            //settings.XAxis.ConfigureTicks(majorTickMarks: displayTicksX);
            //settings.YAxis.ConfigureTicks(majorTickMarks: displayTicksY);
            //settings.XAxis.ConfigureTicks(minorTickMarks: displayTicksXminor);
            //settings.YAxis.ConfigureTicks(minorTickMarks: displayTicksYminor);
            //settings.XAxis.ConfigureTicks(majorTickLabels: displayTickLabelsX);
            //settings.YAxis.ConfigureTicks(majorTickLabels: displayTickLabelsY);

            // AXIS LABEL
            settings.XAxis.TickLabelStyle(fontName: fontName);
            settings.YAxis.TickLabelStyle(fontName: fontName);
            settings.XAxis.TickLabelStyle(fontSize: fontSize);
            settings.YAxis.TickLabelStyle(fontSize: fontSize);

            // TICK LABEL NOTATION
            settings.XAxis.TickLabelNotation(multiplier: useMultiplierNotation);
            settings.YAxis.TickLabelNotation(multiplier: useMultiplierNotation);
            settings.XAxis.TickLabelNotation(offset: useOffsetNotation);
            settings.YAxis.TickLabelNotation(offset: useOffsetNotation);
            settings.XAxis.TickLabelNotation(exponential: useExponentialNotation);
            settings.YAxis.TickLabelNotation(exponential: useExponentialNotation);
            //settings.XAxis.TickLabelNotation(dateTime: dateTimeX);
            //settings.YAxis.TickLabelNotation(dateTime: dateTimeY);
            settings.XAxis.TickLabelNotation(invertSign: invertSignX);
            settings.YAxis.TickLabelNotation(invertSign: invertSignY);
            //settings.XAxis.SetTickLabelFormat(numericFormatStringX ?? dateTimeFormatStringX);
            //settings.YAxis.SetTickLabelFormat(numericFormatStringY ?? dateTimeFormatStringY);
            settings.XAxis.TickLabelNotation(radix: baseX);
            settings.YAxis.TickLabelNotation(radix: baseY);
            settings.XAxis.TickLabelNotation(prefix: prefixX);
            settings.YAxis.TickLabelNotation(prefix: prefixY);

            // TICK STYLING
            //settings.XAxis.ConfigureTickLabelStyle(rulerMode: rulerModeX);
            //settings.YAxis.ConfigureTickLabelStyle(rulerMode: rulerModeY);
            settings.XAxis.TickLabelStyle(rotation: xTickRotation);
            settings.XAxis.TickLabelStyle(color: color);
            settings.YAxis.TickLabelStyle(color: color);
            //settings.XAxis.SetLogMinorTicks(logScaleX);
            //settings.YAxis.SetLogMinorTicks(logScaleY);
            //settings.XAxis.TickLabelStyle(snapToNearestPixel: snapToNearestPixel);
            //settings.YAxis.TickLabelStyle(snapToNearestPixel: snapToNearestPixel);
        }
    }
}
