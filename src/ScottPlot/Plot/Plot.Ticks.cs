/* Code here customizes tick styling and behavior */
using ScottPlot.Ticks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
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
    }
}
